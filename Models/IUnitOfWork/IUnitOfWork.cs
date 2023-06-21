using System;
using System.Threading.Tasks;
using Domains.Interfaces.IGenericRepository;
using Domains.Models;

namespace Domains.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Author> Author { get; }
        IGenericRepository<News> News { get; }
        public Task<int> Save();
    }
}