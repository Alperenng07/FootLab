using AutoMapper;
using FootLab.Bussines.Services.LetsLearnEnglish.Bussines.Services.BaseService;
using FootLab.Entities.DTOs;
using FootLab.Entities.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootLab.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
        public class PlayerController(IBaseService<Player> services, IMapper mapper) : ControllerBase
        {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var player = await services.GetByIdAsync(id);
            if (player == null) return NotFound();

            // Entity -> ResultDto
            var result = mapper.Map<PlayerResultDto>(player);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var players = await services.GetAllAsync();
            var result = mapper.Map<IEnumerable<PlayerResultDto>>(players);
            return Ok(result);
        }

        [HttpPost]
            public async Task<IActionResult> Add(PlayerForPostDto teamForPostDto)
            {
                var player = mapper.Map<Player>(teamForPostDto);
                await services.AddAsync(player);
                // .NET standardında eklenen verinin linkini dönmek (CreatedAtAction) daha doğrudur
                return CreatedAtAction(nameof(GetById), new { id = player.Id }, player);
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
            public async Task<IActionResult> Update(Guid id, PlayerForUpdateDto playerForUpdateDto)
            {
                var existingPlayer = await services.GetByIdAsync(id);
                if (existingPlayer == null) return NotFound();

            // existingPlayer üzerine DTO'daki verileri yazar (Existing Instance Mapping)
            mapper.Map(playerForUpdateDto, existingPlayer);

                await services.UpdateAsync(existingPlayer);
                return Ok(existingPlayer);
            }
        }
    }

