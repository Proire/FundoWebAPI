using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Entity;

namespace UserBLL.Interface
{
    public interface INoteLabelBL
    {
        string AddNoteToLabel(int labelid, int noteid);
        IEnumerable<NoteEntity> GetNotesForLabel(int labelId);

        IEnumerable<LabelEntity> GetLabelsForNotes(int noteId);

        NoteLabelEntity RemovelabelfromNote(int labelId, int noteId);
    }
}
