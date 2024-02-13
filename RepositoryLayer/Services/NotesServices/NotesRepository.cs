using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Model.NotesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces.NotesInterface;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace RepositoryLayer.Services.NotesServices
{
    public class NotesRepository : INotesRepository
    {
        private readonly NotesContext _context;
        private readonly UserContext _userContext;
        public NotesRepository(NotesContext context, UserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        AddFileToFormFile AddFile = new AddFileToFormFile();

        public string ConvertImage(IFormFile file)
        {
            try
            {                
                string originalFileName = file.FileName;
                string UniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now.Ticks}{Path.GetExtension(originalFileName)}";
                string filePath = FileHelper.GetFilePath(UniqueFileName);
                using (var filestream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(filestream);
                }
                return filePath;

                /*string path= Guid.NewGuid().ToString()+"_"+file.FileName;
                string serverFolder=Path.Combine()
                file.CopyTo(new FileStream())*/


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public IEnumerable<NotesModelClass> GetAll()
        {
            var All = _context.Notes.ToList();
            if (All == null) { return null; }
            List<NotesModelClass> list = new List<NotesModelClass>();
            NotesModelClass nmc = new NotesModelClass();

            foreach (var item in All)
            {
                nmc.Title= item.Title; nmc.Description= item.Description;
                nmc.Color= item.Color; nmc.Reminder= item.Reminder;
                nmc.IsPinned= item.IsPinned; nmc.IsArchive= item.IsArchive;
                nmc.IsTrash= item.IsTrash;nmc.UserId = (int)item.UserId;
                var ImageResult=_context.Images.FirstOrDefault(s=>s.Id==item.Id);
                nmc.Files = AddFileToFormFileMethod(ImageResult.ImageUrl);
                list.Add(nmc);
            }
            /*
            foreach(var note in All)
            {
                nmc= new NotesModelClass();
                nmc.Title = note.Title;nmc.Description = note.Description;nmc.Color = note.Color;
                nmc.Files = AddFile.AddFileToFormFileMethod(note.Files); nmc.Reminder = note.Reminder;
                nmc.IsArchive= note.IsArchive;nmc.IsPinned= note.IsPinned;nmc.IsTrash= note.IsTrash;

                list.Add(nmc);
            }*/
            return list;
        }

        public bool Create(NotesModelClass notesModelClass, int userId)
        {
            IEnumerable<TImage> imageList = null;
            User user = _userContext.OnlineUser2.FirstOrDefault(s => s.Id == userId);
            if (user != null)
            {
                Notes notes = new Notes();
                notes.Title = notesModelClass.Title; notes.Description = notesModelClass.Description;
                notes.Color = notesModelClass.Color;
                notes.Reminder = notesModelClass.Reminder; notes.IsArchive = notesModelClass.IsArchive;
                notes.IsPinned = notesModelClass.IsPinned; notes.IsTrash = notesModelClass.IsTrash;
                notes.CreatedAt = DateTime.Now; notes.ModifiedAt = DateTime.Now;
                notes.UserId = notesModelClass.UserId;

                _context.Notes.Add(notes);
                _context.SaveChanges();
                if(notesModelClass.Files != null)
                {
                    imageList=AddImageToDb(notes.Id, userId, notesModelClass.Files);
                }
            }
            else { return false; }

            /*foreach(var image  in notesModelClass.Files)
            {
                notes.ImageName = image.Name;
                notes.ImageUrl = ConvertImage(image).ToString();
            } */

            
            return true;
        }


        public IEnumerable<TImage> AddImageToDb(int noteId, int UserId, ICollection<IFormFile> files)
        {
            try
            {

                Notes resNotes = null;
                var user = _context.Notes.FirstOrDefault(s => s.UserId == UserId);
                if (user != null)
                {
                    resNotes = _context.Notes.Where(n => n.UserId == UserId && n.Id == noteId).FirstOrDefault();
                    if (resNotes != null)
                    {
                        IList<TImage> imagesList = new List<TImage>();
                        foreach (var file in files)
                        {  
                            TImage image = new TImage();
                            //var uploadImageResult=ConvertImage(file);
                            image.NoteId = noteId;
                            image.ImageName = file.FileName; 
                            image.ImageUrl = ConvertImage(file).ToString();
                            imagesList.Add(image);
                            _context.Images.Add(image);
                            _context.SaveChanges();
                            resNotes.ModifiedAt = DateTime.Now;
                            _context.Notes.Update(resNotes);
                            _context.SaveChanges();

                        }
                        return imagesList;
                    }
                    else { return null; }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Put(NotesModelClass notesModel, string FirstName)
        {
            var user = _userContext.OnlineUser2.FirstOrDefault(s => s.First_Name == FirstName);
            var notes = _context.Notes.FirstOrDefault(s => s.UserId == user.Id);
            var image = _context.Images.FirstOrDefault(s => s.NoteId == notes.Id);

            if (user==null || image == null || notes == null) { return false; }
             notes.Title=notesModel.Title; notes.Description=notesModel.Description;
             notes.ModifiedAt=DateTime.Now;notes.Color=notesModel.Color;
            notes.IsArchive=notesModel.IsArchive; notes.IsPinned=notesModel.IsPinned;
            notes.IsTrash=notesModel.IsTrash;
            foreach(var file in notesModel.Files)
            {
                image.ImageName = file.Name;
                image.ImageUrl = ConvertImage(file).ToString();
            }
            _context.Entry(notes).State=EntityState.Modified;
            _context.Entry(image).State=EntityState.Modified;
            _context.SaveChanges();
            return true;

        }

        public bool remove(string Title)
        {
            var notes=_context.Notes.FirstOrDefault(s=>s.Title == Title);
            var image = _context.Images.FirstOrDefault(s => s.NoteId == notes.Id);
            if(image == null || notes==null) { return false; }
            NotesModelClass notesModel=new NotesModelClass();
            _context.Notes.Remove(notes);
            _context.Images.Remove(image);
            _context.SaveChanges();
            return true;
        }

        public ICollection<IFormFile> AddFileToFormFileMethod(string fileName)
        {
            /*string stringValue = fileName;

            // Read the file contents as a byte array
            byte[] fileBytes = File.ReadAllBytes(stringValue);

            // Create a stream from the byte array
            Stream fileStream = new MemoryStream(fileBytes);

            // Create a FormFile instance from the stream, file name, and content type
            IFormFile fileValue = new FormFile(fileStream, 0, fileBytes.Length, "file", stringValue);

            // Create a collection of files with one element
            ICollection<IFormFile> filesValue = new List<IFormFile>() { fileValue };
            return filesValue;*/




            //Create a new List of IFormFile to store the converted file
            List<IFormFile> files = new List<IFormFile>();

            //Open the file stream and create a new FormFile object
            using (var stream = System.IO.File.OpenRead(fileName))
            {
                //The FormFile constructor takes four parameters: the stream, the content offset, the content length, and the file name
                FormFile file = new FormFile(stream, 0, stream.Length, Path.GetFileName(fileName), Path.GetFileName(fileName));

                //Add the FormFile object to the List
                files.Add(file);
            }

            //Now you have an ICollection<IFormFile> that contains the converted file
            ICollection<IFormFile> collection = files;
            return collection;
        }



        public IEnumerable<GetNotesDataClass> SelectDAtaByTitle(string title)
        {
            var notes= _context.Notes.Where(s=>s.Title==title).ToList();

            if (notes == null) return null;

            List<GetNotesDataClass> list = new List<GetNotesDataClass>();
            foreach (var note in notes)
            {
                var image = _context.Images.FirstOrDefault(s => s.NoteId == note.Id);
                GetNotesDataClass getNotesDataClass = new GetNotesDataClass();
                getNotesDataClass.Title = note.Title;
                getNotesDataClass.Description = note.Description;
                getNotesDataClass.Reminder = note.Reminder;
                getNotesDataClass.IsPinned = note.IsPinned;
                getNotesDataClass.UserId = (int)note.UserId;
                getNotesDataClass.Files = image.ImageUrl;
                getNotesDataClass.Color = note.Color;
                getNotesDataClass.IsArchive = note.IsArchive;
                getNotesDataClass.IsTrash = note.IsTrash;
                list.Add(getNotesDataClass);

            }
            return list;

        }

        public IEnumerable<Notes> GetNotesByDate(int userid, DateTime date)
        {
            IEnumerable<Notes> user = _context.Notes.Where(x => x.UserId == userid && x.Reminder.Date == date.Date).ToList();

            if (user.Any())
            {
                return user.ToList();
            }
            return null;
        }

        public bool AddColor( int noteId, string color)
        {
            var note = _context.Notes.FirstOrDefault(x => x.Id == noteId);
            if (note == null)
            {
                return false;
            }
            else
            {
                note.Color = color;
                note.ModifiedAt= DateTime.Now;
                _context.Entry(note).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }

        }

        public Notes ToggleArchive(int userId, int noteId)
        {
            var note = _context.Notes.Where(x => x.UserId==userId && x.Id == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {
                if (note.IsArchive == true)
                {
                    note.IsArchive = false;

                    if (note.IsPinned == true)
                    {
                        note.IsPinned = false;
                    }
                    if (note.IsTrash == true)
                    {
                        note.IsTrash = false;
                    }
                    note.ModifiedAt = DateTime.Now;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
                else
                {
                    note.IsArchive = true;
                    if (note.IsPinned == true)
                    {
                        note.IsPinned = false;
                    }
                    if (note.IsTrash == true)
                    {
                        note.IsTrash = false;
                    }
                    note.ModifiedAt = DateTime.Now;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
            }

        }


        public Notes TogglePin(int userId, int noteId)
        {
            var note = _context.Notes.Where(x => x.UserId == userId && x.Id == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {
                if (note.IsPinned == true)
                {
                    note.IsPinned = false;
                    note.ModifiedAt = DateTime.Now;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();

                }
                else
                {
                    note.IsPinned = true;
                    note.ModifiedAt = DateTime.Now;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return note;
            }

        }


    }
}
