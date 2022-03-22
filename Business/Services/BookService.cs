using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Business.Validation;
using Data.


namespace Business.Services
{
    public class BookService : IBookService
    {
        private readonly Data.IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(BookModel model)
        {
            Validation(model);
            await _unitOfWork.BookRepository.AddAsync(_mapper.Map<BookModel, Book>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await _unitOfWork.BookRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<BookModel> GetAll()
        {
            IEnumerable <Book> allBooks=_unitOfWork.BookRepository.FindAll();
            return _mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(allBooks);
        }

        public IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch)
        {
            IEnumerable<Book> allBooks = _unitOfWork.BookRepository.FindAllWithDetails();
            if (filterSearch.Author==null||filterSearch.Year==0)
                return _mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(allBooks.Where(a => a.Author == filterSearch.Author || a.Year == filterSearch.Year));
            return _mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(allBooks.Where(a => a.Author == filterSearch.Author && a.Year == filterSearch.Year));
        }

        public async Task<BookModel> GetByIdAsync(int id)
        {
            return _mapper.Map < Book, BookModel> (await _unitOfWork.BookRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task UpdateAsync(BookModel model)
        {
            Validation(model);
            _unitOfWork.BookRepository.Update(_mapper.Map<BookModel, Book>(model));
            await _unitOfWork.SaveAsync();

        }
        public void Validation(BookModel book)
        {
            if (book == null)
                throw new LibraryException("BookModel cannot be null");
            else if (book.Author == null || book.Author == "")
                throw new LibraryException("Author must exist");
            else if (book.Title == null || book.Title == "")
                throw new LibraryException("Title must exist");
            else if (book.Year > DateTime.Now.Year)
                throw new LibraryException("Year out of range");
        }
    }
}
