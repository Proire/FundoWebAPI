using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserRLL.Interface
{
    public interface INoteRL
    {
        IList<NoteEntity> GetNotes();

        NoteEntity GetNoteById(int id);

        NoteEntity CreateNote(NoteModel note);

        NoteEntity UpdateNote(int id, NoteModel note);

        NoteEntity DeleteNote(int id);
    }
}
