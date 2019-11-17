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
                        var admin = new User("def", "def", "pass", UserType.Admin);
                        context.Users.Add(admin);

                        var student = new User("stu", "stu", "pass", UserType.Student);
                        context.Users.Add(student);

                        var teacher = new User("te", "te", "pass", UserType.Teacher);
                        context.Users.Add(teacher);

                        var gur = new User("gur", "gur", "pass", UserType.Guardian);
                        context.Users.Add(gur);

                        var sy = new SchoolYear(2019);
                        context.SchoolYears.Add(sy);

                        var sub = new Subject("Math");
                        context.Subjects.Add(sub);

                        var cls = new Class(sub, sy, 4, teacher);
                        context.Classes.Add(cls);

                        var sc = new StudentClass(student, cls);

                        context.SaveChanges();
                    }
                }
            }

            return host;
        }
    }
}