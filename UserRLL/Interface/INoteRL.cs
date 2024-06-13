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
        IList<NoteEntity> GetNotes(int UserId);

        NoteEntity GetNoteById(int id, int UserId);

        NoteEntity CreateNote(NoteModel note,int UserId);

        NoteEntity UpdateNote(int id, NoteModel note,int UserId);

        NoteEntity DeleteNote(int id, int UserId);

        NoteEntity ArchiveNote(int id, int userId);

        NoteEntity TrashNote(int id, int userId);
    }
}
