using DataTransfer.Api.Entities.TestService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataTransfer.Api.Entities.EventService
{
    public class EventServiceDbContext : DbContext
    {
        public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options) : base(options)
        {
        }
        public DbSet<UserQuestion> UserQuestions { get; set; }
    }
}
