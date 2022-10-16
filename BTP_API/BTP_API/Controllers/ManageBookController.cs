using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageBookController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageBookController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public IActionResult GetAllBook()
        {
            try
            {
                var books = _context.Books.ToList();
                if (books.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
                        Data = books
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Danh sách trống!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("denied")]
        public IActionResult GetAllBookDenied()
        {
            try
            {
                var books = _context.Books.Where(b => b.Status == StatusRequest.Denied.ToString());
                if (books.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
                        Data = books
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Danh sách trống!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("approved")]
        public IActionResult GetAllBookApproved()
        {
            try
            {
                var books = _context.Books.Where(b => b.Status == StatusRequest.Approved.ToString());
                if (books.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
                        Data = books
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Danh sách trống!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("waiting")]
        public IActionResult GetAllBookWaiting()
        {
            try
            {
                var books = _context.Books.Where(b => b.Status == StatusRequest.Waiting.ToString());
                if (books.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
                        Data = books
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Danh sách trống!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBookByID(int id)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(b => b.Id == id);
                if (book != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = book
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Danh sách trống!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("approved/{id}")]
        public IActionResult ApprovedBook(int id)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(b => b.Id == id && b.Status == StatusRequest.Waiting.ToString());
                if (book != null)
                {
                    book.Status = StatusRequest.Approved.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Duyệt thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("denied/{id}")]
        public IActionResult DeniedBook(int id)
        {
            try
            {
                var book = _context.Books.SingleOrDefault(b => b.Id == id && b.Status == StatusRequest.Waiting.ToString());
                if (book != null)
                {
                    book.Status = StatusRequest.Denied.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Duyệt thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
