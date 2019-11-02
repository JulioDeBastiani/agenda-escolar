using System;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class Grade : Entity
    {
        public Guid AssignmentId { get; private set; }
        public Assignment Assignment { get; private set; }
        public Guid StudentId { get; private set; }
        public User Student { get; private set; }
        public float FirstGrade { get; private set; }
        // TODO pensar num nome melhor
        public float? Recovery { get; private set; }

        public Grade()
        {
        }

        public Grade(Assignment assignment, User student, float firstGrade)
        {
            AssignmentId = assignment?.Id ?? throw new ArgumentNullException(nameof(assignment));
            Assignment = assignment;

            StudentId = student?.Id ?? throw new ArgumentNullException(nameof(student));
            Student = student;

            if (student.Type != UserType.Student)
                throw new ArgumentException("User is not a student");

            SetFirstGrade(firstGrade);
        }

        public void SetFirstGrade(float firstGrade)
        {
            if (firstGrade < 0 || firstGrade > Assignment.MaxGrade)
                throw new ArgumentOutOfRangeException(nameof(firstGrade));

            FirstGrade = firstGrade;
        }

        public void SetRecovery(float recovery)
        {
            if (recovery < FirstGrade || recovery > Assignment.MaxGrade)
                throw new ArgumentOutOfRangeException(nameof(recovery));

            Recovery = recovery;
        }
    }
}