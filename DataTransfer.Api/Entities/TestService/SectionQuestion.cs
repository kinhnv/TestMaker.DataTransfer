using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer.Api.Entities.TestService
{
    public class SectionQuestion
    {
        public SectionQuestion()
        {
            LastUpdate = DateTime.UtcNow;
        }

        [Required]
        public Guid SectionId { get; set; }

        [Required]
        public Guid QuestionId { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
