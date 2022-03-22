using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Business.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ReaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task AddAsync(ReaderModel model)
        {
            Validation(model);
            Reader reader= new Reader();
            ReaderProfile readerProfile= new ReaderProfile();

            (reader, readerProfile) = _mapper.Map<ReaderModel, (Reader, ReaderProfile)>(model);

            reader.ReaderProfile= readerProfile;
            await _unitOfWork.ReaderRepository.AddAsync(reader);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            await _unitOfWork.ReaderRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public IEnumerable<ReaderModel> GetAll()
        {
            IEnumerable<Reader> allReaders = _unitOfWork.ReaderRepository.GetAllWithDetails();
            List<(Reader, ReaderProfile)> resultTuple = CreateTuple(allReaders);

            return _mapper.Map<IEnumerable<(Reader, ReaderProfile)>, IEnumerable<ReaderModel>>(resultTuple);
        }

        public async Task<ReaderModel> GetByIdAsync(int id)
        {
            var reader = await _unitOfWork.ReaderRepository.GetByIdWithDetails(id);
            return _mapper.Map<(Reader, ReaderProfile), ReaderModel>((reader, reader.ReaderProfile));
        }

        public IEnumerable<ReaderModel> GetReadersThatDontReturnBooks()
        {
            var readers = _unitOfWork.ReaderRepository.GetAllWithDetails();
            var histories = _unitOfWork.HistoryRepository.FindAll();
            var cards = _unitOfWork.CardRepository.FindAll();

            var result = from reader in readers
                         join card in cards on reader.Id equals card.ReaderId
                         join history in histories on card.Id equals history.CardId
                         where history.ReturnDate == new DateTime() || history.ReturnDate == null
                         select reader;

            List <(Reader, ReaderProfile)> resultTuple = CreateTuple(result);
            return _mapper.Map< IEnumerable<(Reader, ReaderProfile)>, IEnumerable<ReaderModel>>(resultTuple);
        }

        public async Task UpdateAsync(ReaderModel model)
        {
            Validation(model);

            Reader reader = new Reader();
            ReaderProfile readerProfile = new ReaderProfile();

            (reader, readerProfile) = _mapper.Map<ReaderModel, (Reader, ReaderProfile)>(model);
            reader.ReaderProfile = readerProfile;

            _unitOfWork.ReaderRepository.Update(reader);
            await _unitOfWork.SaveAsync();
        }
        private void Validation(ReaderModel readerModel)
        {
            if (readerModel == null)
                throw new LibraryException("readerModel cannot be null");
            else if(readerModel.Name == null||readerModel.Name=="")
                throw new LibraryException("readerModel Name cannot be null");
            else if (readerModel.Email == null || readerModel.Email == "")
                throw new LibraryException("readerModel Email cannot be null");
            else if (readerModel.Phone == null || readerModel.Phone == "")
                throw new LibraryException("readerModel Phone cannot be null");
            else if (readerModel.Address == null || readerModel.Address == "")
                throw new LibraryException("readerModel Address cannot be null");
        }

        private List<(Reader, ReaderProfile)> CreateTuple(IEnumerable<Reader> readers)
        {
            List<(Reader, ReaderProfile)> resultTuple = new List<(Reader, ReaderProfile)>();
            foreach (Reader reader in readers)
            {
                resultTuple.Add((reader, reader.ReaderProfile));
            }
            return resultTuple;
        }
    }
}
