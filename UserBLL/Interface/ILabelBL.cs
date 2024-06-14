using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserBLL.Interface
{
    public interface ILabelBL
    {
        IEnumerable<LabelEntity> GetAllLabels();
        LabelEntity GetLabelById(int id);
        void CreateLabel(LabelModel label);
        LabelEntity UpdateLabel(LabelModel label, int id);
        LabelEntity DeleteLabel(int id);
    }

}
