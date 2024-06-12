using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Interface;
using UserRLL.Services;

namespace UserBLL.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL _note;
        public NoteBL(INoteRL note) { 
            this._note = note;
        }
        public NoteEntity CreateNote(NoteModel node,int UserId)
        {
            return _note.CreateNote(node,UserId);
        }

        public NoteEntity DeleteNote(int id, int UserId)
        {
            return _note.DeleteNote(id,UserId);
        }

        public NoteEntity GetNote(int id,int UserId)
        {
            return _note.GetNoteById(id, UserId);
        }

        public IList<NoteEntity> GetNotes(int UserId)
        {
            return _note.GetNotes(UserId);
        }

        public NoteEntity UpdateNote(int id, NoteModel note, int UserId)
        {
            return _note.UpdateNote(id, note, UserId);
        }
    }
}
