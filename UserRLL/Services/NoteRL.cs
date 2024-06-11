using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UserModelLayer;
using UserRLL.Context;
using UserRLL.Entity;
using UserRLL.Interface;
using UserRLL.Exceptions;


namespace UserRLL.Services
{
    public class NoteRL : INoteRL
    {
        private readonly UserDBContext _dbContext; 

        public NoteRL(UserDBContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public NoteEntity CreateNote(NoteModel note)
        {
            NoteEntity noteEntity = new NoteEntity() { Title=note.Title,Description=note.Description };
            try
            {
                _dbContext.Notes.Add(noteEntity);
                _dbContext.SaveChanges();
                return noteEntity;
            }
            catch (Exception)
            {
                throw new NoteException($"An error occurred while adding a Note ");
            }
        }

        public NoteEntity DeleteNote(int id)
        {
            try
            {
                var note  = _dbContext.Notes.FirstOrDefault(p => p.Id == id);
                if (note != null)
                {
                    _dbContext.Notes.Remove(note);
                    _dbContext.SaveChanges();
                    return note;
                }
                throw new NoteException($"No Note Found with id : {id}");
            }
            catch (Exception)
            {
                Console.WriteLine($"An error occurred while deleting Note with id : {id}");
                throw;
            }
        }

        public NoteEntity GetNoteById(int id)
        {
            try
            {
                var note = _dbContext.Notes.FirstOrDefault(p => p.Id == id);
                if (note != null)
                    return note;
                throw new NoteException($"No Note Found with id : {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public IList<NoteEntity> GetNotes()
        {
            try
            {
                var notes = _dbContext.Notes.ToList();
                if(notes != null)
                    return notes;
                throw new NoteException($"Empty Notes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all Notes: {ex.Message}");
                throw;
            }
        }

        public NoteEntity UpdateNote(int id, NoteModel note)
        {
            try
            {
                var existingNote = _dbContext.Notes.FirstOrDefault(p => p.Id == id);
                if (existingNote != null)
                {
                    existingNote.Title = note.Title;
                    existingNote.Description = note.Description; 

                    _dbContext.SaveChanges();
                    return existingNote;
                }
                throw new NoteException($"No Note Found with id : {id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating Note with ID : {id}");
                throw;
            }
        }
    }
}
