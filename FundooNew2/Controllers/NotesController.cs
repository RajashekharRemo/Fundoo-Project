using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.NotesModel;
using RepositoryLayer;
using RepositoryLayer.Entity;

namespace FundooNew2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusiness _notes;
        private readonly NotesContext _notesContext;
        public NotesController(INotesBusiness notes, NotesContext notesContext)
        {
            _notes = notes;
            _notesContext= notesContext;
        }


        [HttpGet]
        [Route("GetAllNotes")]
        public IActionResult GetAll()
        {
            
                return Ok(_notesContext.Notes.ToList());
            
        }

        [HttpGet]
        [Route("GetAllNotesById")]
        public IActionResult GetAllById(int Id)
        {

            if (Id <= 0) { BadRequest(); }
            else
            {
                var notes = _notesContext.Notes.Where(x=> x.UserId == Id);
                if (notes == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(notes);
                }
            }
            return Ok();

        }


        /*[Authorize]
        [HttpPost("Create")]
        public IActionResult Post([FromForm] NotesModelClass notesModel, int UserId)
        {

            if (notesModel == null) { return BadRequest(); }
            bool flag = _notes.Create(notesModel, UserId);
            if (!flag) { return NoContent(); }
            return Ok(notesModel);

        }*/

        [Authorize]
        [HttpPost("Create")]
        public IActionResult Post(NotesModelClass notesModel)//[FromForm]
        {

            int UserId = int.Parse(User.Claims.Where(y => y.Type == "UserId").First().Value);

            if (notesModel == null) { return BadRequest(); }
            //bool flag = _notes.Create(notesModel, UserId);
            bool flag = _notes.Create(notesModel, UserId);
            if (!flag) { return NoContent(); }
            return Ok(notesModel);
            
        }

        [HttpPost("CreateNote")]
        public IActionResult Post(AngularSupportsNotes asn)
        {
            if (asn == null) { return BadRequest("null"); }
            
             bool flag=_notes.CreateAngularNotes(asn);
            if(!flag) { return NoContent(); }
            return Ok(asn);
            
        }

       // C:\Users\rajas\source\repos\FundooNew2\FundooNew2\FundooNew2.csproj

        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put([FromForm]NotesModelClass notesModel, string UserName)
        {
            if(notesModel == null || UserName==null) { return BadRequest(); }
            bool flag= _notes.Put(notesModel, UserName);
            if (flag) { return NoContent(); }
            return NotFound();

        }


        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Remove(string Title)
        {
            if(Title == null) { return BadRequest(); }
            bool flag=_notes.remove(Title);
            if (!flag) { return NotFound(); }
            return Ok();
        }

        [HttpDelete("DeleteById")]
        public IActionResult deleteById(int Id)
        {
            if (Id<=0) { return BadRequest(); }
            bool flag = _notes.deleteById(Id);
            if (!flag) { return NotFound("Not Found"); }
            return Ok(new {message="Deleted successfully", result=true});
        }


        [HttpGet("GetNotesByTitle")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult getNotesDataClasses(string Title) {
             if(Title.IsNullOrEmpty()) return BadRequest();
             var result= _notes.SelectDAtaByTitle(Title);
             if(result.IsNullOrEmpty()) return NotFound();
             else return Ok(result);
        }



        //[Authorize]
        [HttpGet("GetByDate")]
        public IActionResult GetNotesByDate(DateTime date)
        {
            var userid = int.Parse(User.Claims.Where(y => y.Type == "UserId").First().Value);
            var result = _notes.GetNotesByDate(userid, date);

            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        //[Authorize]
        [HttpPut("AddColor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddColor(int noteId, string color)
        {
            //var userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            if (color == null || noteId <= 0) { return BadRequest(); }

            var result = _notes.AddColor( noteId, color);
            if (result !=false)
            {
                return Ok("Color Added");
            }
            else
            {
                return BadRequest();
            }
        }

        /*[Authorize]
        [HttpPost]
        public IActionResult AddReminder(long noteId, DateTime reminder)
        {
            var userid = long.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _notes.AddReminder(userid, noteId, reminder);
            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }*/


        [Authorize]
        [HttpPut("ToggleArchive")]
        public IActionResult ToggleArchive(int noteId)
        {

            var userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _notes.ToggleArchive(userid, noteId);
            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [Authorize]
        [HttpPut("ToggleTrash")]
        public IActionResult ToggleTrash(int noteId)
        {

            var userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _notes.ToggleTrash(userid, noteId);
            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [Authorize] // 401 will come
        [HttpPut("TogglePin")]
        public IActionResult TogglePin(int noteId)
        {

            var userid = int.Parse(User.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value);
            var result = _notes.TogglePin(userid, noteId);
            if (result != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }


    }
}
