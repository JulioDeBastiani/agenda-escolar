using System;

namespace Diary.WebApi.InputModels
{
    public class AssignmentInputModel
    {
        public Guid ClassId { get; set; }
        public DateTime DueAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float MaxGrade { get; set; }
    }
}