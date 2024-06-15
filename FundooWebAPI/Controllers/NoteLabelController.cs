using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;

namespace FundoWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "CrudScheme")]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteLabelController(INoteLabelBL notelabelBL) : ControllerBase
    {
        private readonly INoteLabelBL _noteLabelBL = notelabelBL;

        [HttpPost("labels/{labelId}/notes/{noteId}")]
        public ResponseModel<string> AddNoteToLabel(int labelId, int noteId)
        {
            try
            {
                var status = _noteLabelBL.AddNoteToLabel(labelId, noteId);
                ResponseModel<string> responseModel = new ResponseModel<string>() { Message = "Added Note to Label", Data = status };
                return responseModel;
            }
            catch (Exception ie)
            {
                return new ResponseModel<string>() { Status = false,Message = "Error Occured while Adding Note", Data = ie.Message };
            }
        }

        [HttpGet("labels/{labelId}/notes")]
        public ResponseModel<IEnumerable<NoteEntity>> GetNotesForLabel(int labelId)
        {
            try
            {
                var notes = _noteLabelBL.GetNotesForLabel(labelId);    
                return new ResponseModel<IEnumerable<NoteEntity>>() { Message = $"Retrieved Notes with Labelid :{labelId}", Data = notes };
            }
            catch (Exception)
            {
                return new ResponseModel<IEnumerable<NoteEntity>>() { Status = false ,Message = "Error Occured while retrive Notes", Data = null };
            }
        }

        [HttpGet("notes/{noteId}/labels")]
        public ResponseModel<IEnumerable<LabelEntity>> GetLabelsForNote(int noteId)
        {
            try
            {
                var labels = _noteLabelBL.GetLabelsForNotes(noteId);
                return new ResponseModel<IEnumerable<LabelEntity>>() { Message = $"Retrieved Labels with Noteid :{noteId}", Data = labels };
            }
            catch (Exception)
            {
                return new ResponseModel<IEnumerable<LabelEntity>>() { Status = false, Message = "Error Occured while retrive labels", Data = null };
            }
        }

        [HttpDelete("notes/{noteId}/labels/{labelId}")]
        public ResponseModel<NoteLabelEntity> RemoveLabelFromNote(int noteId, int labelId)
        {
            try
            {
                var label = _noteLabelBL.RemovelabelfromNote(noteId, labelId);
                ResponseModel<NoteLabelEntity> responseModel = new ResponseModel<NoteLabelEntity>() { Message = "Removed Label from note", Data = label };
                return responseModel;
            }
            catch (Exception ie)
            {
                return new ResponseModel<NoteLabelEntity>() { Status = false, Message = "Error Occured while Adding Note", Data = null };
            }
        }
    }
}
