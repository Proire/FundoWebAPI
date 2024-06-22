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

        public NoteEntity CreateNote(NoteModel note,int UserId)
        {
            NoteEntity noteEntity = new() { Title=note.Title,Description=note.Description ,UserEntityId=UserId};
            try
            {
                _dbContext.Notes.Add(noteEntity);
                _dbContext.SaveChanges();
                return noteEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public NoteEntity DeleteNote(int id, int userId)
        {
            try
            {
                var note  = _dbContext.Notes.FirstOrDefault(p => p.Id == id && p.UserEntityId == userId);
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

        public NoteEntity GetNoteById(int id, int UserId)
        {
            try
            {
                var note = _dbContext.Notes.FirstOrDefault(p => p.Id == id && p.UserEntityId==UserId);
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

        public IList<NoteEntity> GetNotes(int UserId)
        {
            try
            {
                var notes = _dbContext.Notes.Where(p => p.UserEntityId == UserId).ToList();
                if(notes != null)
                    return notes;
                throw new NoteException($"Empty Notes");
            }
            catch (Exception )
            {
                Console.WriteLine($"An error occurred while retrieving all Notes");
                throw;
            }
        }

        public NoteEntity UpdateNote(int id, NoteModel note, int UserId)
        {
            try
            {
                var existingNote = _dbContext.Notes.FirstOrDefault(p => p.Id == id && p.UserEntityId == UserId);
                if (existingNote != null)
                {
                    existingNote.Title = note.Title;
                    existingNote.Description = note.Description; 

                    _dbContext.SaveChanges();
                    return existingNote;
                }
                throw new NoteException($"No Note Found with id : {id}");
            }
            catch (Exception )
            {
                Console.WriteLine($"An error occurred while updating Note with ID : {id}");
                throw;
            }
        }

        public NoteEntity ArchiveNote(int id,int userId)
        {
            try
            {
                var existingNote = _dbContext.Notes.FirstOrDefault(p => p.Id == id && p.UserEntityId == userId);
                if (existingNote != null && (existingNote.IsDeleted == false || existingNote.IsTrashed == false))
                {
                    existingNote.IsArchived = !existingNote.IsArchived;
                    _dbContext.SaveChanges();
                    return existingNote;
                }
                else
                    throw new NoteException($"Note with {id} is not Avaliable");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public NoteEntity TrashNote(int id,int userId)
        {
            try
            {
                var existingNote = _dbContext.Notes.FirstOrDefault(p => p.Id == id && p.UserEntityId == userId);
                if (existingNote != null && existingNote.IsDeleted == false)
                {
                    existingNote.IsTrashed = !existingNote.IsTrashed;
                    _dbContext.SaveChanges();
                    return existingNote;
                }
                else
                    throw new NoteException($"Note with {id} is not Available");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ICollection<NoteEntity> GetAllTrashNotes(int userId)
        {
            try
            {
                if (_dbContext.Users.Any(x => x.Id == userId))
                {
                    var notes = _dbContext.Notes.Where(p => p.UserEntityId == userId && p.IsTrashed==true).ToList();
                    if (notes.Count == 0)
                        throw new UserException("Empty Trash");
                    return notes;
                }
                else
                    throw new UserException($"No User with {userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ICollection<NoteEntity> GetAllArchieveNotes(int userId)
        {
            try
            {
                if (_dbContext.Users.Any(x => x.Id == userId))
                {
                    var notes = _dbContext.Notes.Where(p => p.UserEntityId == userId && p.IsArchived==true).ToList();
                    if (notes.Count == 0)
                        throw new UserException("Empty Archive");
                    return notes;
                }
                else
                    throw new UserException($"No User with {userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
