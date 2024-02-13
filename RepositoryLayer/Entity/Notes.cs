using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Notes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DefaultValue("2024-05-12 12:12:55.5343")]
        public DateTime Reminder { get; set; }

        public bool IsArchive { get; set; }

        public bool IsPinned { get; set; }

        public bool IsTrash { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }

    }
}
