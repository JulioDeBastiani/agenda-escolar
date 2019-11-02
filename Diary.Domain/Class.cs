using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class Class : Entity
    {
        public Guid SubjectId { get; private set; }
        public Subject Subject { get; private set; }
        public Guid SchoolYearId { get; private set; }
        public SchoolYear SchoolYear { get; private set; }
        public int MaxCredits { get; private set; }
        public ICollection<StudentClass> Students { get; private set; }
        public Guid TeacherId { get; private set; }
        public User Teacher { get; private set; }

        public Class()
        {
        }

        public Class(Subject subject, SchoolYear schoolYear, int maxCredits, User teacher)
        {
            SubjectId = subject?.Id ?? throw new ArgumentNullException(nameof(subject));
            Subject = subject;
            SchoolYearId = schoolYear?.Id ?? throw new ArgumentNullException(nameof(schoolYear));
            SchoolYear = schoolYear;

            if (maxCredits < 0)
                throw new ArgumentOutOfRangeException(nameof(maxCredits));

            MaxCredits = maxCredits;

            TeacherId = teacher?.Id ?? throw new ArgumentNullException(nameof(teacher));
            Teacher = teacher;

            if (teacher.Type != UserType.Teacher)
                throw new ArgumentException("User is not a teacher");
            
            Students = new Collection<StudentClass>();
        }
    }
}