using FootLab.Bussines.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootLab.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperController : ControllerBase
    {
        private readonly TffScraper _tffScraper;

        public ScraperController(TffScraper tffScraper)
        {
            _tffScraper = tffScraper;
        }

        [HttpPost("sync-teams")]
        public async Task<IActionResult> SyncTeams()
        {
            try
            {
               // var teamList = await _tffScraper.ScrapeDenizliAmateurTeamsAsync(Guid targetGroupId,  "2025-2026");

                return Ok(new
                {
                    //Success = true,
                    //TotalCount = teamList.Count,
                    //Teams = teamList
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }
        [HttpPost("sync-players")]
        public async Task<IActionResult> SyncPlayers()
        {
            try
            {
                var count = await _tffScraper.ScrapePlayersByTeamSearchAsync();
                return Ok(new { Message = "Oyuncu senkronizasyonu tamamlandı.", AddedCount = count });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
