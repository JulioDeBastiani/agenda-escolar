using System.Linq;
using Arq.Data;
using Diary.Domain;
using Diary.Domain.Enumerators;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Diary.WebApi.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost Seed(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();

                    var user = context.Users.FirstOrDefault();
                    if (user == null)
                    {
                        user = new User("def", "def", "pass", UserType.Administrator);
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                }
            }

            return host;
        }
    }
}