using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<BookModel> GetMostPopularBooks(int bookCount)
        {
            var histories = _unitOfWork.HistoryRepository.GetAllWithDetails();

            var result = from h in histories
                         group h by h.Book.Id into b
                         orderby b.Count() descending
                         select histories.FirstOrDefault(x=>x.BookId==b.Key).Book ;
            var l=result.ToList();

            return _mapper.Map <IEnumerable<Book>, IEnumerable<BookModel>>  (result.Take(bookCount));
        }

        public IEnumerable<ReaderActivityModel> GetReadersWhoTookTheMostBooks(int readersCount, DateTime firstDate, DateTime lastDate)
        {
            var histories = _unitOfWork.HistoryRepository.GetAllWithDetails().Where(h => h.TakeDate >= firstDate && h.TakeDate <= lastDate);

            var cards = histories.Select(x => x.Card);

            var group = from c in cards
                        group c by c.Id into b
                        orderby b.Count() descending
                        select b;

            List<(Reader, int)> resultTuple = new List<(Reader, int)>();

            foreach (var item in group)
            {
                resultTuple.Add((item.FirstOrDefault().Reader, item.Count()));
            }
            return _mapper.Map<IEnumerable<(Reader,int)>, IEnumerable<ReaderActivityModel>>(resultTuple);
        }
    }
}
