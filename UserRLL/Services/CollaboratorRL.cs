using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Context;
using UserRLL.Entity;
using UserRLL.Exceptions;
using UserRLL.Interface;

namespace UserRLL.Services
{
    public class CollaboratorRL(UserDBContext context) : ICollaboratorRL
    {
        private readonly UserDBContext _dbContext = context;

        public string DeleteCollaborator(int noteId)
        {
            try
            {
                var existingCollaborator = _dbContext.Collaboraters.Find(noteId) ?? throw new UserException($"No collaborator found with Id : {noteId}");  
                _dbContext.Collaboraters.Remove(existingCollaborator);
                _dbContext.SaveChanges();
                return existingCollaborator.CollaboratorEmail;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ICollection<CollaboraterEntity> GetCollaborators(int noteId)
        {
            try
            {
                return _dbContext.Collaboraters.Where(x => x.NoteEntityId == noteId).ToList() ?? throw new UserException($"No Collaborators with noteId : {noteId} found");
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public void SaveCollaborator(int noteId,string collaborator)
        {
            try
            {
                var isSameUser = _dbContext.Notes.Include(x => x.UserEntity).FirstOrDefault(x => x.Id == noteId && x.UserEntity.Email == collaborator) ?? throw new UserException($"Owner cannot be Collaborater");
                var validUser = _dbContext.Users.FirstOrDefault(p => p.Email == collaborator) ?? throw new UserException($"No User Found with email : {collaborator}");
                if (!_dbContext.Collaboraters.Any(x => x.CollaboratorEmail == collaborator))
                {
                    var newCollabarotor = new CollaboraterEntity() { CollaboratorEmail = collaborator, NoteEntityId = noteId };
                    _dbContext.Collaboraters.Add(newCollabarotor);
                    _dbContext.SaveChanges();
                }
                throw new UserException($"{collaborator} already exists");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
