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
    public class BookRepository : IBookRepository
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly DbSet<Book> _books;
        private readonly DbSet<History> _hisrories;

        public BookRepository(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
            _books = _dbContext.Set<Book>();
            _hisrories = _dbContext.Set<History>();
        }
        public Task AddAsync(Book entity)
        {
            _books.Add(entity);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Book entity)
        {
            var result = entity != null;
            if (result)
            {
                _books.Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
            _dbContext.SaveChanges();
        }
        public IQueryable<Book> FindAll()
        {
            return _books.AsQueryable();
        }

        public IQueryable<Book> FindAllWithDetails()
        {
            var allBooks = AddHistoryToBook(FindAll());
            allBooks = allBooks.Where(a => a.Cards != null);
            if (allBooks == null|| allBooks.Count() == 0)
            {
                //Have to write my own
                throw new ArgumentNullException();
            }
            else
                return allBooks;
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _books.Where(x =>
               x.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<Book> GetByIdWithDetailsAsync(int id)
        {
            return AddHistoryToBook(FindAll().Where(a=>a.Id == id)).FirstOrDefault();
        }

        public void Update(Book entity)
        {
            _books.Update(entity);
            _dbContext.SaveChanges();
        }

        private IQueryable<Book> AddHistoryToBook(IQueryable<Book> allBooks)
        {
            foreach (var book in allBooks)
            {
                book.Cards = (ICollection<History>)_hisrories.AsQueryable().Where(a => a.BookId == book.Id).ToList();
            }
            return allBooks;
        }
    }
}
