﻿using BusinessLogic.Data;
using Core.Entities;
using Core.Entities.Specifications;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ClaseBase
    {
        private readonly MarketDbContext _context;
        public GenericRepository(MarketDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

    

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
           return await ApplySpecification(spec).FirstOrDefaultAsync() ;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec) {
           return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(),spec);

        }
    }
}