using Data.Interfaces;
using Data.Repositories;
using System;
using System.Threading.Tasks;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private ILibraryDbContext _libraryDbContext;

        private IBookRepository _bookRepository;
        private ICardRepository _cardRepository;
        private IHistoryRepository _historyRepository;
        private IReaderRepository _readerRepository;

        public UnitOfWork(ILibraryDbContext libraryDBContext)
        {
            _libraryDbContext = libraryDBContext;
        }
        public IBookRepository BookRepository 
        {
            get 
            { 
                if(_bookRepository==null)
                {
                    _bookRepository= new BookRepository(_libraryDbContext);
                }
                return _bookRepository;
            }
        }
        public ICardRepository CardRepository
        {
            get
            {
                if (_cardRepository == null)
                {
                    _cardRepository = new CardRepository(_libraryDbContext);
                }
                return _cardRepository;
            }
        }
        public IHistoryRepository HistoryRepository
        {
            get
            {
                if (_historyRepository == null)
                {
                    _historyRepository = new HistoryRepository(_libraryDbContext);
                }
                return _historyRepository;
            }
        }
        public IReaderRepository ReaderRepository
        {
            get
            {
                if (_readerRepository == null)
                {
                    _readerRepository = new ReaderRepository(_libraryDbContext);
                }
                return _readerRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            _libraryDbContext.SaveChanges();
            return await (Task<int>)Task.CompletedTask;
        }
    }
}