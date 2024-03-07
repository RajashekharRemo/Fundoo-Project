using Model.NotesModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces.NotesInterface
{
    public interface INotesRepository
    {
        public IEnumerable<GetNotesDataClass> GetAll();
        // public IEnumerable<GetNotesDataClass> GetNotesByUserId(int id);
        //public bool Create(NotesModelClass notesModelClass, int userId);
        public bool Create(NotesModelClass notesModelClass, int UserId);

        public bool CreateAngularNotes(AngularSupportsNotes asn);

        public bool Put(NotesModelClass notesModel, string FirstName);

        public bool remove(string Title);

        public IEnumerable<GetNotesDataClass> SelectDAtaByTitle(string title);

        public IEnumerable<Notes> GetNotesByDate(int userid, DateTime date);
        public bool AddColor(int noteId, string color);
        public Notes ToggleArchive(int userId, int noteId);
        public Notes ToggleTrash(int userId, int noteId);
        public Notes TogglePin(int userId, int noteId);

        public bool deleteById(int Id);

    }
}
