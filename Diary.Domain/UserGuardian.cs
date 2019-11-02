using System;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class UserGuardian : Entity
    {
        public Guid GuardianId { get; private set; }
        public User Guardian { get; private set; }
        public Guid StudentId { get; private set; }
        public User Student { get; private set; }

        public UserGuardian()
        {
        }

        public UserGuardian(User guardian, User student)
        {
            GuardianId = guardian?.Id ?? throw new ArgumentNullException(nameof(guardian));
            Guardian = guardian;

            if (guardian.Type != UserType.Guardian)
                throw new ArgumentException("User must be of type guardian");

            StudentId = student?.Id ?? throw new ArgumentNullException(nameof(student));
            Student = student;

            if (student.Type != UserType.Student)
                throw new ArgumentException("User must be of type student");
        }
    }
}