using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly DbSet<History> _histories;
        private readonly DbSet<Card> _cards;
        private readonly DbSet<Book> _books;

        public HistoryRepository(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            _histories = _dbContext.Set<History>();
            _cards = _dbContext.Set<Card>();
            _books = _dbContext.Set<Book>();
        }
        public Task AddAsync(History entity)
        {
            _histories.Add(entity);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(History entity)
        {
            var result = entity != null;
            if (result)
            {
                _histories.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
            _dbContext.SaveChanges();
        }

        public IQueryable<History> FindAll()
        {
            return _histories.AsQueryable();
        }

        public IQueryable<History> GetAllWithDetails()
        {
            return AddBookaAndCard(FindAll());
        }

        public async Task<History> GetByIdAsync(int id)
        {
            return _histories.Find(id);
        }

        public void Update(History entity)
        {
            _histories.Update(entity);
            _dbContext.SaveChanges();
        }
        private IQueryable<History> AddBookaAndCard(IQueryable<History> histories)
        {
            foreach (var history in histories)
            {
                history.Card = _cards.Find(history.CardId);
                history.Book= _books.Find(history.BookId);
            }
            return histories;
        }
    }
}
