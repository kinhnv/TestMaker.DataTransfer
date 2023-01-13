using Microsoft.EntityFrameworkCore;

namespace DataTransfer.Api.Entities.TestService
{
    public class TestServiceDbContext : DbContext
    {
        public TestServiceDbContext(DbContextOptions<TestServiceDbContext> options) : base(options)
        {
        }
    }
}
