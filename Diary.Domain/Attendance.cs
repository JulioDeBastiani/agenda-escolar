using System;
using Diary.Domain.Base;

namespace Diary.Domain
{
    public class Attendance : Entity
    {
        public Guid StudentClassId { get; private set; }
        public StudentClass StudentClass { get; private set; }
        public DateTime Date { get; private set; }
        public bool Absent { get; private set; }

        public Attendance()
        {
        }

        public Attendance(StudentClass studentClass, DateTime date, bool absent)
        {
            StudentClassId = studentClass?.Id ?? throw new ArgumentNullException(nameof(studentClass));
            StudentClass = studentClass;
            Date = date;
            SetAbsent(absent);
        }

        public void SetAbsent(bool absent)
        {
            Absent = absent;
        }
    }
}