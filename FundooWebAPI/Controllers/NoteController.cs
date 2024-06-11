using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;
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

        [HttpPost]
        public ResponseModel<NoteEntity> AddNote([FromBody] NoteModel model)
        {
            try
            {
                var addedNote = noteBL.CreateNote(model);
                return new ResponseModel<NoteEntity> { Message="Note Added", Data = addedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity> { Status=false, Message=ex.Message, Data=null};
            }
        }

        [HttpGet("{id}")]
        public ResponseModel<NoteEntity> GetNoteById(int id)
        {
            try
            {
                var node = noteBL.GetNote(id);
                return new ResponseModel<NoteEntity>() { Message = "Note Retrieved", Data = node };

            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data= null };
            }
        }

        [Route("notes")]
        [HttpGet]
        public ResponseModel<IList<NoteEntity>> GetNotes()
        {
            try
            {
                IList<NoteEntity> Notes = noteBL.GetNotes();
                return new ResponseModel<IList<NoteEntity>>() { Data = Notes, Message = "Notes Retrived" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IList<NoteEntity>>() { Data = null, Message = ex.Message };
            }
        }

        [HttpDelete("{id}")]
        public ResponseModel<NoteEntity> DeleteNote(int id)
        {
            try
            {
                var deletedNote = noteBL.DeleteNote(id);
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
            try
            {
                var updatedNote = noteBL.UpdateNote(id, node);
                return new ResponseModel<NoteEntity>() { Message = "Note Updated", Data = updatedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<NoteEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }
    }
}
