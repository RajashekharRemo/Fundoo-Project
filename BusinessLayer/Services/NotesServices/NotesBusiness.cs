using BusinessLayer.Interfaces;
using Model.NotesModel;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces.NotesInterface;
using RepositoryLayer.Services.NotesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.NotesServices
{
    public class NotesBusiness : INotesBusiness
    {
        private readonly INotesRepository notes;
        public NotesBusiness(INotesRepository notes)
        {
            this.notes = notes;
        }
        public IEnumerable<GetNotesDataClass> GetAll()
        {
            return notes.GetAll();
        }

        //public IEnumerable<GetNotesDataClass> GetNotesByUserId(int id)
        //{
        //    return notes.GetNotesByUserId(id);
        //}

        //public bool Create(NotesModelClass notesModelClass, int userId)
        //{
        //    return notes.Create(notesModelClass, userId);
        //}


        public bool Create(NotesModelClass notesModelClass, int UserId)
        {
            return notes.Create(notesModelClass, UserId);
        }

        public bool CreateAngularNotes(AngularSupportsNotes asn)
        {
            return notes.CreateAngularNotes(asn);
        }

        public bool Put(NotesModelClass notesModel, string FirstName)
        {
            return notes.Put(notesModel, FirstName);
        }


        public bool remove(string Title)
        {
            return notes.remove(Title);
        }
        public IEnumerable<GetNotesDataClass> SelectDAtaByTitle(string title)
        {
            return notes.SelectDAtaByTitle(title);
        }

        public IEnumerable<Notes> GetNotesByDate(int userid, DateTime date)
        {
            return notes.GetNotesByDate(userid, date);
        }
        public bool AddColor(int noteId, string color)
        {
            return notes.AddColor( noteId, color);
        }
        public Notes ToggleArchive(int userId, int noteId)
        {
            return notes.ToggleArchive(userId, noteId);
        }

        public Notes ToggleTrash(int userId, int noteId)
        {
            return notes.ToggleTrash(userId, noteId);
        }

        public Notes TogglePin(int userId, int noteId)
        {
            return notes.TogglePin( userId, noteId);
        }

        public bool deleteById(int Id)
        {
            return notes.deleteById(Id);
        }

    }
}
