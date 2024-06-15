using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Interface;
using UserBLL.Interface;
using UserRLL.Entity;

namespace UserBLL.Service
{
    public class NoteLabelBL(INoteLabelRL notelableRL) : INoteLabelBL
    {
        private readonly INoteLabelRL noteLabelRL = notelableRL;
        public string AddNoteToLabel(int labelid, int noteid)
        {
            try
            {
                return noteLabelRL.AddNoteToLabel(labelid, noteid);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IEnumerable<LabelEntity> GetLabelsForNotes(int noteId)
        {
            try
            {
                return noteLabelRL.GetLabelsForNotes(noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<NoteEntity> GetNotesForLabel(int labelId)
        {
            try
            {
                return noteLabelRL.GetNotesForLabel(labelId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NoteLabelEntity RemovelabelfromNote(int labelId, int noteId)
        {
            try
            {
                return noteLabelRL.RemovelabelfromNote(labelId, noteId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
