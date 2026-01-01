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
            var scrapedTeams = new List<Team>();
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                try
                {
                    driver.Navigate().GoToUrl("https://www.tff.org/default.aspx?pageID=119");
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                    // Filtreleme Fonksiyonu
                    Func<Task> applyFilter = async () => {
                        var city = wait.Until(d => d.FindElement(By.Id("ctl00_MPane_m_119_2780_ctnr_m_119_2780_SehirSelector1_combo_Input")));
                        city.Clear();
                        city.SendKeys("DENİZLİ" + Keys.Enter);
                        await Task.Delay(3000);
                        var status = wait.Until(d => d.FindElement(By.Id("ctl00_MPane_m_119_2780_ctnr_m_119_2780_cmbStatu_Input")));
                        status.Clear();
                        status.SendKeys("Amatör" + Keys.Enter);
                        await Task.Delay(2000);
                        driver.FindElement(By.Id("ctl00_MPane_m_119_2780_ctnr_m_119_2780_btnSave")).Click();
                        await Task.Delay(4000);
                    };

                    await applyFilter();

                    for (int p = 1; p <= 7; p++)
                    {
                        wait.Until(d => d.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]")).Count > 0);
                        var nodes = driver.FindElements(By.XPath("//table[contains(@id, 'rdgSonuclar')]//a[contains(@href, 'kulupID')]"));

                        if (nodes.Any(n => n.Text.Contains("ADANA")))
                        {
                            await applyFilter();
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
                                var imgElement = row.FindElement(By.TagName("img"));
                                logoUrl = imgElement.GetAttribute("src");
                            }
                            catch { logoUrl = null; }

                            if (!scrapedTeams.Any(t => t.TffId == tffId))
                            {
                                scrapedTeams.Add(new Team
                                {
                                    Id = Guid.NewGuid(),
                                    Name = teamName,
                                    TffId = tffId,
                                    LogoUrl = logoUrl,
                                    LeagueCategory = (FootLab.Entities.Entites.LeagueCategoryCode)1,
                                    // PostgreSQL için en güvenli tarih formatı:
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow,
                                    IsDeleted = false
                                });
                            }
                        }

                        if (p < 7)
                        {
                            try
                            {
                                var nextLink = driver.FindElement(By.XPath($"//td[contains(., 'Sayfalar')]//a[text()='{p + 1}']"));
                                js.ExecuteScript("arguments[0].click();", nextLink);
                                await Task.Delay(4000);
                            }
                            catch { break; }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Scraping Hatası: {ex.Message}");
                }
                finally
                {
                    driver.Quit();
                }
            }

            // 2. AŞAM: VERİTABANINA KAYIT
            try
            {
                if (scrapedTeams.Count == 0)
                {
                    Console.WriteLine("Uyarı: Hiç takım bulunamadı!");
                    return scrapedTeams;
                }

                int savedCount = 0;
                foreach (var team in scrapedTeams)
                {
                    var exists = await _context.Teams.AnyAsync(t => t.TffId == team.TffId);
                    if (!exists)
                    {
                        _context.Teams.Add(team);
                        savedCount++;
                    }
                }

                if (savedCount > 0)
                {
                    // Burası asıl kritik nokta
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"{savedCount} yeni takım başarıyla DB'ye eklendi.");
                }
                else
                {
                    Console.WriteLine("Yeni takım bulunamadı, hepsi zaten DB'de mevcut.");
                }
            }
            catch (Exception dbEx)
            {
                // Detaylı hata mesajı için InnerException kontrolü
                var errorMsg = dbEx.InnerException != null ? dbEx.InnerException.Message : dbEx.Message;
                Console.WriteLine($"KRİTİK DB HATASI: {errorMsg}");
            }

            return scrapedTeams;
        }



        //public async Task<int> ScrapePlayersByTeamSearchAsync()
        //{
        //    var teams = await _context.Teams.Where(t => !t.IsDeleted).ToListAsync();
        //    int totalSaved = 0;

        //    var options = new ChromeOptions();
        //    options.AddArgument("--headless=new"); // Görerek test etmek istersen burayı yorum satırı yap

        //    using (IWebDriver driver = new ChromeDriver(options))
        //    {
        //        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        //        foreach (var team in teams)
        //        {
        //            try
        //            {
        //                // 1. Oyuncu Arama Sayfasına Git
        //                driver.Navigate().GoToUrl("https://www.tff.org/Default.aspx?pageID=130");

        //                // 2. "Kulübe Göre" sekmesine tıkla
        //                var clubTab = wait.Until(d => d.FindElement(By.XPath("//a[contains(text(), 'Kulübe Göre')]")));
        //                clubTab.Click();
        //                await Task.Delay(1000);

        //                // 3. Kulüp Adını Yaz (Senin DB'deki tam ad)
        //                var clubInput = driver.FindElement(By.Id("ctl00_MPane_m_130_711_ctnr_m_130_711_txtKulup"));
        //                clubInput.Clear();
        //                clubInput.SendKeys(team.Name);

        //                // 4. Statü "Amatör" seç (Görselde Amatör seçili)
        //                var statusInput = driver.FindElement(By.Id("ctl00_MPane_m_130_711_ctnr_m_130_711_cmbStatu_Input"));
        //                statusInput.Clear();
        //                statusInput.SendKeys("Amatör" + Keys.Enter);
        //                await Task.Delay(1000);

        //                // 5. Ara Butonuna Bas
        //                var searchBtn = driver.FindElement(By.Id("ctl00_MPane_m_130_711_ctnr_m_130_711_btnAra"));
        //                searchBtn.Click();

        //                // 6. Sonuç Tablosunu Bekle
        //                await Task.Delay(3000);
        //                var rows = driver.FindElements(By.XPath("//table[contains(@id, 'dgBilgiBankasi')]//tr[td]"));

        //                foreach (var row in rows)
        //                {
        //                    var cells = row.FindElements(By.TagName("td"));
        //                    if (cells.Count < 4) continue;

        //                    string licenseNo = cells[0].Text.Trim(); // Lisans No
        //                    string fullName = cells[1].Text.Trim();  // Ad-Soyad
        //                    string birthDateStr = cells[3].Text.Trim(); // Doğum Tarihi

        //                    // İsim parçalama
        //                    var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        //                    string firstName = nameParts[0];
        //                    string midName = nameParts.Length > 2 ? string.Join(" ", nameParts.Skip(1).Take(nameParts.Length - 2)) : "";
        //                    string lastName = nameParts.Length > 1 ? nameParts.Last() : "";

        //                    // Doğum tarihi parse
        //                    DateTime? birthDay = null;
        //                    if (DateTime.TryParse(birthDateStr, out DateTime dt))
        //                        birthDay = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        //                    // Lisans No (TffId) üzerinden kontrol et (Daha güvenli)
        //                    var exists = await _context.Players.AnyAsync(p => p.TffId == licenseNo);

        //                    if (!exists)
        //                    {
        //                        _context.Players.Add(new Player
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            FirstName = firstName,
        //                            MidName = midName,
        //                            LastName = lastName,
        //                            BirthDay = birthDay ?? DateTime.MinValue,
        //                            TffId = licenseNo, // Lisans numarasını TffId'ye koyuyoruz
        //                            TeamId = team.Id,
        //                            isFreeAgent = false,
        //                            CreatedAt = DateTime.UtcNow,
        //                            UpdatedAt = DateTime.UtcNow,
        //                            IsDeleted = false
        //                        });
        //                        totalSaved++;
        //                    }
        //                }
        //                await _context.SaveChangesAsync();
        //                Console.WriteLine($"{team.Name}: {rows.Count} oyuncu işlendi.");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"{team.Name} aranırken hata: {ex.Message}");
        //            }
        //        }
        //    }
        //    return totalSaved;
        //}
        //public async Task<int> ScrapePlayersByTeamSearchAsync()
        //{
        //    var teams = await _context.Teams.Where(t => !t.IsDeleted).ToListAsync();
        //    int totalSaved = 0;

        //    var options = new ChromeOptions();
        //    options.AddArgument("--window-size=1920,1080");
        //    options.AddArgument("--disable-blink-features=AutomationControlled");

        //    using (IWebDriver driver = new ChromeDriver(options))
        //    {
        //        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        //        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

        //        driver.Navigate().GoToUrl("https://www.tff.org/Default.aspx?pageID=130");
        //        await Task.Delay(5000);

        //        // 1. Sekme Seçimi
        //        var clubTab = wait.Until(d => d.FindElement(By.XPath("//span[contains(text(),'Kulübe Göre')]")));
        //        clubTab.Click();
        //        await Task.Delay(5000);

        //        // 2. Filtre Seçimleri
        //        await GarantiliFiltreSec(driver, wait, "_ks_Input", "Amatör");
        //        await GarantiliFiltreSec(driver, wait, "_f2_Input", "Faal");

        //        // 3. Takım Döngüsü
        //        foreach (var team in teams)
        //        {
        //            try
        //            {
        //                // KULÜP ADI: Dinamik bulma
        //                var clubInput = wait.Until(d => {
        //                    var el = d.FindElement(By.CssSelector("input[id*='txtKulupAdi']"));
        //                    return el.Displayed ? el : null;
        //                });
        //                clubInput.Clear();
        //                clubInput.SendKeys(team.Name);
        //                await Task.Delay(1000);

        //                // ARA BUTONU: Yeni ID yapısına göre düzenlendi (btnSearch2)
        //                IWebElement searchBtn = null;
        //                for (int i = 0; i < 5; i++)
        //                {
        //                    try
        //                    {
        //                        // Sitedeki 'Ara' yazan ve ID'si değişen butonu yakalar
        //                        searchBtn = driver.FindElement(By.CssSelector("input[id*='btnSearch2'], input[id*='btnAra']"));
        //                        if (searchBtn.Displayed) break;
        //                    }
        //                    catch { await Task.Delay(1000); }
        //                }

        //                if (searchBtn != null)
        //                {
        //                    // Butonu görünür alana kaydır ve odaklan
        //                    js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", searchBtn);
        //                    await Task.Delay(500);

        //                    // Önce klasik click, yemezse JS click
        //                    try { searchBtn.Click(); }
        //                    catch { js.ExecuteScript("arguments[0].click();", searchBtn); }
        //                }
        //                else
        //                {
        //                    Console.WriteLine($"{team.Name}: Ara butonu bulunamadı.");
        //                    continue;
        //                }

        //                Console.WriteLine($"{team.Name} sorgulanıyor...");

        //                // 4. Veri Bekleme Senkronizasyonu
        //                bool dataCame = false;
        //                for (int i = 0; i < 15; i++)
        //                {
        //                    await Task.Delay(1500);
        //                    // Tablonun güncellendiğini anlamak için Grid satırlarını say
        //                    var rows = driver.FindElements(By.XPath("//table[contains(@id, 'dgBilgiBankasi')]//tr[contains(@class, 'Grid')]"));
        //                    if (rows.Count > 0) { dataCame = true; break; }
        //                    if (driver.PageSource.Contains("Kayıt bulunamadı")) break;
        //                }

        //                if (!dataCame) continue;

        //                // 5. Veri Kaydetme
        //                var finalRows = driver.FindElements(By.XPath("//table[contains(@id, 'dgBilgiBankasi')]//tr[contains(@class, 'Grid')]"));
        //                foreach (var row in finalRows)
        //                {
        //                    var cells = row.FindElements(By.TagName("td"));
        //                    if (cells.Count < 2) continue;

        //                    string tffId = cells[0].Text.Trim();
        //                    if (!await _context.Players.AnyAsync(p => p.TffId == tffId))
        //                    {
        //                        var names = cells[1].Text.Trim().Split(' ');
        //                        _context.Players.Add(new Player
        //                        {
        //                            Id = Guid.NewGuid(),
        //                            FirstName = names[0],
        //                            LastName = names.Length > 1 ? names.Last() : "",
        //                            TffId = tffId,
        //                            TeamId = team.Id,
        //                            CreatedAt = DateTime.UtcNow
        //                        });
        //                        totalSaved++;
        //                    }
        //                }
        //                await _context.SaveChangesAsync();
        //                Console.WriteLine($"{team.Name} bitti. Toplam Yeni: {totalSaved}");

        //                // Diğer takıma geçmeden önce kısa bir nefes (TFF koruması için)
        //                await Task.Delay(1000);
        //            }
        //            catch (Exception ex) { Console.WriteLine($"Takım Hatası: {team.Name} -> {ex.Message}"); }
        //        }
        //    }
        //    return totalSaved;
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
                await GarantiliFiltreSec(driver, wait, "_ks_Input", "Amatör");
                await GarantiliFiltreSec(driver, wait, "_f2_Input", "Faal");

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
        private async Task GarantiliFiltreSec(IWebDriver driver, WebDriverWait wait, string idPart, string hedefMetin)
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

