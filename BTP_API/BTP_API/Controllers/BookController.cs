using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BTPContext _context;
        private readonly IWebHostEnvironment _environment;

        public BookController(BTPContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("all")]
        public IActionResult GetAllBook()
        {
            try
            {
                var books = _context.Books.Include(b => b.User).Include(b => b.Category).Where(b => b.Status == StatusRequest.Approved.ToString() && b.IsReady == true).ToList();
                if (books.Count == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Danh sách trống!",
                        Data = books
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "GetAllBook successfull!",
                    Data = books
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            try
            {
                var book = _context.Books.Include(b => b.User).Include(b => b.Category).SingleOrDefault(b => b.Id == id && b.Status == StatusRequest.Approved.ToString() && b.IsReady == true);
                if (book != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = book
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Not found!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("category{id}")]
        public IActionResult GetBookByCategory(int id)
        {
            try
            {
                var data = _context.Books.Include(b => b.User).Include(b => b.Category).Where(b => b.CategoryId == id && b.IsReady == true && b.Status == StatusRequest.Approved.ToString());
                if (data != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = data
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Not found!",
                        Data = null
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("user{id}")]
        public IActionResult GetBookByUserId(int id)
        {
            try
            {
                var data = _context.Books.Include(b => b.User).Where(b => b.UserId == id && b.Status == StatusRequest.Approved.ToString() && b.IsReady == true);
                if (data != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = data
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Not found!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("search-by-title/{search}")]
        public IActionResult SearchBookByTitle(string search)
        {
            try
            {
                var books = _context.Books.Include(b => b.User).Where(b => b.Title.Contains(search) && b.IsReady == true && b.Status == StatusRequest.Approved.ToString());
                if (books.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = books
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy kết quả tương ứng!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("create")]
        public IActionResult CreateBook([FromForm] BookVM bookVM)
        {
            try
            {
                int userId = GetUserId();
                if (userId == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Vui lòng đăng nhập!"
                    });
                }

                string fileImageName = UploadBookFile(bookVM);

                var book = new Book
                {
                    UserId = userId,
                    CategoryId = bookVM.CategoryId,
                    Title = bookVM.Title,
                    Description = bookVM.Description,
                    Author = bookVM.Author,
                    Publisher = bookVM.Publisher,
                    Year = bookVM.Year,
                    Language = bookVM.Language,
                    NumberOfPages = bookVM.NumberOfPages,
                    Weight = bookVM.Weight,
                    CoverPrice = bookVM.CoverPrice,
                    DepositPrice = bookVM.DepositPrice,
                    StatusBook = bookVM.StatusBook,
                    Image = fileImageName,
                    PostedDate = DateOnly.FromDateTime(DateTime.Today),
                    IsExchange = bookVM.IsExchange,
                    IsRent = bookVM.IsRent,
                    RentFee = bookVM.RentFee,
                    NumberOfDays = (int)(Math.Ceiling((double)bookVM.NumberOfPages / 100) * 5),
                    IsReady = true,
                    IsTrade = false,
                    Status = StatusRequest.Waiting.ToString(),
                };
                _context.Add(book);
                _context.SaveChanges();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Create successfull!",
                    Data = book
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{id}/feedback")]
        public IActionResult Feedback(int bookId, FeedbackVM feedbackVM)
        {
            try
            {
                int userId = GetUserId();
                if (userId == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Vui lòng đăng nhập!"
                    });
                }

                var feedback = new Feedback
                {
                    BookId = bookId,
                    UserId = userId,
                    Content = feedbackVM.Content,
                    CreatedDate = DateTime.Now
                };
                _context.Add(feedback);
                _context.SaveChanges();

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Feedback successfull!",
                    Data = feedback
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("edit/{id}")]
        public IActionResult UpdateBookById(int id, BookVM bookVM)
        {
            try
            {
                var _book = _context.Books.SingleOrDefault(b => b.Id == id);
                if (_book != null)
                {
                    _book.CategoryId = bookVM.CategoryId;
                    _book.Title = bookVM.Title;
                    _book.Description = bookVM.Description;
                    _book.Author = bookVM.Author;
                    _book.Publisher = bookVM.Publisher;
                    _book.Year = bookVM.Year;
                    _book.Language = bookVM.Language;
                    _book.NumberOfPages = bookVM.NumberOfPages;
                    _book.Weight = bookVM.Weight;
                    _book.CoverPrice = bookVM.CoverPrice;
                    _book.DepositPrice = bookVM.DepositPrice;
                    _book.StatusBook = bookVM.StatusBook;
                    _book.IsExchange = bookVM.IsExchange;
                    _book.IsRent = bookVM.IsRent;
                    _book.RentFee = bookVM.RentFee;
                    _book.IsReady = true;
                    _book.Status = StatusRequest.Waiting.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Update successfull!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Update failed!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("hide/{id}")]
        public IActionResult HideBook(int id)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(u => u.Id == id && u.IsReady == true);
                if (book == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không timg thấy!"
                    });
                }

                if (book.IsReady)
                {
                    book.IsReady = false;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Ẩn thành công!"
                    });
                }

                book.IsReady = true;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hiện sách thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("show/{id}")]
        public IActionResult ShowBook(int id)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(u => u.Id == id && u.IsReady == false);
                if (book == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy!"
                    });
                }

                book.IsReady = true;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hiện sách thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string UploadBookFile(BookVM bookVM)
        {
            string fileName;
            if (bookVM != null)
            {
                string uploadDir = Path.Combine(_environment.WebRootPath, "BookImage");
                fileName = bookVM.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    bookVM.Image.CopyTo(fileStream);
                }

                byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                fileName = Convert.ToBase64String(imageArray);
            }
            else
            {
                fileName = "empty";
            }
            return fileName;
        }

        private int GetUserId()
        {
            var cookie = Request.Cookies["accessToken"];
            if (cookie == null)
            {
                return 0;
            }
            var token = new JwtSecurityToken(jwtEncodedString: cookie);
            var userId = token.Claims.FirstOrDefault();
            int id = Int32.Parse(userId.Value);
            return id;
        }
    }
}
