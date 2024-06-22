using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Exceptions;
using UserRLL.Interface;
using UserRLL.Services;

namespace UserBLL.Service
{
    public class NoteBL(INoteRL note) : INoteBL
    {
        private readonly INoteRL _note = note;

        public NoteEntity CreateNote(NoteModel node,int UserId)
        {
            try
            {
                return _note.CreateNote(node, UserId);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public NoteEntity DeleteNote(int id, int UserId)
        {   
            try
            {
                return _note.DeleteNote(id, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity GetNote(int id,int UserId)
        {
            try
            {
                return _note.GetNoteById(id, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IList<NoteEntity> GetNotes(int UserId)
        {
            
            try
            {
                return _note.GetNotes(UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity UpdateNote(int id, NoteModel note, int UserId)
        {
            try
            {
                return _note.UpdateNote(id, note, UserId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity ArchiveNote(int id, int Userid)
        {
            try
            {
                return _note.ArchiveNote(id, Userid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteEntity TrashNote(int id, int userId)
        {
            try
            {
                return _note.TrashNote(id, userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteEntity> GetAllTrashedNotes(int userId)
        {
            try
            {
                return _note.GetAllTrashNotes(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ICollection<NoteEntity> GetAllArchievedNotes(int userId)
        {
            try
            {
                return _note.GetAllArchieveNotes(userId);   
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
