using System;
using Diary.Domain;

namespace Diary.WebApi.InputModels
{
    public class EventUpdateInputModel
    {
        public DateTime? Date { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}