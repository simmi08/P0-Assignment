using AssignmentP0.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AssignmentP0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendlyController : ControllerBase
    {
        private readonly CalendlyContext _context;

        public CalendlyController(CalendlyContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }
        [HttpGet]
        [Route ("/availability/{userID}")]
        public async Task<IActionResult> GetAvailability(string userID)
        {
            var user = await _context.Users
                .Include(u => u.Availabilities)
                .SingleOrDefaultAsync(u => u.UserID == userID);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user.Availabilities);
        }

        [HttpPost]
        [Route("/availability")]
        public async Task<IActionResult> SetAvailability(AvailabilityRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Availabilities)
                .SingleOrDefaultAsync(u => u.UserID == request.UserID && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid UserID or Password");
            }

            if (DateTime.TryParse(request.Date, out DateTime date) &&
                DateTime.TryParse(request.StartTime, out DateTime startTime) &&
                DateTime.TryParse(request.EndTime, out DateTime endTime))
            {
                var availability = new Availability
                {
                    UserID = request.UserID,
                    Date = date.ToString("yyyy-MM-dd"),
                    StartTime = date.Add(startTime.TimeOfDay),
                    EndTime = date.Add(endTime.TimeOfDay)
                };

                user.Availabilities?.Add(availability);

                await _context.SaveChangesAsync();

                return Ok("Availability added successfully");
            }
            else
            {
                return BadRequest("Invalid Date, StartTime, or EndTime format");
            }
        }

        [HttpPost]
        [Route("/createUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationRequest request)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.UserID == request.UserID);
            if (existingUser != null)
            {
                return Conflict("User already exists");
            }

            var user = new User
            {
                UserID = request.UserID,
                Name = request.Name,
                Password = request.Password,
                Availabilities = new List<Availability>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User created successfully");
        }

        [HttpGet("/overlap")]
        public async Task<IActionResult> GetOverlapAvailability([FromQuery] OverlapRequest request)
        {
            if (!DateTime.TryParseExact(request.Date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                return BadRequest("Invalid Date format");
            }

            var user1 = await _context.Users
                .Include(u => u.Availabilities)
                .SingleOrDefaultAsync(u => u.UserID == request.UserID1);

            var user2 = await _context.Users
                .Include(u => u.Availabilities)
                .SingleOrDefaultAsync(u => u.UserID == request.UserID2);

            if (user1 == null || user2 == null)
            {
                return NotFound("One or both users not found");
            }

            var user1Availabilities = user1.Availabilities?.Where(a => a.Date == date.ToString("yyyy-MM-dd")).ToList();
            var user2Availabilities = user2.Availabilities?.Where(a => a.Date == date.ToString("yyyy-MM-dd")).ToList();

            if (user1Availabilities == null || user2Availabilities == null)
            {
                return Ok("No availabilities found for the given date");
            }

            var overlappingAvailabilities = new List<Availability>();

            foreach (var avail1 in user1Availabilities)
            {
                foreach (var avail2 in user2Availabilities)
                {
                    var overlapStart = avail1.StartTime > avail2.StartTime ? avail1.StartTime : avail2.StartTime;
                    var overlapEnd = avail1.EndTime < avail2.EndTime ? avail1.EndTime : avail2.EndTime;

                    if (overlapStart < overlapEnd)
                    {
                        overlappingAvailabilities.Add(new Availability
                        {
                            StartTime = overlapStart,
                            EndTime = overlapEnd,
                            Date = avail1.Date
                        });
                    }
                }
            }

            return Ok(overlappingAvailabilities);
        }
    }
}
