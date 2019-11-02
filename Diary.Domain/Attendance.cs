using System;
using Diary.Domain.Base;

namespace Diary.Domain
{
    public class Attendance : Entity
    {
        public Guid StudentClassId { get; private set; }
        public StudentClass StudentClass { get; private set; }
        public DateTime Date { get; private set; }
        public int CreditsAttended { get; private set; }

        public Attendance()
        {
        }

        public Attendance(StudentClass studentClass, DateTime date, int creditsAttended)
        {
            StudentClassId = studentClass?.Id ?? throw new ArgumentNullException(nameof(studentClass));
            StudentClass = studentClass;
            Date = date;
            SetCreditsAttended(creditsAttended);
        }

        public void SetCreditsAttended(int creditsAttended)
        {
            if (creditsAttended > StudentClass.Class.MaxCredits || creditsAttended < 0)
                throw new ArgumentOutOfRangeException(nameof(creditsAttended));

            CreditsAttended = creditsAttended;
        }
    }
}