using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Context;
using UserRLL.Entity;
using UserRLL.Interface;
using UserRLL.Exceptions;

namespace UserRLL.Services
{
    public class NoteLabelRL(UserDBContext context) : INoteLabelRL
    {

        private readonly UserDBContext _context = context;
        public string AddNoteToLabel(int labelId, int noteId)
        {
            // fetch label 
            var label = _context.Labels.Include(l => l.NoteLabels).FirstOrDefault(l => l.Id == labelId);
            if (label == null)
            {
                throw new LabelException($"Label with ID {labelId} not found");
            }
            // find notes with id noteid
            var note = _context.Notes.Find(noteId);
            if (note == null)
            {
                throw new NoteException($"Note with ID {noteId} not found");
            }

            // Check if the association already exists
            if (!label.NoteLabels.Any(nl => nl.NoteId == noteId))
            {
                label.NoteLabels.Add(new NoteLabelEntity { NoteId = noteId, LabelId = labelId });
                _context.SaveChanges();
                return $"Label with ID {labelId} is associated with Note ID{noteId}";
            }
            throw new NoteLabelException($"Label with ID {labelId} is not associated with Note ID {noteId}");
        }

        public IEnumerable<LabelEntity> GetLabelsForNotes(int noteId)
        {
            var note = _context.Notes.Include(n => n.NoteLabels).ThenInclude(nl => nl.Label)
                              .FirstOrDefault(n => n.Id == noteId);
            if (note == null)
            {
                throw new NoteException($"Note with ID {noteId} not found");
            }

            var labels = note.NoteLabels.Select(nl => nl.Label).ToList();
            return labels;
        }

        public IEnumerable<NoteEntity> GetNotesForLabel(int labelId)
        {
            // fetches all labels with id labelid and also its associated all notes 
            var label = _context.Labels.Include(l => l.NoteLabels).ThenInclude(nl => nl.Note)
                               .FirstOrDefault(l => l.Id == labelId);
            if (label == null)
            {
                throw new LabelException($"Label with ID {labelId} not found");
            }
            // give out all notes which are there in the associated table query 
            var notes = label.NoteLabels.Select(nl => nl.Note).ToList();
            return notes;
        }

        public LabelEntity RemovelabelfromNote(int labelId, int noteId)
        {
            var label = _context.Labels.Include(l => l.NoteLabels).FirstOrDefault(l => l.Id == labelId);
            if (label == null)
            {
                throw new LabelException($"Label with ID {labelId} not found");
            }

            var noteLabel = label.NoteLabels.FirstOrDefault(nl => nl.NoteId == noteId);
            if (noteLabel == null)
            {
                throw new NoteException($"Note with ID {noteId} not found");
            }

            try
            {
                label.NoteLabels.Remove(noteLabel); // Remove the association
                _context.SaveChanges();
                return _context.Labels.FirstOrDefault(p=>p.Id==noteLabel.LabelId) ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            
        }
    }
}
