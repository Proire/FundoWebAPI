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
        IList<NoteEntity> GetNotes(int userId);

        NoteEntity GetNote(int id,int UserId);

        NoteEntity CreateNote(NoteModel note,int userId);

        NoteEntity UpdateNote(int id, NoteModel note,int UserId);

        NoteEntity DeleteNote(int id,int UserId);
        NoteEntity ArchiveNote(int id, int Userid);

        NoteEntity TrashNote(int id, int userId);

        ICollection<NoteEntity> GetAllTrashedNotes(int userId);

        ICollection<NoteEntity> GetAllArchievedNotes(int userId);
    }
}
