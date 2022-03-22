using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(p => p.CardsIds, c => c.MapFrom(card => card.Cards.Select(x => x.CardId)))
                .ReverseMap();
            CreateMap<BookModel, Book>()
                .ForMember(p => p.Cards, c => c.MapFrom(card => card.CardsIds))
                .ReverseMap();

            CreateMap<Card, CardModel>()
                .ForMember(p => p.BooksIds, c => c.MapFrom(card => card.Books.Select(x=>x.BookId)))
                .ReverseMap();
            CreateMap<CardModel, Card>()
                .ForMember(p => p.Books, c => c.MapFrom(card => card.BooksIds))
                .ReverseMap();

            CreateMap<(Reader, ReaderProfile),ReaderModel>()
               .ForMember(p => p.Id, r => r.MapFrom(reader => reader.Item1.Id))
               .ForMember(p => p.Name, r => r.MapFrom(reader => reader.Item1.Name))
               .ForMember(p => p.Email, r => r.MapFrom(reader => reader.Item1.Email))
               .ForMember(p => p.Phone, r => r.MapFrom(reader => reader.Item2.Phone))
               .ForMember(p => p.Address, r => r.MapFrom(reader => reader.Item2.Address))
               .ForMember(p => p.CardsIds, r => r.MapFrom(reader => reader.Item1.Cards.Select(x=>x.Id)))
               .ReverseMap();

            CreateMap<(Reader, int), ReaderActivityModel>()
               .ForMember(p => p.ReaderId, c => c.MapFrom(card => card.Item1.Id))
               .ForMember(p => p.ReaderName, c => c.MapFrom(card => card.Item1.Name))
               .ForMember(p => p.BooksCount, c => c.MapFrom(card => card.Item2))
               .ReverseMap();

            //CreateMap< ReaderModel, (Reader, ReaderProfile)>()
            //    .ForMember(p=>p.Item1.Id, r => r.MapFrom(reader=>reader.Id))
            //    .ForMember(p => p.Item1., r => r.MapFrom(reader => reader.Id))
            //    .ReverseMap();

            //TODO: Create mapping for card and card model

            //TODO: Create mapping that combines Reader and ReaderProfile into ReaderModel
            //Before doing reader mapping, learn more about projection in AutoMapper.
            //https://docs.automapper.org/en/stable/Projection.html
            //https://www.infoworld.com/article/3192900/how-to-work-with-automapper-in-csharp.html
        }
    }
}