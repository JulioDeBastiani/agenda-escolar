using System;
using System.Collections.Generic;
using System.Linq;
using Diary.Domain;

namespace Diary.WebApi.ViewModels
{
    public class ClassViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public SubjectViewModel Subject { get; set; }
        public int SchoolYear { get; set; }
        public int MaxCredits { get; set; }
        public UserViewModel Teacher { get; set; }
        public IEnumerable<StudentClassViewModel> Students { get; set; }
        public IEnumerable<AssignmentViewModel> Assignments { get; set; }

        public static implicit operator ClassViewModel(Class @class)
        {
            if (@class == null)
                return null;

            if (@class.SchoolYear == null)
                return null;

            if (@class.Subject == null)
                return null;

            if (@class.Teacher == null)
                return null;

            return new ClassViewModel
            {
                Id = @class.Id,
                CreatedAt = @class.CreatedAt,
                Subject = @class.Subject,
                SchoolYear = @class.SchoolYear.Year,
                MaxCredits = @class.MaxCredits,
                Teacher = @class.Teacher,
                Students = @class.Students.Select(s => (StudentClassViewModel) s),
                Assignments = @class.Assignments.Select(a => (AssignmentViewModel) a)
            };
        }
    }
}