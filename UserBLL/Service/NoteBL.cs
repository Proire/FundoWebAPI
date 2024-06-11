using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBLL.Interface;
using UserModelLayer;
using UserRLL.Entity;
using UserRLL.Interface;

namespace UserBLL.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL _note;
        public NoteBL(INoteRL note) { 
            this._note = note;
        }
        public NoteEntity CreateNote(NoteModel node)
        {
            return _note.CreateNote(node);
        }

        public NoteEntity DeleteNote(int id)
        {
            return _note.DeleteNote(id);
        }

        public NoteEntity GetNote(int id)
        {
            return _note.GetNoteById(id);
        }

        public IList<NoteEntity> GetNotes()
        {
            return _note.GetNotes();
        }

        public NoteEntity UpdateNote(int id, NoteModel note)
        {
            return _note.UpdateNote(id, note);
        }
    }
}
