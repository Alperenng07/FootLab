using AutoMapper;
using FootLab.Bussines.Services.LetsLearnEnglish.Bussines.Services.BaseService;
using FootLab.Entities.DTOs;
using FootLab.Entities.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootLab.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamSeasonDetailController(IBaseService<TeamSeasonDetail> services, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(mapper.Map<IEnumerable<TeamSeasonDetailResultDto>>(await services.GetAllAsync()));

        [HttpGet("{id:guid}")] 
        public async Task<IActionResult> GetById(Guid id) {
            var item = await services.GetByIdAsync(id);
            return item != null ? Ok(mapper.Map<TeamSeasonDetailResultDto>(item)) : NotFound();
        }

        [HttpPost] 
        public async Task<IActionResult> Add(TeamSeasonDetailForPostDto dto) { 
            var entity = mapper.Map<TeamSeasonDetail>(dto);
            await services.AddAsync(entity); 
            return CreatedAtAction(nameof(GetById), 
                new { id = entity.Id }, 
                mapper.Map<TeamSeasonDetailResultDto>(entity)); 
        }

        [HttpDelete("{id:guid}")] 
        public async Task<IActionResult> Delete(Guid id) => await services.DeleteAsync(id) ? NoContent() : BadRequest();
    }
}
