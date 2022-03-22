using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly DbSet<Reader> _readers;
        private readonly DbSet<Card> _cards;
        private readonly DbSet<ReaderProfile> _readerProfiles;

        public ReaderRepository(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            _readers = _dbContext.Set<Reader>();
            _cards= _dbContext.Set<Card>();
            _readerProfiles= _dbContext.Set<ReaderProfile>();
        }
        public Task AddAsync(Reader entity)
        {
            _readers.Add(entity);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Reader entity)
        {
            var result = entity != null;
            if (result)
            {
                _readers.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
            _dbContext.SaveChanges();
        }

        public IQueryable<Reader> FindAll()
        {
            return _readers.AsQueryable();
        }

        public IQueryable<Reader> GetAllWithDetails()
        {
            return AddCardAndProfile(FindAll());
        }

        public Task<Reader> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Reader> GetByIdWithDetails(int id)
        {
            return AddCardAndProfile(FindAll().Where(x => x.Id == id)).FirstOrDefault();
        }

        public void Update(Reader entity)
        {
            throw new NotImplementedException();
        }

        private IQueryable<Reader> AddCardAndProfile(IQueryable<Reader> readers)
        {
            foreach (var reader in readers)
            {
                reader.ReaderProfile = _readerProfiles.Find(reader.Id);
                reader.Cards= _cards.AsQueryable().Where(a=>a.ReaderId== reader.Id).ToList();
            }
            return readers;
        }
    }
}
