using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WebApi.Filters;

namespace WebApi.Controllers
{
    ////[Produces("application/json")]
    ////[Route("api/[controller]")]
    //[Route("[controller]")]
    //[ApiController]
    //public class BooksController : ControllerBase
    //{
    //    private readonly IBookService _bookService;

    //    public BooksController(IBookService bookService)
    //    {
    //        _bookService = bookService;
    //    }
    //    [HttpGet]
    //    public ActionResult<int> Get()
    //    {

    //        return 12;
    //    }

    //    //Inject book service via constructor

    //    //GET: /api/books/?Author=Jon%20Snow&Year=1996
    //    [HttpGet]
    //    public ActionResult<IEnumerable<BookModel>> GetByFilter([FromQuery] FilterSearchModel model)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //GET: /api/books/1
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<IEnumerable<BookModel>>> GetById(int id)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //POST: /api/books/
    //    [HttpPost]
    //    public async Task<ActionResult> Add([FromBody] BookModel bookModel)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //PUT: /api/books/
    //    [HttpPut]
    //    public async Task<ActionResult> Update(BookModel bookModel)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //DELETE: /api/books/1
    //    [HttpDelete("{id}")]
    //    public async Task<ActionResult> Delete(int id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}



    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ModelStateActionFilter]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Route("books/GetNumber")]
        [HttpPost]
        public ActionResult<string> GetNumber([FromBody] BookModel s)
        {
            var newb = new BookModel() { Author = "123", Year = 123, Id=1, Title="Ddd" };
            var json = JsonConvert.SerializeObject(newb);
            return "ww";
        }

        //GET: /api/books/?Author=Jon%20Snow&Year=1996
        [HttpGet]
        public ActionResult<IEnumerable<BookModel>> GetByFilter([FromQuery] FilterSearchModel model)
        {
            if ((model.Author == null || model.Author == "") && model.Year == 0)
                return Content(JsonConvert.SerializeObject(_bookService.GetAll()));
            return Content(JsonConvert.SerializeObject(_bookService.GetByFilter(model)));
        }

        //GET: /api/books/1
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetById(int id)
        {
            return Content(JsonConvert.SerializeObject(await _bookService.GetByIdAsync(id)));
        }

        //POST: /api/books/
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BookModel bookModel)
        {
            bookModel.Id = 100;
            await _bookService.AddAsync(bookModel);
            return Content(JsonConvert.SerializeObject(bookModel));
        }
        //PUT: /api/books/
        [HttpPut]
        public async Task<ActionResult> Update(BookModel bookModel)
        {
            await _bookService.UpdateAsync(bookModel);
            return Ok();
        }

        //DELETE: /api/books/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _bookService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}