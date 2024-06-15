using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Entity;
using UserRLL.Services;

namespace UserRLL.Interface
{
    public interface ICollaboratorRL
    {
        public void SaveCollaborator(int noteId,string Collaborator);

        public string DeleteCollaborator(int noteId);

        public ICollection<CollaboraterEntity> GetCollaborators(int noteId);
    }
}
