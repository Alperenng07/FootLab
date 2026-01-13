using AutoMapper;
using FootLab.Bussines.Services;
using FootLab.Bussines.Services.LetsLearnEnglish.Bussines.Services.BaseService;
using FootLab.Entities.DTOs;
using FootLab.Entities.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FootLab.Web.Api.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        // .NET 8 Primary Constructor: Field tanımlamaya ve atamaya gerek kalmaz.
        public class TeamController(IBaseService<Team> services, IMapper mapper) : ControllerBase
        {
            
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var team = await services.GetByIdAsync(id);
            if (team == null) return NotFound();

            var result = mapper.Map<TeamResultDto>(team);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var teams = await services.GetAllAsync();
            var result = mapper.Map<IEnumerable<TeamResultDto>>(teams);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(TeamForPostDto teamForPostDto)
        {
            var team = mapper.Map<Team>(teamForPostDto);
            await services.AddAsync(team);

            var result = mapper.Map<TeamResultDto>(team);
            return CreatedAtAction(nameof(GetById), new { id = team.Id }, result);
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
        public async Task<IActionResult> Update(Guid id, TeamForUpdateDto teamForUpdateDto)
        {
            var existingTeam = await services.GetByIdAsync(id);
            if (existingTeam == null) return NotFound();

            // Nesneyi güncelle (Tracking sayesinde sadece değişen yerler SQL'e gider)
            mapper.Map(teamForUpdateDto, existingTeam);

            await services.UpdateAsync(existingTeam);

            var result = mapper.Map<TeamResultDto>(existingTeam);
            return Ok(result);
        }
    }
    }
