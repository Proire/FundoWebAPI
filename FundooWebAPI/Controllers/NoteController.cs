using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Security.Claims;
using System.Xml.Linq;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;

namespace FundoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL noteBL;

        public NoteController(INoteBL noteBL)
        {
            this.noteBL = noteBL;
        }

        [Authorize]
        [HttpPost]
        public ResponseModel<NoteEntity> AddNote([FromBody] NoteModel model)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var addedNote = noteBL.CreateNote(model,UserId);
                return new ResponseModel<NoteEntity> { Message="Note Added", Data = addedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity> { Status=false, Message=ex.Message, Data= null};
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public ResponseModel<NoteEntity> GetNoteById(int id)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var node = noteBL.GetNote(id,UserId);
                return new ResponseModel<NoteEntity>() { Message = "Note Retrieved", Data = node };

            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }

        [Authorize]
        [Route("notes")]
        [HttpGet]
        public ResponseModel<IList<NoteEntity>> GetNotes()
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                IList<NoteEntity> Notes = noteBL.GetNotes(UserId);
                return new ResponseModel<IList<NoteEntity>>() { Data = Notes, Message = "Notes Retrived" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IList<NoteEntity>>() { Data = null, Message = ex.Message };
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public ResponseModel<NoteEntity> DeleteNote(int id)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var deletedNote = noteBL.DeleteNote(id,UserId);
                return new ResponseModel<NoteEntity>() { Message = "Note Deleted", Data = deletedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public ResponseModel<NoteEntity> UpdateNote(int id, [FromBody] NoteModel node)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var updatedNote = noteBL.UpdateNote(id, node,UserId);
                return new ResponseModel<NoteEntity>() { Message = "Note Updated", Data = updatedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }


        [Authorize]
        [HttpGet("archive/{id}")]
        public ResponseModel<NoteEntity> ArchiveNote(int id)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var Note = noteBL.ArchiveNote(id,UserId);
                var status = (Note.IsArchived) ? "Archived" : "UnArchived";
                return new ResponseModel<NoteEntity>() { Message = $"Note {status}", Data = Note};
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }

        [Authorize]
        [HttpGet("trash/{id}")]
        public ResponseModel<NoteEntity> TrashNote(int id)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var Note = noteBL.TrashNote(id, UserId);
                var status = (Note.IsTrashed) ? "Trashed" : "UnTrashed";
                return new ResponseModel<NoteEntity>() { Message = $"Note {status}", Data = Note };
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }
    }
}
