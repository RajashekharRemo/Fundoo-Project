
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
using Microsoft.Data.SqlClient;
using MassTransit.Testing;

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

        public IEnumerable<GetNotesDataClass> GetAll()
        {
            var All = _context.Notes.ToList();
            if (All == null) { return null; }
            List<GetNotesDataClass> list = new List<GetNotesDataClass>();
            GetNotesDataClass nmc = new GetNotesDataClass();

            foreach (var item in All)
            {
                nmc.Id = item.Id.ToString();
                nmc.Title = item.Title; nmc.Description = item.Description;
                nmc.Color = item.Color; nmc.Reminder = item.Reminder;
                nmc.IsPinned = item.IsPinned; nmc.IsArchive = item.IsArchive;
                nmc.IsTrash = item.IsTrash;
                nmc.UserId = item.UserId.ToString();
                var ImageResult = _context.Images.FirstOrDefault(s => s.Id == item.Id);
                nmc.Files = ImageResult.ImageUrl;


                list.Add(nmc);
            }

            return list;

            //_context.Notes.ToList();

            //string path = "Data Source=LAPTOP-MUFM59UB\\SQLEXPRESS;Initial Catalog=mydatabase;Integrated Security=True; Encrypt=False";
            //using(SqlConnection con=new SqlConnection(path))
            //{
            //    con.Open();
            //    SqlCommand cmd = new SqlCommand("select * from Notes", con);
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        gnd.Id = reader[0].ToString();
            //        gnd.Title= reader[1].ToString();
            //        gnd.Description= reader[2].ToString();
            //        gnd.Color= reader[3].ToString();
            //        gnd.Reminder = Convert.ToDateTime(reader[4].ToString());
            //        gnd.IsArchive = reader[5].ToString();
            //        gnd.IsPinned= reader[6].ToString();
            //        gnd.IsTrash= reader[7].ToString();
            //        gnd.UserId= reader[10].ToString();
            //        var ImageResult = _context.Images.FirstOrDefault(s => s.Id == Convert.ToInt32(reader[0].ToString()));
            //        gnd.Files = ImageResult.ImageUrl;
            //        list.Add(gnd);
            //    }
            //}
            //return list;

        }


        //public IEnumerable<GetNotesDataClass> GetNotesByUserId(int id)
        //{
        //    var All = _context.Notes.Where(x=>x.UserId==id);
        //    if (All == null) { return null; }
        //    List<GetNotesDataClass> list = new List<GetNotesDataClass>();
        //    GetNotesDataClass nmc = new GetNotesDataClass();

        //    foreach (var item in All)
        //    {
        //        nmc.Id = item.Id.ToString();
        //        nmc.Title = item.Title; nmc.Description = item.Description;
        //        nmc.Color = item.Color; nmc.Reminder = item.Reminder;
        //        nmc.IsPinned = item.IsPinned; nmc.IsArchive = item.IsArchive;
        //        nmc.IsTrash = item.IsTrash;
        //        nmc.UserId = item.UserId.ToString();
        //        var ImageResult = _context.Images.FirstOrDefault(s => s.Id == item.Id);
        //        nmc.Files = ImageResult.ImageUrl;


        //        list.Add(nmc);
        //    }

        //    return list;
        //}


        public bool Create(NotesModelClass notesModelClass,int  UserId)
        {
            IEnumerable<TImage> imageList = null;
            User user = _userContext.OnlineUser2.FirstOrDefault(s => s.Id == UserId);
            if (user != null)
            {
                Notes notes = new Notes();
                notes.Title = notesModelClass.Title; notes.Description = notesModelClass.Description;
                notes.Color = notesModelClass.Color;
                notes.Reminder = notesModelClass.Reminder; notes.IsArchive = notesModelClass.IsArchive;
                notes.IsPinned = notesModelClass.IsPinned; notes.IsTrash = notesModelClass.IsTrash;
                notes.CreatedAt = DateTime.Now; notes.ModifiedAt = DateTime.Now;
                notes.UserId = UserId;

                _context.Notes.Add(notes);
                _context.SaveChanges();
                //if(notesModelClass.Files != null)
                //{
                //    imageList=AddImageToDb(notes.Id, notesModelClass.UserId, notesModelClass.Files);
                //}
            }
            else { return false; }

            /*foreach(var image  in notesModelClass.Files)
            {
                notes.ImageName = image.Name;
                notes.ImageUrl = ConvertImage(image).ToString();
            } */

            
            return true;
        }

        public bool CreateAngularNotes(AngularSupportsNotes asn)
        {

            User user = _userContext.OnlineUser2.FirstOrDefault(s => s.Id == asn.UserId);
            if (user != null)
            {
                Notes notes = new Notes();
                notes.Title = asn.Title; notes.Description = asn.Description;
                notes.Color = asn.Color;
                notes.Reminder = asn.Reminder; notes.IsArchive = asn.IsArchive;
                notes.IsPinned = asn.IsPinned; notes.IsTrash = asn.IsTrash;
                notes.CreatedAt = DateTime.Now; notes.ModifiedAt = DateTime.Now;
                notes.UserId = asn.UserId;

                _context.Notes.Add(notes);
                _context.SaveChanges();
                
            }
            else { return false; }
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
           // var image = _context.Images.FirstOrDefault(s => s.NoteId == notes.Id);

            if (user==null  || notes == null) { return false; }//|| image == null
            notes.Title=notesModel.Title; notes.Description=notesModel.Description;
             notes.ModifiedAt=DateTime.Now;notes.Color=notesModel.Color;
            notes.IsArchive=notesModel.IsArchive; notes.IsPinned=notesModel.IsPinned;
            notes.IsTrash=notesModel.IsTrash;
            //foreach(var file in notesModel.Files)
            //{
            //    image.ImageName = file.Name;
            //    image.ImageUrl = ConvertImage(file).ToString();
            //}
            _context.Entry(notes).State=EntityState.Modified;
            //_context.Entry(image).State=EntityState.Modified;
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

        public bool deleteById(int Id)
        {
            var notes = _context.Notes.FirstOrDefault(s => s.Id == Id);
            if ( notes == null) { return false; }
            NotesModelClass notesModel = new NotesModelClass();
            _context.Notes.Remove(notes);
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
                getNotesDataClass.UserId = note.UserId.ToString();
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

                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
                else
                {
                    note.IsArchive = true;
                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
            }

        }


        public Notes ToggleTrash(int userId, int noteId)
        {
            var note = _context.Notes.Where(x => x.UserId == userId && x.Id == noteId).FirstOrDefault();
            if (note == null)
            {
                return null;
            }
            else
            {
                if (note.IsTrash == true)
                {
                    note.IsTrash = false;

                    _context.Entry(note).State = EntityState.Modified;
                    _context.SaveChanges();
                    return note;
                }
                else
                {
                    note.IsTrash = true;
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
