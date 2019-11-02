using System;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class UserEvent : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid EventId { get; private set; }
        public Event Event { get; private set; }
        public UserEventStatus Status { get; private set; }
        
        private UserEvent()
        {
        }

        public UserEvent(User user, Event @event)
        {
            UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            User = user;
            EventId = @event?.Id ?? throw new ArgumentNullException(nameof(@event));
            Event = @event;
            Status = UserEventStatus.Invited;
        }

        public void SetStatus(UserEventStatus status)
        {
            Status = status;
        }
    }
}