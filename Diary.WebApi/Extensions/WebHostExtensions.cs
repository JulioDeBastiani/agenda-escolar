using System.Linq;
using Diary.Data;
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
                        var admin = new User("admin", "admin", "email", "admin", UserType.Admin);
                        context.Users.Add(admin);

                        var teacher = new User("iraci", "iraci", "email", "pass", UserType.Teacher);
                        context.Users.Add(teacher);

                        var julio = new User("julio", "julio", "jcsbastiani@ucs.br", "pass", UserType.Student);
                        context.Users.Add(julio);

                        var guilherme = new User("guilherme", "guilherme", "email", "pass", UserType.Student);
                        context.Users.Add(guilherme);

                        var cesar = new User("cesar", "cesar", "email", "pass", UserType.Student);
                        context.Users.Add(cesar);

                        var matheus = new User("matheus", "matheus", "email", "pass", UserType.Student);
                        context.Users.Add(matheus);

                        var sy = new SchoolYear(2019);
                        context.SchoolYears.Add(sy);

                        var sub = new Subject("Fundamentos");
                        context.Subjects.Add(sub);

                        var cls = new Class(sub, sy, 4, teacher);
                        context.Classes.Add(cls);

                        context.StudentClasses.Add(new StudentClass(julio, cls));
                        context.StudentClasses.Add(new StudentClass(guilherme, cls));
                        context.StudentClasses.Add(new StudentClass(cesar, cls));
                        context.StudentClasses.Add(new StudentClass(matheus, cls));

                        context.SaveChanges();
                    }
                }
            }

            return host;
        }
    }
}