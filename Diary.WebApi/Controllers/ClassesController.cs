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
        private GenericRepository<Class> _classesRepository;
        private GenericRepository<Attendance> _attendanceRepository;
        
        public ClassesController(
            GenericRepository<Class> classesRepository,
            GenericRepository<Attendance> attendanceRepository)
        {
            _classesRepository = classesRepository;
            _attendanceRepository = attendanceRepository;
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

        [HttpPost("{id}/present/{studentId}/{date}")]
        [Authorize]
        public Task<ActionResult> PresentAsync(Guid id, Guid studentId, DateTime date)
            => ExecuteAsync(async () =>
            {
                var @class = await _classesRepository.GetByIdAsync(
                    id,
                    includes: c => c
                        .Include(r => r.Students)
                            .ThenInclude(s => s.Attendance));

                if (@class == null)
                    return NotFound();

                var student = @class.Students.SingleOrDefault(s => s.StudentId == studentId);

                if (student == null)
                    return NotFound();

                var attendance = student.Attendance.SingleOrDefault(a => a.Date == date.Date);

                if (attendance == null)
                {
                    attendance = new Attendance(student, date.Date, false);
                    await _attendanceRepository.AddAsync(attendance);
                }
                else
                {
                    attendance.SetAbsent(false);
                    await _attendanceRepository.UpdateAsync(attendance);
                }

                return Ok();
            });

        [HttpPost("{id}/present/{studentId}/{date}")]
        [Authorize]
        public Task<ActionResult> AbsentAsync(Guid id, Guid studentId, DateTime date)
            => ExecuteAsync(async () =>
            {
                var @class = await _classesRepository.GetByIdAsync(
                    id,
                    includes: c => c
                        .Include(r => r.Students)
                            .ThenInclude(s => s.Attendance));

                if (@class == null)
                    return NotFound();

                var student = @class.Students.SingleOrDefault(s => s.StudentId == studentId);

                if (student == null)
                    return NotFound();

                var attendance = student.Attendance.SingleOrDefault(a => a.Date == date.Date);

                if (attendance == null)
                {
                    attendance = new Attendance(student, date.Date, true);
                    await _attendanceRepository.AddAsync(attendance);
                }
                else
                {
                    attendance.SetAbsent(true);
                    await _attendanceRepository.UpdateAsync(attendance);
                }

                return Ok();
            });
    }
}