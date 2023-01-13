using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Api.Entities.EventService
{
    public class UserQuestion
    {
        public UserQuestion()
        {
            Rank = 0;
            LastUpdate = DateTime.UtcNow;
            IsDeleted = false;
        }

        public Guid UserId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid TestId { get; set; }

        [Required]
        public double Rank { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
