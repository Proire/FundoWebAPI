using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;

namespace FundoWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "CrudScheme")]
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController(ICollaboratorBL collaboratorBL) : ControllerBase
    {
        private readonly ICollaboratorBL _collaboratorBL = collaboratorBL;

        [Route("addCollaborator/{noteid}")]
        [HttpPost]
        public ResponseModel<string> AddCollaborator(int noteId ,[FromBody]string collaborator)
        {
            try
            {
                _collaboratorBL.AddCollaborator(noteId,collaborator);
                return new ResponseModel<string>() { Message = "Added Collaborator", Data = "Successful operation" };
            }
            catch(Exception ex)
            {
                return new ResponseModel<string>() { Message = "Problem Occured while Adding Collaborator", Data =ex.Message };
            }
        }

        [Route("removeCollaborator/{noteid}")]
        [HttpDelete]
        public ResponseModel<string> RemoveCollaborator(int noteid)
        {
            try
            {
                _collaboratorBL.RemoveCollaborator(noteid);
                return new ResponseModel<string>() { Message = "Removed Collaborator", Data = "Successful operation" };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string>() { Message = "Problem Occured while Removing Collaborator", Data = ex.Message };
            }
        }

        [Route("getCollaborators/noteId/{noteId}")]
        [HttpGet]
        public ResponseModel<ICollection<CollaboraterEntity>> GetCollaborators(int noteId)
        {
            try
            {
                var collaborators = _collaboratorBL.GetCollaborators(noteId);
                return new ResponseModel<ICollection<CollaboraterEntity>>() { Message = "Collaborators Retrieved", Data = collaborators };
            }
            catch (Exception)
            {
                return new ResponseModel<ICollection<CollaboraterEntity>>() { Message = "Problem Occured while retrieving Collabarators", Data = null };
            }
        }
    }
}
