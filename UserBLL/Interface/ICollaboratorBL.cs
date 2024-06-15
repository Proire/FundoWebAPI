using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Service;
using UserRLL.Entity;

namespace UserBLL.Interface
{
    public interface ICollaboratorBL
    {
        void AddCollaborator(int noteId, string Collaborator);

        string RemoveCollaborator(int noteId);

        ICollection<CollaboraterEntity> GetCollaborators(int noteId);
    }
}
