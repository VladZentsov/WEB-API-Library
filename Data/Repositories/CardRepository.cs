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
    public class CardRepository : ICardRepository
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly DbSet<Card> _cards;
        private readonly DbSet<Reader> _readers;
        private readonly DbSet<History> _histories;

        public CardRepository(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            _cards = _dbContext.Set<Card>();
            _readers = _dbContext.Set<Reader>();
            _histories = _dbContext.Set<History>();
        }
        public Task AddAsync(Card entity)
        {
            _cards.Add(entity);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Card entity)
        {
            var result = entity != null;
            if (result)
            {
                _cards.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
            _dbContext.SaveChanges();
        }

        public IQueryable<Card> FindAll()
        {
            return _cards.AsQueryable();
        }

        public IQueryable<Card> FindAllWithDetails()
        {
            return AddEntitiesToCard(FindAll());
        }

        public async Task<Card> GetByIdAsync(int id)
        {
            return FindAll().FirstOrDefault(x => x.Id == id);
        }

        public async Task<Card> GetByIdWithDetailsAsync(int id)
        {
            return AddEntitiesToCard(FindAll().Where(a=>a.Id==id)).FirstOrDefault();
        }

        public void Update(Card entity)
        {
            throw new NotImplementedException();
        }
        private IQueryable<Card> AddEntitiesToCard(IQueryable<Card> cards)
        {
            foreach (var card in cards)
            {
                card.Reader = _readers.Find(card.ReaderId);
                card.Books = _histories.AsQueryable().Where(a => a.CardId == card.Id).ToList();
            }
            return cards;
        }
    }
}
