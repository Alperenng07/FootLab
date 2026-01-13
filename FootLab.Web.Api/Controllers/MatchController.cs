using AutoMapper;
using FootLab.Bussines.Services.LetsLearnEnglish.Bussines.Services.BaseService;
using FootLab.Entities.DTOs;
using FootLab.Entities.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FootLab.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController(IBaseService<Entities.Entites.Match> services, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var match = await services.GetByIdAsync(id);
            if (match == null) return NotFound();

            var result = mapper.Map<MatchResultDto>(match);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var matches = await services.GetAllAsync();
            var result = mapper.Map<IEnumerable<MatchResultDto>>(matches);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MatchForPostDto matchForPostDto)
        {
            var match = mapper.Map<Entities.Entites.Match>(matchForPostDto);
            await services.AddAsync(match);

            // Ekleme sonrası frontend'in ihtiyacı olan tüm isimleri dönmek için mapliyoruz
            var result = mapper.Map<MatchResultDto>(match);
            return CreatedAtAction(nameof(GetById), new { id = match.Id }, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var exists = await services.GetByIdAsync(id);
            if (exists == null) return NotFound();

            var result = await services.DeleteAsync(id);
            return result ? NoContent() : BadRequest("Silme işlemi başarısız.");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, MatchForUpdateDto matchForUpdateDto)
        {
            var existingMatch = await services.GetByIdAsync(id);
            if (existingMatch == null) return NotFound();

            mapper.Map(matchForUpdateDto, existingMatch);

            await services.UpdateAsync(existingMatch);

            var result = mapper.Map<MatchResultDto>(existingMatch);
            return Ok(result);
        }
    }
}
