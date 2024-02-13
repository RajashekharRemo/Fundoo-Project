using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class TImage
    {
        public int Id { get; set; } 
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }
        [ForeignKey("Notes")]
        public int NoteId { get; set; }
        [JsonIgnore]
        public virtual Notes? Notes { get; set; }
    }
}
