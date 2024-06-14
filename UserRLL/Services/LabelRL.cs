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
using UserModelLayer;

namespace UserRLL.Services
{
    public class LabelRL(UserDBContext context) : ILabelRL
    {
        private readonly UserDBContext _context = context;

        public IEnumerable<LabelEntity> GetAll()
        {
            try
            {
                var labels = _context.Labels.ToList();
                if(labels.Count == 0)
                {
                    throw new LabelException("No Labels Found");
                }
                return labels;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }

        }

        public LabelEntity GetById(int id)
        {

            try
            {
                var label = _context.Labels.Find(id);
                if (label == null)
                {
                    throw new LabelException($"No Label with id {id} Found");
                }
                return label;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }

        }

        public void Add(LabelModel label)
        {
            LabelEntity labelEntity = new LabelEntity() { LableName = label.LableName };
            try
            {
                _context.Labels.Add(labelEntity);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }
        }

        public LabelEntity Update(LabelModel label,int id)
        {
            try
            {
                var existinglabel = _context.Labels.FirstOrDefault(p => p.Id == id);
                if (existinglabel != null)
                {
                    existinglabel.LableName = label.LableName;
                    _context.SaveChanges();
                    return existinglabel;
                }
                throw new LabelException($"No label Found with id : {id}");
            }
            catch (Exception)
            {
                Console.WriteLine($"An error occurred while updating Note with id : {id}");
                throw;
            }
        }

        public LabelEntity Delete(int id)
        {
            try
            {
                var label = _context.Labels.Find(id);
                if (label != null)
                {
                    _context.Labels.Remove(label);
                    _context.SaveChanges();
                    return label;
                }
                throw new LabelException($"No label Found with id : {id}");
            }
            catch(Exception ie )
            {
                Console.WriteLine (ie.Message);
                throw;
            }
        }
    }

}
