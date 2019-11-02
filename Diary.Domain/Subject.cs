using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Domain.Base;

namespace Diary.Domain
{
    public class Subject : Entity
    {
        public string Description { get; private set; }
        public ICollection<Class> Classes { get; private set; }

        public Subject()
        {
        }

        public Subject(string description)
        {
            SetDescription(description);

            Classes = new Collection<Class>();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            Description = description;
        }
    }
}