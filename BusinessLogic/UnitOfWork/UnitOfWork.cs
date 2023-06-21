
using System;
using System.Threading.Tasks;
using BusinesssLogic.Repository.GenericRepository;
using Domains.Interfaces.IGenericRepository;
using Domains.Interfaces.IUnitOfWork;
using Domains.Models;
using ERP_BusinessLogic.Context;

namespace BusinessLogic.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Author> Author { get; private set; }
        public IGenericRepository<News> News{ get; private set; }

        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Author = new GenericRepository<Author>(_context);
            News = new GenericRepository<News>(_context);

        }

        public void Dispose()
        {   
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

    }
}