using System;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class Appointment : Entity
    {
        public DateTime Date { get; private set; }
        public TimeSpan Duration { get; private set; }
        public AppointmentStatus Status { get; private set; }
        public Guid CreatorId { get; private set; }
        public User Creator { get; private set; }
        // TODO um nome mais decente
        public Guid TargetId { get; private set; }
        public User Target { get; private set; }

        public Appointment()
        {
        }

        public Appointment(DateTime date, TimeSpan duration, User creator, User target)
        {
            Status = AppointmentStatus.Scheduled;
            
            SetDate(date);
            SetDuration(duration);

            CreatorId = creator?.Id ?? throw new ArgumentNullException(nameof(creator));
            Creator = creator;

            TargetId = target?.Id ?? throw new ArgumentNullException(nameof(target));
            Target = target;
        }
            
        public void SetDate(DateTime date)
        {
            if (Status != AppointmentStatus.Scheduled)
                throw new InvalidOperationException("Appointment is already finished");
            
            if (date <= DateTime.UtcNow)
                throw new ArgumentOutOfRangeException(nameof(date));

            Date = date;
        }

        public void SetDuration(TimeSpan duration)
        {
            if (Status != AppointmentStatus.Scheduled)
                throw new InvalidOperationException("Appointment is already finished");
            
            if (duration <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            Duration = duration;
        }

        public void SetStatus(AppointmentStatus status)
        {
            if (Status != AppointmentStatus.Scheduled)
                throw new InvalidOperationException("Appointment is already finished");

            if (status == AppointmentStatus.Scheduled)
                throw new InvalidOperationException("New appointment status must be terminal");

            Status = status;
        }
    }
}