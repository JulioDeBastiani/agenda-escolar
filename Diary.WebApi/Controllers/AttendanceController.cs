using System.Threading.Tasks;
using Diary.Data;
using Diary.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ApiControllerBase
    {
        private GenericRepository<User> _usersRepository;
        private GenericRepository<Attendance> _attendanceRepository;

        public AttendanceController(
            GenericRepository<User> usersRepository,
            GenericRepository<Attendance> attendanceRepository)
        {
            _usersRepository = usersRepository;
            _attendanceRepository = attendanceRepository;
        }
    }
}