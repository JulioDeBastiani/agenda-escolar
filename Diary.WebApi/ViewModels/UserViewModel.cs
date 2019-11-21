using System;
using Diary.Domain;

namespace Diary.WebApi.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }

        public static implicit operator UserViewModel(User user)
        {
            if (user == null)
                return null;

            return new UserViewModel
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                Name = user.Name,
                Username = user.Username,
                Type = user.Type.ToString()
            };
        }
    }
}