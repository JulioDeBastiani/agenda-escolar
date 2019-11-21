using System;
using System.Collections.Generic;
using System.Linq;
using Diary.Domain;

namespace Diary.WebApi.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }
        public UserViewModel Creator { get; set; }
        public IEnumerable<AttendeeViewModel> Attendees { get; set; }

        public static implicit operator EventViewModel(Event @event)
        {
            if (@event == null)
                return null;

            return new EventViewModel
            {
                Id = @event.Id,
                CreatedAt = @event.CreatedAt,
                Date = @event.Date,
                Duration = @event.Duration,
                Status = @event.Status.ToString(),
                Creator = @event.Creator,
                Attendees = @event.Attendees.Select(a => (AttendeeViewModel) a)
            };
        }

        public class AttendeeViewModel
        {
            public Guid Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }

            public static implicit operator AttendeeViewModel(UserEvent userEvent)
            {
                if (userEvent == null)
                    return null;

                if (userEvent.User == null)
                    return null;

                return new AttendeeViewModel
                {
                    Id = userEvent.UserId,
                    CreatedAt = userEvent.CreatedAt,
                    Name = userEvent.User?.Name,
                    Username = userEvent.User?.Username,
                    Type = userEvent.User?.Type.ToString(),
                    Status = userEvent.Status.ToString()
                };
            }
        }
    }
}