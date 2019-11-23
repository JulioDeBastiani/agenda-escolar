using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diary.Data;
using Diary.Domain;
using Diary.WebApi.InputModels;
using Diary.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassesController : ApiControllerBase
    {
        public GenericRepository<Class> _classesRepository;
        
        public ClassesController(GenericRepository<Class> classesRepository)
        {
            _classesRepository = classesRepository;
        }

        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<ClassViewModel>>> GetAsync([FromQuery] PageInputModel inputModel = null)
            => ExecuteAsync<IEnumerable<ClassViewModel>>(async () =>
            {
                var classes = await _classesRepository.GetPageDescendingAsync(
                    includes: c => c
                        .Include(r => r.Assignments)
                        .Include(r => r.SchoolYear)
                        .Include(r => r.Students)
                            .ThenInclude(s => s.Attendance)
                        .Include(r => r.Subject)
                        .Include(r => r.Teacher),
                    skip: inputModel?.Skip,
                    take: inputModel?.Take
                );

                if (!classes.Any())
                    return NoContent();

                return Ok(classes.Select(c => (ClassViewModel) c));
            });

        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<IEnumerable<ClassViewModel>>> GetAsync(Guid id)
            => ExecuteAsync<IEnumerable<ClassViewModel>>(async () =>
            {
                var @class = await _classesRepository.GetByIdAsync(
                    id,
                    includes: c => c
                        .Include(r => r.Assignments)
                        .Include(r => r.SchoolYear)
                        .Include(r => r.Students)
                            .ThenInclude(s => s.Attendance)
                        .Include(r => r.Subject)
                        .Include(r => r.Teacher)
                );

                if (@class == null)
                    return NotFound();

                return Ok((ClassViewModel) @class);
            });

        [HttpGet("teacher/{teacherId}")]
        [Authorize]
        public Task<ActionResult<IEnumerable<ClassViewModel>>> GetAsync(Guid teacherId, [FromQuery] PageInputModel inputModel = null)
            => ExecuteAsync<IEnumerable<ClassViewModel>>(async () =>
            {
                var classes = await _classesRepository.GetPageDescendingAsync(
                    predicate: c => c.TeacherId == teacherId,
                    includes: c => c
                        .Include(r => r.Assignments)
                        .Include(r => r.SchoolYear)
                        .Include(r => r.Students)
                            .ThenInclude(s => s.Attendance)
                        .Include(r => r.Subject)
                        .Include(r => r.Teacher),
                    skip: inputModel?.Skip,
                    take: inputModel?.Take
                );

                if (!classes.Any())
                    return NoContent();

                return Ok(classes.Select(c => (ClassViewModel) c));
            });
    }
}