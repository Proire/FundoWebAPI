using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserRLL.Interface
{
    public interface ILabelRL
    {
        IEnumerable<LabelEntity> GetAll();
        LabelEntity GetById(int id);
        void Add(LabelModel label);
        LabelEntity Update(LabelModel label,int id);
        LabelEntity Delete(int id);
    }
}
