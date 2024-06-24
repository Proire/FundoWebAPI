using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Xml.Linq;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Utilities;

namespace FundoWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "CrudScheme")]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL noteBL;
        private readonly string UserKey = "UserId";
        private readonly ICacheService _cacheService;

        public NoteController(INoteBL noteBL, ICacheService cacheService)
        {
            this.noteBL = noteBL;
            this._cacheService = cacheService;
        }

        [HttpPost]
        public ResponseModel<NoteEntity> AddNote([FromBody] NoteModel model)
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            //// Using Session State Management
            //HttpContext.Session.SetInt32(UserKey, UserId);

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

        [HttpGet("{id}")]
        public ResponseModel<NoteEntity> GetNoteById(int id)
        {
            //int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // Using Session State Management
            int? UserId = HttpContext.Session.GetInt32(UserKey);

            if (!UserId.HasValue || UserId == 0)
            {
                return new ResponseModel<NoteEntity>{Status = false,Message = "Not Registered User",Data = default};
            }
            else
            {
                var userId = UserId.Value;
                Console.WriteLine($"Session State Successful : {userId}");
                // Fetching data from Cache
                NoteEntity noteEntity = null;
                var cacheData = _cacheService.GetData<IEnumerable<NoteEntity>>("notes");
                if (cacheData != null)
                {
                    noteEntity = cacheData.Where(x => x.Id == id && x.UserEntityId == UserId.Value).FirstOrDefault();
                    return new ResponseModel<NoteEntity> { Message = "Fetched note from cache", Data = noteEntity };
                }

                try
                {
                    var note = noteBL.GetNote(id, userId);
                    return new ResponseModel<NoteEntity>{Status = true,Message = "Note Retrieved",Data = note};
                }
                catch (Exception ex)
                {
                    return new ResponseModel<NoteEntity>{Status = false,Message = ex.Message,Data = null};
                }
            }
        }

        [Route("notes")]
        [HttpGet]
        public ResponseModel<IEnumerable<NoteEntity>> GetNotes()
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // Retrieve Data from Cache
            var cacheData = _cacheService.GetData<IEnumerable<NoteEntity>>("notes");
            if (cacheData != null)
                return new ResponseModel<IEnumerable<NoteEntity>>() { Data = cacheData, Message = "Notes Retrived from Cache" };
            try
            {
                IList<NoteEntity> Notes = noteBL.GetNotes(UserId);
                return new ResponseModel<IEnumerable<NoteEntity>>() { Data = Notes, Message = "Notes Retrived" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<NoteEntity>>() { Data = null, Message = ex.Message , Status = false};
            }
        }

        [Route("trashedNotes")]
        [HttpGet]
        public ResponseModel<ICollection<NoteEntity>> GetAllTrashedNotes()
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                ICollection<NoteEntity> Notes = noteBL.GetAllTrashedNotes(UserId);    
                return new ResponseModel<ICollection<NoteEntity>>() { Data = Notes, Message = "Trashed Notes Retrived" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ICollection<NoteEntity>>() { Data = null, Message = ex.Message, Status = false };
            }
        }

        [Route("ArchivedNotes")]
        [HttpGet]
        public ResponseModel<ICollection<NoteEntity>> GetAllArchivedNotes()
        {
            int UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                ICollection<NoteEntity> Notes = noteBL.GetAllArchievedNotes(UserId);
                return new ResponseModel<ICollection<NoteEntity>>() { Data = Notes, Message = "Archived Notes Retrived" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ICollection<NoteEntity>>() { Data = null, Message = ex.Message, Status = false };
            }
        }

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
