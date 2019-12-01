using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Diary.Domain.Base;
using Diary.Domain.Enumerators;

namespace Diary.Domain
{
    public class User : Entity
    {
        public string Name { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public UserType Type { get; private set; }
        public ICollection<Class> TaughtClasses { get; private set; }
        public ICollection<StudentClass> Classes { get; private set; }
        public ICollection<Appointment> AppointmentsCreated { get; private set; }
        public ICollection<Appointment> Appointments { get; private set; }
        public ICollection<Event> EventsCreated { get; private set; }
        public ICollection<UserEvent> Events { get; private set; }
        public ICollection<UserGuardian> Guardians { get; private set; }
        public ICollection<UserGuardian> Dependents { get; private set; }

        private User()
        {
        }

        public User(string name, string username, string email, string password, UserType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));
            
            Name = name;
            Username = username;
            Email = email;
            Password = password;
            Type = type;

            TaughtClasses = new Collection<Class>();
            Classes = new Collection<StudentClass>();
            AppointmentsCreated = new Collection<Appointment>();
            Appointments = new Collection<Appointment>();
            EventsCreated = new Collection<Event>();
            Events = new Collection<UserEvent>();
            Guardians = new Collection<UserGuardian>();
            Dependents = new Collection<UserGuardian>();
        }

        public bool ComparePassword(string password)
        {
            return Password == password;
        }
    }
}