using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Model.NotesModel
{
    public class GetNotesDataClass
    {
             public string? Id { get; set; }
        
            public string? Title { get; set; }
            public string? Description { get; set; }
            public string? Color { get; set; }
            //public ICollection<IFormFile>? Files { get; set; }
            public string? Files { get; set; }
            //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public DateTime Reminder { get; set; }
            public bool IsArchive { get; set; }
            public bool IsPinned { get; set; }
            public bool IsTrash { get; set; }
            public string? UserId { get; set; }
        
    }
}
