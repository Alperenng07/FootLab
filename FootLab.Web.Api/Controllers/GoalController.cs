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
    public class GoalController(IBaseService<Goal> services, IMapper mapper) : ControllerBase
    {
        [HttpGet] 
        public async Task<IActionResult> GetAll() => Ok(mapper.Map<IEnumerable<GoalResultDto>>(await services.GetAllAsync()));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id) { 
            var item = await services.GetByIdAsync(id); 
            return item != null ? Ok(mapper.Map<GoalResultDto>(item)) : NotFound();
        }

        [HttpPost] 
        public async Task<IActionResult> Add(GoalForPostDto dto) { 
            var entity = mapper.Map<Goal>(dto); await services.AddAsync(entity);
            return CreatedAtAction(nameof(GetById),
                new { id = entity.Id }, mapper.Map<GoalResultDto>(entity));
        }

        [HttpDelete("{id:guid}")] 
        public async Task<IActionResult> Delete(Guid id) => await services.DeleteAsync(id) ? NoContent() : BadRequest();
    }
}
