using Diary.Domain;
using Microsoft.EntityFrameworkCore;

namespace Diary.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<SchoolYear> SchoolYears { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<UserGuardian> UserGuardians { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Creator)
                .WithMany(c => c.AppointmentsCreated)
                .HasForeignKey(a => a.CreatorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Target)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.TargetId);

            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new 
                {
                    a.Date,
                    a.StudentClassId
                })
                .IsUnique();

            modelBuilder.Entity<Grade>()
                .HasIndex(g => new
                {
                    g.AssignmentId,
                    g.StudentId
                })
                .IsUnique();

            modelBuilder.Entity<SchoolYear>()
                .HasIndex(s => s.Year)
                .IsUnique();

            modelBuilder.Entity<StudentClass>()
                .HasIndex(s => new
                {
                    s.StudentId,
                    s.ClassId
                })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<UserEvent>()
                .HasIndex(u => new
                {
                    u.UserId,
                    u.EventId
                })
                .IsUnique();
                
            modelBuilder.Entity<UserGuardian>()
                .HasOne(u => u.Guardian)
                .WithMany(u => u.Dependents)
                .HasForeignKey(u => u.GuardianId);

            modelBuilder.Entity<UserGuardian>()
                .HasOne(u => u.Student)
                .WithMany(u => u.Guardians)
                .HasForeignKey(u => u.StudentId);

            modelBuilder.Entity<UserGuardian>()
                .HasIndex(u => new
                {
                    u.StudentId,
                    u.GuardianId,
                })
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}