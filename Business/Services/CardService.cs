using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CardService : ICardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(CardModel model)
        {
            await _unitOfWork.CardRepository.AddAsync(_mapper.Map<CardModel, Card>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await _unitOfWork.CardRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<CardModel> GetAll()
        {
            IEnumerable<Card> allCards = _unitOfWork.CardRepository.FindAllWithDetails();
            return _mapper.Map<IEnumerable<Card>, IEnumerable<CardModel>>(allCards);
        }

        public IEnumerable<BookModel> GetBooksByCardId(int cardId)
        {
            IEnumerable<Book> allBooks = _unitOfWork.BookRepository.FindAllWithDetails();
            var c = allBooks.ToList();
            var book = c[0];
            BookModel bookm = _mapper.Map<Book, BookModel>(book);
            return _mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(allBooks.ToList());
        }

        public async Task<CardModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Card, CardModel>(await _unitOfWork.CardRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task HandOverBookAsync(int cartId, int bookId)
        {
            var history = _unitOfWork.HistoryRepository.FindAll()
                .Where(h => h.CardId == cartId && h.BookId == bookId).Where(h=>h.ReturnDate==null||h.ReturnDate==new DateTime())
                .FirstOrDefault();
            if (history == null)
                throw new LibraryException("book is already returned");
            history.ReturnDate = DateTime.Now;
            _unitOfWork.HistoryRepository.Update(history);
            await _unitOfWork.SaveAsync();
        }

        public async Task TakeBookAsync(int cartId, int bookId)
        {
            //var p = _unitOfWork.HistoryRepository.FindAll();
            var book =await _unitOfWork.BookRepository.GetByIdAsync(bookId) ?? throw new LibraryException("There is no book with such id");
            var card = await _unitOfWork.CardRepository.GetByIdAsync(cartId) ?? throw new LibraryException("There is no card with such id");
            var histories= _unitOfWork.HistoryRepository.FindAll();
            foreach (var historyItem in histories)  
            {
                if (historyItem.BookId == bookId || historyItem.ReturnDate == null)
                    throw new LibraryException("book has not been returned yet");
            }

            
            History history=new History() { BookId = bookId, CardId=cartId, TakeDate=DateTime.Now, Id=cartId , Book=book, Card=card};
            if(book.Cards==null)
                book.Cards=new List<History>();
            book.Cards.Add(history);
            if(card.Books==null)
                card.Books=new List<History>();
            card.Books.Add(history);
            await _unitOfWork.HistoryRepository.AddAsync(history);
        }

        public async Task UpdateAsync(CardModel model)
        {
            _unitOfWork.CardRepository.Update(_mapper.Map<CardModel, Card>(model));
            await _unitOfWork.SaveAsync();
        }
    }
}
