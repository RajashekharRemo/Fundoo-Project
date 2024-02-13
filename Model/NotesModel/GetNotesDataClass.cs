﻿using Microsoft.AspNetCore.Http;
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
        
            [Required]
            public string? Title { get; set; }
            [Required]
            public string? Description { get; set; }
            [Required]
            public string? Color { get; set; }
            //public ICollection<IFormFile>? Files { get; set; }
            [Required]
            public string Files { get; set; }
            //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            [DefaultValue("2024-05-12 12:12:55.5343")]
            public DateTime Reminder { get; set; }
            [Required]
            public bool IsArchive { get; set; }
            public bool IsPinned { get; set; }
            [Required]
            public bool IsTrash { get; set; }
            [JsonIgnore]
            public int UserId { get; set; }
        
    }
}
