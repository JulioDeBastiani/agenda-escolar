using System;
using System.Linq;
using Diary.Domain;

namespace Diary.WebApi.ViewModels
{
    public class StudentClassViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public int Absences { get; set; }

        public static implicit operator StudentClassViewModel(StudentClass studentClass)
        {
            if (studentClass?.Student == null)
                return null;

            if (studentClass?.Attendance == null)
                return null;

            return new StudentClassViewModel
            {
                Id = studentClass.Student.Id,
                CreatedAt = studentClass.Student.CreatedAt,
                Name = studentClass.Student.Name,
                Username = studentClass.Student.Username,
                Absences = studentClass.Attendance.Count(a => a.Absent)
            };
        }
    }
}