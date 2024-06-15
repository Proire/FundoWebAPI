using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Interface;
using UserRLL.Entity;
using UserRLL.Interface;
using UserRLL.Services;

namespace UserBLL.Service
{
    public class CollaboratorBL(ICollaboratorRL collaboratorRL) :ICollaboratorBL
    {
        private readonly ICollaboratorRL _collaboratorRL = collaboratorRL;

        public void AddCollaborator(int noteId, string Collaborator)
        {
            try
            {
                _collaboratorRL.SaveCollaborator(noteId, Collaborator);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<CollaboraterEntity> GetCollaborators(int noteId)
        {
            try
            {
                return _collaboratorRL.GetCollaborators(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string RemoveCollaborator(int noteId)
        {
            try
            {
                return _collaboratorRL.DeleteCollaborator(noteId); 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
