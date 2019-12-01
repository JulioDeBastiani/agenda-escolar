using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class Event : Entity
    {
        public DateTime Date { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Description { get; private set; }
        public EventStatus Status { get; private set; }
        public Guid CreatorId { get; private set; }
        public User Creator { get; private set; }
        public string NotificationEventId { get; private set; }
        public ICollection<UserEvent> Attendees { get; private set; }

        public Event()
        {
        }

        public Event(DateTime date, TimeSpan duration, string description, User creator)
        {
            SetDate(date);
            SetDuration(duration);

            CreatorId = creator?.Id ?? throw new ArgumentNullException(nameof(creator));
            Creator = creator;

            Attendees = new Collection<UserEvent>();
        }
            
        public void SetDate(DateTime date)
        {
            if (Status != EventStatus.Scheduled)
                throw new InvalidOperationException("Event is already finished");
            
            if (date <= DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(date));

            Date = date;
        }

        public void SetDuration(TimeSpan duration)
        {
            if (Status != EventStatus.Scheduled)
                throw new InvalidOperationException("Event is already finished");
            
            if (duration <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            Duration = duration;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            Description = description;
        }

        public void SetStatus(EventStatus status)
        {
            if (Status != EventStatus.Scheduled)
                throw new InvalidOperationException("Event is already finished");

            if (status == EventStatus.Scheduled)
                throw new InvalidOperationException("New event status must be terminal");

            Status = status;
        }

        public void SetNotificationEventId(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
                throw new ArgumentNullException(nameof(jobId));

            NotificationEventId = jobId;
        }
    }
}