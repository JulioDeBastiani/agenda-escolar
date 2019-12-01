using System;
using System.Collections;
using System.Collections.Generic;
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
        public string Email { get; set; }
        public int Absences { get; set; }
        public IEnumerable<AttendanceViewModel> Attendance { get; set; }
        public float AttendanceM { get; set; }

        public class AttendanceViewModel
        {
            public DateTime Date { get; set; }
            public bool Absent { get; set; }

            public static implicit operator AttendanceViewModel(Attendance attendance)
            {
                if (attendance == null)
                    return null;

                return new AttendanceViewModel
                {
                    Date = attendance.Date.Date,
                    Absent = attendance.Absent
                };
            }
        }

        public static implicit operator StudentClassViewModel(StudentClass studentClass)
        {
            if (studentClass?.Student == null)
                return null;

            if (studentClass?.Attendance == null)
                return null;

            var attendance = studentClass.Attendance.Select(a => (AttendanceViewModel) a);
            var absences = attendance.Count(a => a.Absent);
            var totalClasses = attendance.Count();

            return new StudentClassViewModel
            {
                Id = studentClass.Student.Id,
                CreatedAt = studentClass.Student.CreatedAt,
                Name = studentClass.Student.Name,
                Username = studentClass.Student.Username,
                Email = studentClass.Student.Email,
                Absences = absences,
                Attendance = attendance,
                AttendanceM = (totalClasses - absences)
            };
        }
    }
}