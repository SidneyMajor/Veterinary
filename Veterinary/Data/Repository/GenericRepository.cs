﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Data.Entities;

namespace Veterinary.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(T entity)
        {
            entity.WasDeleted = false;
            entity.UpdatedDate = DateTime.Now;
            entity.CreatedDate = DateTime.Now;
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();
        }


        public async Task DeleteAsync(T entity)
        {
            entity.WasDeleted = true;
            entity.UpdatedDate = DateTime.Now;
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }


        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id && e.WasDeleted == false);
        }


        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking()
                .Where(e => e.WasDeleted == false);
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                 .FirstOrDefaultAsync(e => e.Id == id && e.WasDeleted == false);
        }


        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _context.Set<T>().Update(entity);
            await SaveAllAsync();
        }



        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
