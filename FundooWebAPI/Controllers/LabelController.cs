using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserBLL.Interface;
using UserBLL.Service;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Interface;
using UserRLL.Services;

namespace FundoWebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "CrudScheme")]
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelBL _labelService;

        public LabelsController(ILabelBL labelService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        public ResponseModel<IEnumerable<LabelEntity>> GetAllLabels()
        {
            try
            {
                var labels = _labelService.GetAllLabels();
                return new ResponseModel<IEnumerable<LabelEntity>>() { Data = labels, Message = "Fetched all Labels"  };

            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<LabelEntity>>() { Data = null, Message = ex.Message, Status = false };
            }
        }

        [HttpGet("{id}")]
        public ResponseModel<LabelEntity> GetLabelById(int id)
        {
            try
            {
                var label = _labelService.GetLabelById(id);
                return new ResponseModel<LabelEntity>() { Message = "Label Retrieved", Data = label };

            }
            catch (Exception ex)
            {
                return new ResponseModel<LabelEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }

        [HttpPost]
        public ResponseModel<string> CreateLabel([FromBody] LabelModel label)
        {
            try
            {
                _labelService.CreateLabel(label);
                return new ResponseModel<string> { Message = "Label Added" , Data = string.Empty };
            }
            catch (Exception ex)
            {
                return new ResponseModel<string> { Status = false, Message = ex.Message, Data = string.Empty };
            }
        }

        [HttpPut("{id}")]
        public ResponseModel<LabelEntity> UpdateLabel(int id, [FromBody] LabelModel label)
        {
            try
            {
                var updatedNote = _labelService.UpdateLabel(label, id);
                return new ResponseModel<LabelEntity>() { Message = "label Updated", Data = updatedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<LabelEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }

        [HttpDelete("{id}")]
        public ResponseModel<LabelEntity> DeleteLabel(int id)
        {
            try
            {
                var deletedNote = _labelService.DeleteLabel(id);    
                return new ResponseModel<LabelEntity>() { Message = "label Deleted", Data = deletedNote };
            }
            catch (Exception ex)
            {
                return new ResponseModel<LabelEntity>() { Status = false, Message = ex.Message, Data = null };
            }
        }
    }

}
