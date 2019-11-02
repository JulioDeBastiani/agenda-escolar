using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Domain.Base;

namespace Diary.Domain
{
    public class SchoolYear : Entity
    {
        public int Year { get; private set; }
        public ICollection<Class> Classes { get; private set; }

        public SchoolYear()
        {
        }

        public SchoolYear(int year)
        {
            Year = year;

            Classes = new Collection<Class>();
        }
    }
}