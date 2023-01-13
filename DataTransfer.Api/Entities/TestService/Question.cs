using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataTransfer.Api.Entities.TestService
{
    public class Question
    {
        public Question()
        {
            LastUpdate = DateTime.UtcNow;
            IsDeleted = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid QuestionId { get; set; }

        public string Media { get; set; }

        public int Type { get; set; }

        [JsonIgnore]
        [Required]
        public string ContentAsJson { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
