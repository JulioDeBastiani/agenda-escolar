using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class StudentClass : Entity
    {
        public Guid StudentId { get; private set; }
        public User Student { get; private set; }
        public Guid ClassId { get; private set; }
        public Class Class { get; private set; }
        public StudentClassStatus Status { get; private set; }
        public float FinalGrade { get; private set; }
        public ICollection<Attendance> Attendance { get; private set; }
        public ICollection<Assignment> Assignments { get; private set; }

        public StudentClass()
        {
        }

        public StudentClass(User student, Class @class)
        {
            StudentId = student?.Id ?? throw new ArgumentNullException(nameof(student));
            Student = student;

            if (student.Type != UserType.Student)
                throw new ArgumentException("User is not a student");
            
            ClassId = @class?.Id ?? throw new ArgumentNullException(nameof(@class));
            Class = @class;
            Status = StudentClassStatus.Registered;
            FinalGrade = 0;

            Attendance = new Collection<Attendance>();
            Assignments = new Collection<Assignment>();
        }

        // TODO metodo para 'finalizar' a cadeira, com nota minima de aprovacao e alteracao do status
    }
}