using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diary.Data;
using Diary.Domain;
using Diary.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;

namespace Diary.WebApi.Services
{
    public class NotificationService
    {
        private GenericRepository<Event> _eventsRepository;
        private GenericRepository<Assignment> _assignmentsRepository;
        private EmailService _emailService;

        public NotificationService(
            GenericRepository<Event> eventsRepository,
            GenericRepository<Assignment> assignmentsRepository,
            EmailService emailService)
        {
            _eventsRepository = eventsRepository;
            _assignmentsRepository = assignmentsRepository;
            _emailService = emailService;
        }
        
        public async Task HandleEventNotification(Guid eventId)
        {
            var e = await _eventsRepository.GetByIdAsync(
                eventId,
                i => i
                    .Include(r => r.Attendees)
                        .ThenInclude(r => r.User)
                    .Include(r => r.Creator));

            if (e?.Attendees == null)
                return;

            var tasks = new List<Task>();

            foreach (var user in e.Attendees.Where(i => i.Status != UserEventStatus.Declined).Select(i => i.User))
            {
                tasks.Add(_emailService.SendEmailAsync(user.Email, e.Description, "O evento para o qual você foi convidado ocorrerá amanhã"));
            }

            tasks.Add(_emailService.SendEmailAsync(e.Creator.Email, e.Description, "O evento para o qual você criou ocorrerá amanhã"));
            await Task.WhenAll(tasks);
        }

        public async Task HandleAssignmentNotification(Guid assignmentId)
        {
            var a = await _assignmentsRepository.GetByIdAsync(
                assignmentId,
                e => e
                    .Include(r => r.Class)
                        .ThenInclude(r => r.Students)
                            .ThenInclude(r => r.Student));

            if (a?.Class?.Students == null)
                return;

            var tasks = new List<Task>();

            foreach (var student in a.Class.Students.Select(s => s.Student))
            {
                tasks.Add(_emailService.SendEmailAsync(student.Email, a.Description, "O seu trabalho deverá ser entregue amanhã"));
            }
        }
    }
}