using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.NotesModel
{
    public class AngularSupportsNotes
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        //public ICollection<IFormFile>? Files { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Reminder { get; set; }
        public bool IsArchive { get; set; }
        public bool IsPinned { get; set; }
        public bool IsTrash { get; set; }
        public int UserId { get; set; }
    }
}
