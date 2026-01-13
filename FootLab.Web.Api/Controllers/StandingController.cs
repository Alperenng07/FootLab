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
    public class StandingController(IBaseService<Standing> services, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(mapper.Map<IEnumerable<StandingResultDto>>(await services.GetAllAsync()));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id) { var item = await services.GetByIdAsync(id); return item != null ? Ok(mapper.Map<StandingResultDto>(item)) : NotFound(); }
      
        [HttpPost] 
        public async Task<IActionResult> Add(StandingForPostDto dto) {
            var entity = mapper.Map<Standing>(dto);
            await services.AddAsync(entity); 
            return CreatedAtAction(nameof(GetById),
                new { id = entity.Id }, 
                mapper.Map<StandingResultDto>(entity)); 
        }


        [HttpPut("{id:guid}")] 
        public async Task<IActionResult> Update(Guid id, StandingForPostDto dto) {
            var existing = await services.GetByIdAsync(id); 
            if (existing == null) return NotFound(); mapper.Map(dto, existing); 
            await services.UpdateAsync(existing);
            return Ok(mapper.Map<StandingResultDto>(existing));
        }

        [HttpDelete("{id:guid}")] 
        public async Task<IActionResult> Delete(Guid id) => await services.DeleteAsync(id) ? NoContent() : BadRequest();
    }
}
