using System;
using Diary.Domain;

namespace Diary.WebApi.ViewModels
{
    public class SubjectViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }

        public static implicit operator SubjectViewModel(Subject subject)
        {
            if (subject == null)
                return null;

            return new SubjectViewModel
            {
                Id = subject.Id,
                CreatedAt = subject.CreatedAt,
                Description = subject.Description
            };
        }
    }
}