using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Entity;

namespace UserRLL.Interface
{
    public interface INoteLabelRL
    {
        string AddNoteToLabel(int labelId, int noteId);

        IEnumerable<NoteEntity> GetNotesForLabel(int labelId);

        IEnumerable<LabelEntity> GetLabelsForNotes(int noteId);

        NoteLabelEntity RemovelabelfromNote(int labelId, int noteId);
    }
}
