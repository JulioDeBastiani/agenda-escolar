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

namespace Diary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ApiControllerBase
    {
        private GenericRepository<Assignment> _assignmentsRepository;
        private GenericRepository<Class> _classesRepository;

        public AssignmentsController(
            GenericRepository<Assignment> assignmentsRepository,
            GenericRepository<Class> classesRepository)
        {
            _assignmentsRepository = assignmentsRepository;
            _classesRepository = classesRepository;
        }

        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<AssignmentViewModel>>> GetAsync([FromQuery] PageInputModel inputModel = null)
            => ExecuteAsync<IEnumerable<AssignmentViewModel>>(async () =>
            {
                var assignments = await _assignmentsRepository.GetPageDescendingAsync(
                    skip: inputModel?.Skip,
                    take: inputModel?.Take
                );

                if (!assignments.Any())
                    return NoContent();

                return Ok(assignments.Select(a => (AssignmentViewModel) a));
            });

        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<AssignmentViewModel>> GetAsync(Guid id)
            => ExecuteAsync<AssignmentViewModel>(async () =>
            {
                var assignment = await _assignmentsRepository.GetByIdAsync(id);

                if (assignment == null)
                    return NotFound();

                return Ok((AssignmentViewModel) assignment);
            });

        [HttpPost]
        [Authorize]
        public Task<ActionResult<AssignmentViewModel>> PostAsync([FromBody] AssignmentInputModel inputModel)
            => ExecuteAsync<AssignmentViewModel>(async () =>
            {
                if (inputModel == null)
                    return BadRequest();

                var @class = await _classesRepository.GetByIdAsync(inputModel.ClassId);

                if (@class == null)
                    return BadRequest();
                
                var assignment = new Assignment(@class, inputModel.DueAt, inputModel.Title, inputModel.Description, inputModel.MaxGrade);
                await _assignmentsRepository.AddAsync(assignment);
                return Ok((AssignmentViewModel) assignment);
            });

        [HttpPut("{id}")]
        [Authorize]
        public Task<ActionResult<AssignmentViewModel>> PutAsync(Guid id, [FromBody] AssignmentUpdateInputModel inputModel)
            => ExecuteAsync<AssignmentViewModel>(async () =>
            {
                if (inputModel == null)
                    return BadRequest();

                var assignment = await _assignmentsRepository.GetByIdAsync(id);

                if (assignment == null)
                    return NotFound();

                if (inputModel.DueAt != null)
                    assignment.SetDueAt(inputModel.DueAt.Value);
                
                if (inputModel.Title != null)
                    assignment.SetTitle(inputModel.Title);
                
                if (inputModel.Description != null)
                    assignment.SetDescription(inputModel.Description);
                
                if (inputModel.MaxGrade != null)
                    assignment.SetMaxGrade(inputModel.MaxGrade.Value);

                await _assignmentsRepository.UpdateAsync(assignment);
                return Ok((AssignmentViewModel) assignment);
            });

        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var assignment = await _assignmentsRepository.GetByIdAsync(id);

                if (assignment == null)
                    return NotFound();

                await _assignmentsRepository.DeleteAsync(assignment);
                return Ok();
            });
    }
}