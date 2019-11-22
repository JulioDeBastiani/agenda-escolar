using System;
using Diary.Domain;

namespace Diary.WebApi.ViewModels
{
    public class AssignmentViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DueAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float MaxGrade { get; set; }

        public static implicit operator AssignmentViewModel(Assignment assignment)
        {
            if (assignment == null)
                return null;

            return new AssignmentViewModel
            {
                Id = assignment.Id,
                CreatedAt = assignment.CreatedAt,
                DueAt = assignment.DueAt,
                Title = assignment.Title,
                Description = assignment.Description,
                MaxGrade = assignment.MaxGrade
            };
        }
    }
}