using FootLab.DataAccess;
using FootLab.Entities.Entites;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Collections.Specialized;
using System.Web; // HttpUtility için

namespace FootLab.Bussines.Services
{
    public class TffScraper
    {

        private readonly DataContext _context;

        public TffScraper(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Team>> ScrapeDenizliAmateurTeamsAsync()
        {
            // 1. GENEL SABİTLER
            string currentSeason = "2025-2026";
            string targetLeagueName = "Denizli Amatör Ligleri"; // Tüm amatörleri bu çatı altında topluyoruz
            string targetGroupName = "Genel Takım Havuzu";

            var scrapedTeams = new List<Team>();

            // 2. ÖN HAZIRLIK: Genel Lig ve Grup ID'sini al (Yoksa oluştur)
            var league = await _context.Leagues.FirstOrDefaultAsync(l => l.Name == targetLeagueName);
            if (league == null)
            {
                league = new League { Name = targetLeagueName };
                _context.Leagues.Add(league);
                await _context.SaveChangesAsync();
            }

            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Name == targetGroupName && g.LeagueId == league.Id);
            if (group == null)
            {
                group = new Group { Name = targetGroupName, LeagueId = league.Id };
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
            }

            Guid targetGroupId = group.Id;

            // 3. SELENIUM AYARLARI
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                async Task ApplyFilterAsync()
                {
                    var cityInput = wait.Until(d => d.FindElement(By.Id("ctl00_MPane_m_119_2780_ctnr_m_119_2780_SehirSelector1_combo_Input")));
                    cityInput.Clear();
                    cityInput.SendKeys("DENİZLİ" + Keys.Enter);
                    await Task.Delay(3000);

                    var statusInput = wait.Until(d => d.FindElement(By.Id("ctl00_MPane_m_119_2780_ctnr_m_119_2780_cmbStatu_Input")));
                    statusInput.Clear();
                    statusInput.SendKeys("Amatör" + Keys.Enter);
                    await Task.Delay(2000);

                    driver.FindElement(By.Id("ctl00_MPane_m_119_2780_ctnr_m_119_2780_btnSave")).Click();
                    await Task.Delay(4000);
                }

                try
                {
                    driver.Navigate().GoToUrl("https://www.tff.org/default.aspx?pageID=119");
                    await ApplyFilterAsync();

                    // TFF'de Denizli Amatör listesi genelde 7 sayfadan fazladır. 
                    // Sayfa sayısını dinamik kontrol etmek için p <= 20 gibi yüksek bir sınır koyalım.
                    for (int p = 1; p <= 20; p++)
                    {
                        wait.Until(d => d.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]")).Count > 0);
                        var nodes = driver.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]"));

                        // Adana'ya düşme kontrolü
                        if (nodes.Any(n => n.Text.Contains("ADANA")))
                        {
                            await ApplyFilterAsync();
                            for (int jump = 2; jump <= p; jump++)
                            {
                                var jumpBtn = driver.FindElement(By.XPath($"//td[contains(., 'Sayfalar')]//a[text()='{jump}']"));
                                js.ExecuteScript("arguments[0].click();", jumpBtn);
                                await Task.Delay(3500);
                            }
                            nodes = driver.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]"));
                        }

                        foreach (var node in nodes)
                        {
                            var teamName = node.Text.Trim();
                            if (string.IsNullOrEmpty(teamName) || teamName == "Detay") continue;

                            var href = node.GetAttribute("href");
                            var tffId = href.Contains("kulupID=") ? href.Split("kulupID=")[1].Split('&')[0] : "";

                            var logoUrl = "";
                            try
                            {
                                var row = node.FindElement(By.XPath("./ancestor::tr"));
                                logoUrl = row.FindElement(By.TagName("img")).GetAttribute("src");
                            }
                            catch { logoUrl = null; }

                            if (!scrapedTeams.Any(t => t.TffId == tffId))
                            {
                                scrapedTeams.Add(new Team { Name = teamName, TffId = tffId, LogoUrl = logoUrl });
                            }
                        }

                        // Bir sonraki sayfa var mı kontrolü
                        try
                        {
                            var nextLink = driver.FindElement(By.XPath($"//td[contains(., 'Sayfalar')]//a[text()='{p + 1}']"));
                            js.ExecuteScript("arguments[0].click();", nextLink);
                            await Task.Delay(4000);
                        }
                        catch
                        {
                            // Sayfa numarası bulunamazsa (Listenin sonu) döngüden çık
                            break;
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Scraping Hatası: {ex.Message}"); }
                finally { driver.Quit(); }
            }

            // 4. VERİTABANI KAYIT (TÜM LİSTE İÇİN)
            foreach (var sTeam in scrapedTeams)
            {
                var team = await _context.Teams.FirstOrDefaultAsync(t => t.TffId == sTeam.TffId);
                if (team == null)
                {
                    team = sTeam;
                    _context.Teams.Add(team);
                    await _context.SaveChangesAsync();
                }

                var hasDetail = await _context.TeamSeasonDetails.AnyAsync(tsd =>
                    tsd.TeamId == team.Id && tsd.GroupId == targetGroupId && tsd.Season == currentSeason);

                if (!hasDetail)
                {
                    _context.TeamSeasonDetails.Add(new TeamSeasonDetail
                    {
                        TeamId = team.Id,
                        GroupId = targetGroupId,
                        Season = currentSeason
                    });
                }
            }
            await _context.SaveChangesAsync();
            return scrapedTeams;
        }

        //public async Task<List<Team>> ScrapeDenizliAmateurTeamsAsync(Guid targetGroupId, string currentSeason = "2025-2026")
        //{
        //    var scrapedTeams = new List<Team>();
        //    var options = new ChromeOptions();
        //    options.AddArgument("--headless=new");
        //    // ... diğer options ayarların aynı kalsın ...

        //    using (IWebDriver driver = new ChromeDriver(options))
        //    {
        //        try
        //        {
        //            driver.Navigate().GoToUrl("https://www.tff.org/default.aspx?pageID=119");
        //            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        //            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        //            // Filtreleme fonksiyonun (applyFilter) içeriği aynı kalsın...
        //            await applyFilter();

        //            for (int p = 1; p <= 7; p++)
        //            {
        //                wait.Until(d => d.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]")).Count > 0);
        //                var nodes = driver.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]"));

        //                // ADANA kontrolü ve sayfa atlama mantığın aynı kalsın...

        //                foreach (var node in nodes)
        //                {
        //                    var teamName = node.Text.Trim();
        //                    if (string.IsNullOrEmpty(teamName) || teamName == "Detay") continue;

        //                    var href = node.GetAttribute("href");
        //                    var tffId = href.Contains("kulupID=") ? href.Split("kulupID=")[1].Split('&')[0] : "";

        //                    var logoUrl = "";
        //                    try
        //                    {
        //                        var row = node.FindElement(By.XPath("./ancestor::tr"));
        //                        logoUrl = row.FindElement(By.TagName("img")).GetAttribute("src");
        //                    }
        //                    catch { logoUrl = null; }

        //                    if (!scrapedTeams.Any(t => t.TffId == tffId))
        //                    {
        //                        // --- YENİ MODEL YAPISI ---
        //                        scrapedTeams.Add(new Team
        //                        {
        //                            Name = teamName,
        //                            TffId = tffId,
        //                            LogoUrl = logoUrl
        //                            // LeagueCategory BURADAN KALDIRILDI!
        //                        });
        //                    }
        //                }
        //                // Sayfa değiştirme mantığın aynı kalsın...
        //            }
        //        }
        //        catch (Exception ex) { Console.WriteLine($"Scraping Hatası: {ex.Message}"); }
        //        finally { driver.Quit(); }
        //    }

        //    // --- VERİTABANINA KAYIT AŞAMASI (YENİ MANTIK) ---
        //    try
        //    {
        //        foreach (var sTeam in scrapedTeams)
        //        {
        //            // 1. Takım var mı kontrol et veya ekle
        //            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TffId == sTeam.TffId);
        //            if (team == null)
        //            {
        //                team = sTeam;
        //                _context.Teams.Add(team);
        //                await _context.SaveChangesAsync(); // ID oluşması için
        //            }

        //            // 2. Takımı bu sezona ve gruba bağla (TeamSeasonDetail)
        //            var isAlreadyInGroup = await _context.TeamSeasonDetails.AnyAsync(tsd =>
        //                tsd.TeamId == team.Id &&
        //                tsd.GroupId == targetGroupId &&
        //                tsd.Season == currentSeason);

        //            if (!isAlreadyInGroup)
        //            {
        //                _context.TeamSeasonDetails.Add(new TeamSeasonDetail
        //                {
        //                    TeamId = team.Id,
        //                    GroupId = targetGroupId,
        //                    Season = currentSeason
        //                });
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //        Console.WriteLine("İşlem başarıyla tamamlandı.");
        //    }
        //    catch (Exception ex) { Console.WriteLine($"Hata: {ex.Message}"); }

        //    return scrapedTeams;
        //}


        public async Task<int> ScrapePlayersByTeamSearchAsync()
        {
            var teams = await _context.Teams.Where(t => !t.IsDeleted).ToListAsync();
            int totalSaved = 0;

            var options = new ChromeOptions();
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-blink-features=AutomationControlled");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                driver.Navigate().GoToUrl("https://www.tff.org/Default.aspx?pageID=130");
                await Task.Delay(5000);

                // 1. Sekme Seçimi
                var clubTab = wait.Until(d => d.FindElement(By.XPath("//span[contains(text(),'Kulübe Göre')]")));
                clubTab.Click();
                await Task.Delay(5000);

                // 2. Filtre Seçimleri
                await FiltrSeelect(driver, wait, "_ks_Input", "Amatör");
                await FiltrSeelect(driver, wait, "_f2_Input", "Faal");

                // 3. Takım Döngüsü
                foreach (var team in teams)
                {
                    try
                    {
                        var clubInput = wait.Until(d => {
                            var el = d.FindElement(By.CssSelector("input[id*='txtKulupAdi']"));
                            return el.Displayed ? el : null;
                        });
                        clubInput.Clear();
                        clubInput.SendKeys(team.Name);
                        await Task.Delay(1000);

                        IWebElement searchBtn = null;
                        for (int i = 0; i < 5; i++)
                        {
                            try
                            {
                                // Hem eski hem yeni ID ihtimalini kontrol eder
                                searchBtn = driver.FindElement(By.CssSelector("input[id*='btnSearch2'], input[id*='btnAra']"));
                                if (searchBtn.Displayed) break;
                            }
                            catch { await Task.Delay(1000); }
                        }

                        if (searchBtn != null)
                        {
                            js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", searchBtn);
                            await Task.Delay(500);
                            try { searchBtn.Click(); }
                            catch { js.ExecuteScript("arguments[0].click();", searchBtn); }
                        }
                        else { continue; }

                        Console.WriteLine($"{team.Name} sorgulanıyor...");

                        // 4. Veri Bekleme Senkronizasyonu (Yeni Tablo ID'sine Göre)
                        bool dataCame = false;
                        for (int i = 0; i < 15; i++)
                        {
                            await Task.Delay(1500);
                            // 'rdgSonuclar' içeren tabloyu ve içindeki veri satırlarını kontrol et
                            var rows = driver.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//tr[contains(@class, 'Grid')]"));

                            // Eğer 'Uygun kayıt bulunamadı' mesajı varsa bu takımı geç
                            if (driver.PageSource.Contains("Uygun kayıt bulunamadı"))
                            {
                                Console.WriteLine($"{team.Name}: Kayıt bulunamadı.");
                                break;
                            }

                            if (rows.Count > 0) { dataCame = true; break; }
                        }

                        if (!dataCame) continue;

                        // 5. Veri Kaydetme (RadGrid Uyumlu)
                        var finalRows = driver.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//tr[contains(@class, 'Grid')]"));

                        foreach (var row in finalRows)
                        {
                            var cells = row.FindElements(By.TagName("td"));
                            // TFF tablosu yapısında 0: Lisans No, 1: Ad-Soyad
                            if (cells.Count < 2) continue;

                            string tffId = cells[0].Text.Trim();

                            // Başlık veya boş satır kontrolü
                            if (string.IsNullOrEmpty(tffId) || tffId.Contains("Lisans No") || tffId.Contains("Uygun kayıt")) continue;

                            if (!await _context.Players.AnyAsync(p => p.TffId == tffId))
                            {
                                var fullName = cells[1].Text.Trim();
                                var names = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                                _context.Players.Add(new Player
                                {
                                    Id = Guid.NewGuid(),
                                    FirstName = names[0],
                                    // Soyadı kısmını varsa tüm soyadlarını birleştirerek al
                                    LastName = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "",
                                    TffId = tffId,
                                    TeamId = team.Id,
                                    CreatedAt = DateTime.UtcNow
                                });
                                totalSaved++;
                            }
                        }
                        await _context.SaveChangesAsync();
                        Console.WriteLine($"{team.Name} tamamlandı. Eklenen: {totalSaved}");
                        await Task.Delay(1000);
                    }
                    catch (Exception ex) { Console.WriteLine($"Takım Hatası: {team.Name} -> {ex.Message}"); }
                }
            }
            return totalSaved;
        }




        // $find hatası vermeyen, fiziksel tıklama ve değişim simülasyonu yapan metod
        private async Task FiltrSeelect(IWebDriver driver, WebDriverWait wait, string idPart, string hedefMetin)
        {
            var input = wait.Until(d => d.FindElement(By.CssSelector($"input[id*='{idPart}']")));

            // 1. Kutuyu temizle ve metni yaz
            input.Click();
            await Task.Delay(500);
            input.SendKeys(Keys.Control + "a");
            input.SendKeys(Keys.Backspace);
            input.SendKeys(hedefMetin);
            await Task.Delay(500);
            input.SendKeys(Keys.Enter); // Seçimi onayla

            // 2. TFF'nin gizli JavaScript 'onchange' olayını manuel tetikle (Kritik nokta)
            // Bu sayede Telerik nesnesi yüklü olmasa bile tarayıcı değişikliği fark eder.
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(@"
        var el = arguments[0];
        var ev = document.createEvent('HTMLEvents');
        ev.initEvent('change', true, false);
        el.dispatchEvent(ev);
        el.blur();", input);

            // 3. Sayfanın PostBack yapması için bekle
            await Task.Delay(5000);
        }
    }
    }

