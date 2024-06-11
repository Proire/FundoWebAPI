using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Entity;

namespace UserBLL.Interface
{
    public interface INoteBL
    {
        IList<NoteEntity> GetNotes();

        NoteEntity GetNote(int id);

        NoteEntity CreateNote(NoteModel note);

        NoteEntity UpdateNote(int id, NoteModel note);

        NoteEntity DeleteNote(int id);
    }
}
