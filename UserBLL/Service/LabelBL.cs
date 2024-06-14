using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Entity;
using UserRLL.Interface;
using UserBLL.Interface;
using UserModelLayer;

namespace UserBLL.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL _labelRepository;

        public LabelBL(ILabelRL labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public IEnumerable<LabelEntity> GetAllLabels()
        {
            
            try
            {
                return _labelRepository.GetAll();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelEntity GetLabelById(int id)
        {
            
            try
            {
                return _labelRepository.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateLabel(LabelModel label)
        {
            
            try
            {
                _labelRepository.Add(label);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelEntity UpdateLabel(LabelModel label, int id)
        {
            
            try
            {
                return _labelRepository.Update(label, id); 
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LabelEntity DeleteLabel(int id)
        { 
            try
            {
                return _labelRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
