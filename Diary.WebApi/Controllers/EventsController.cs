using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arq.Data;
using Diary.Domain;
using Diary.Domain.Enumerators;
using Diary.WebApi.InputModels;
using Diary.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ApiControllerBase
    {
        private GenericRepository<Event> _eventsRepository;
        private GenericRepository<User> _usersRepository;

        public EventsController(
            GenericRepository<Event> eventsRepository,
            GenericRepository<User> usersRepository)
        {
            _eventsRepository = eventsRepository;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<EventViewModel>>> GetAsync([FromQuery] PageInputModel inputModel = null)
            => ExecuteAsync<IEnumerable<EventViewModel>>(async () =>
            {
                var events = await _eventsRepository.GetPageDescendingAsync(
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User),
                    skip: inputModel?.Skip,
                    take: inputModel?.Take
                );

                if (!events.Any())
                    return NoContent();

                return Ok(events.Select(e => (EventViewModel) e));
            });

        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> GetAsync(Guid id)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                return Ok((EventViewModel) @event);
            });

        [HttpGet("created/{userId}")]
        [Authorize]
        public Task<ActionResult<IEnumerable<EventViewModel>>> GetCreatedAsync(Guid userId, [FromQuery] PageInputModel inputModel = null)
            => ExecuteAsync<IEnumerable<EventViewModel>>(async () =>
            {
                var events = await _eventsRepository.GetPageDescendingAsync(
                    predicate: e => e.CreatorId == userId,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User),
                    skip: inputModel?.Skip,
                    take: inputModel?.Take
                );

                if (!events.Any())
                    return NoContent();

                return Ok(events.Select(e => (EventViewModel) e));
            });

        [HttpGet("invited/{userId}")]
        [Authorize]
        public Task<ActionResult<IEnumerable<EventViewModel>>> GetInvitedAsync(Guid userId, [FromQuery] PageInputModel inputModel = null)
            => ExecuteAsync<IEnumerable<EventViewModel>>(async () =>
            {
                var events = await _eventsRepository.GetPageDescendingAsync(
                    predicate: e => e.Attendees.Any(a => a.UserId == userId),
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User),
                    skip: inputModel?.Skip,
                    take: inputModel?.Take
                );

                if (!events.Any())
                    return NoContent();

                return Ok(events.Select(e => (EventViewModel) e));
            });

        [HttpPost]
        [Authorize]
        public Task<ActionResult<EventViewModel>> PostAsync([FromBody] EventInputModel inputModel)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                if (inputModel == null)
                    return BadRequest();

                var creator = await _usersRepository.GetByIdAsync(inputModel.CreatorId);

                var @event = new Event(inputModel.Date, inputModel.Duration, creator);
                await _eventsRepository.AddAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpPut("{id}")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> PutAsync(Guid id, [FromBody] EventUpdateInputModel inputModel)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                if (inputModel == null)
                    return BadRequest();

                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                if (inputModel.Date.HasValue)
                    @event.SetDate(inputModel.Date.Value);

                if (inputModel.Duration.HasValue)
                    @event.SetDuration(inputModel.Duration.Value);

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> DeleteAsync(Guid id)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                @event.SetStatus(EventStatus.Canceled);

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpPost("{id}/complete")]
        [Authorize]
        // TODO ideally should be a hangfire job
        public Task<ActionResult<EventViewModel>> CompleteAsync(Guid id)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                @event.SetStatus(EventStatus.Completed);

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpPost("{id}/invite/{userId}")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> InviteAsync(Guid id, Guid userId)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                var user = await _usersRepository.GetByIdAsync(userId);

                if (user == null)
                    return NotFound();

                if (@event.Attendees.Any(a => a.UserId == userId))
                    return BadRequest();

                @event.Attendees.Add(new UserEvent(user, @event));

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpDelete("{id}/invite/{userId}")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> UninviteAsync(Guid id, Guid userId)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                var userEvent = @event.Attendees.FirstOrDefault(a => a.UserId == userId);

                if (userEvent == null)
                    return NotFound();

                @event.Attendees.Remove(userEvent);

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpPost("{id}/invite/{userId}/confirm")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> ConfirmAsync(Guid id, Guid userId)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                var userEvent = @event.Attendees.FirstOrDefault(a => a.UserId == userId);

                if (userEvent == null)
                    return NotFound();

                userEvent.SetStatus(UserEventStatus.Confirmed);

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });

        [HttpPost("{id}/invite/{userId}/decline")]
        [Authorize]
        public Task<ActionResult<EventViewModel>> DeclineAsync(Guid id, Guid userId)
            => ExecuteAsync<EventViewModel>(async () =>
            {
                var @event = await _eventsRepository.GetByIdAsync(
                    id,
                    includes: e => e
                        .Include(r => r.Attendees)
                            .ThenInclude(a => a.User)
                );

                if (@event == null)
                    return NotFound();

                var userEvent = @event.Attendees.FirstOrDefault(a => a.UserId == userId);

                if (userEvent == null)
                    return NotFound();

                userEvent.SetStatus(UserEventStatus.Declined);

                await _eventsRepository.UpdateAsync(@event);
                return Ok((EventViewModel) @event);
            });
    }
}