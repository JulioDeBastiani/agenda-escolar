using System;
using Diary.Domain;

namespace Diary.WebApi.InputModels
{
    public class EventInputModel
    {
        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }
        public string Description { get; set; }
        public Guid CreatorId { get; set; }
    }
}