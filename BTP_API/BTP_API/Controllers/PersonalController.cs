using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly BTPContext _context;
        private readonly IWebHostEnvironment _environment;

        public PersonalController(BTPContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        [HttpGet("can-trade")]
        public IActionResult GetBookCanTrade()
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

                var books = _context.Books.Where(b => b.UserId == userId && b.IsTrade == false && b.IsReady == true);
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
                    return Ok(new ApiResponse
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


        [HttpGet("my-book-list")]
        public IActionResult GetAllBook()
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
                var books = _context.Books.Where(b => b.UserId == userId).ToList();
                if (books.Count != 0)
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
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Danh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("my-approved-book-list")]
        public IActionResult GetBookApproved()
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
                var books = _context.Books.Where(b => b.UserId == userId && b.Status == StatusRequest.Approved.ToString()).ToList();
                if (books.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
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

        [HttpGet("my-denied-book-list")]
        public IActionResult GetBookDenied()
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
                var books = _context.Books.Where(b => b.UserId == userId && b.Status == StatusRequest.Denied.ToString()).ToList();
                if (books.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
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

        [HttpGet("my-waiting-book-list")]
        public IActionResult GetBookWaiting()
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
                var books = _context.Books.Where(b => b.UserId == userId && b.Status == StatusRequest.Waiting.ToString()).ToList();
                if (books.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
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






        [HttpGet("my-post-list")]
        public IActionResult GetAllPost()
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
                var posts = _context.Posts.Where(b => b.UserId == userId).ToList();
                if (posts.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = posts
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

        [HttpGet("my-approved-post-list")]
        public IActionResult GetPostApproved()
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
                var posts = _context.Posts.Where(b => b.UserId == userId && b.Status == StatusRequest.Approved.ToString()).ToList();
                if (posts.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = posts
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

        [HttpGet("my-denied-post-list")]
        public IActionResult GetPostDenied()
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
                var posts = _context.Posts.Where(b => b.UserId == userId && b.Status == StatusRequest.Denied.ToString()).ToList();
                if (posts.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = posts
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

        [HttpGet("my-waiting-post-list")]
        public IActionResult GetPostWaiting()
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
                var posts = _context.Posts.Where(b => b.UserId == userId && b.Status == StatusRequest.Waiting.ToString()).ToList();
                if (posts.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = posts
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


        [HttpGet("my-favorites-book")]
        public IActionResult GetBookByFavorites()
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
                var lists = _context.FavoriteBookLists.Include(f => f.Book).Where(f => f.UserId == userId);
                if (lists.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = lists
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

        [HttpPost("my-favorites-book/add/{bookId}")]
        public IActionResult AddBookByFavorites(int bookId)
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

                var check = _context.FavoriteBookLists.SingleOrDefault(f => f.BookId == bookId && f.UserId == userId);
                if (check != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Đã có trong danh sách yêu thích!"
                    });
                }

                var favoriteBook = new FavoriteBookList
                {
                    BookId = bookId,
                    UserId = userId
                };

                _context.Add(favoriteBook);
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Add successfull!",
                    Data = favoriteBook
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("un-favorites-book/{bookId}")]
        public IActionResult DeleteBookByFavorites(int bookId)
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
                var lists = _context.FavoriteBookLists.SingleOrDefault(f => f.UserId == userId && f.BookId == bookId);
                if (lists != null)
                {
                    _context.Remove(lists);
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Delete successfull!"
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



        [HttpGet("my-favorites-post")]
        public IActionResult GetPostByFavorites()
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
                var lists = _context.FavoritePostLists.Include(f => f.Post).Where(f => f.UserId == userId);
                if (lists.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = lists
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Danh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("my-favorites-post/add/{postId}")]
        public IActionResult AddPostByFavorites(int postId)
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

                var check = _context.FavoritePostLists.SingleOrDefault(p => p.PostId == postId && p.UserId == userId);
                if (check != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Đã tồn tại!"
                    });
                }

                var favoritePost = new FavoritePostList
                {
                    PostId = postId,
                    UserId = userId
                };

                _context.Add(favoritePost);
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Add successfull!",
                    Data = favoritePost
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("un-favorites-post/{postId}")]
        public IActionResult DeletePostByFavorites(int postId)
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
                var post = _context.FavoritePostLists.SingleOrDefault(f => f.UserId == userId && f.PostId == postId);
                if (post != null)
                {
                    _context.Remove(post);
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Delete successfull!"
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Danh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("my-favorites-user")]
        public IActionResult GetUserByFavorites()
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
                var lists = _context.FavoriteUserLists.Include(f => f.FavoriteUser).Where(f => f.UserId == userId);
                if (lists.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = lists
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Danh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("my-favorites-user/add/{userId}")]
        public IActionResult AddUserByFavorites(int userId)
        {
            try
            {
                int _userId = GetUserId();
                if (_userId == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Vui lòng đăng nhập!"
                    });
                }

                var check = _context.FavoriteUserLists.SingleOrDefault(u => u.FavoriteUserId == userId && u.UserId == _userId);
                if (check != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Đã tồn tại!"
                    });
                }

                var favoriteUser = new FavoriteUserList
                {
                    FavoriteUserId = userId,
                    UserId = _userId
                };

                _context.Add(favoriteUser);
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Add successfull!",
                    Data = favoriteUser
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("un-favorites-user/{userId}")]
        public IActionResult DeleteUserByFavorites(int userId)
        {
            try
            {
                int _userId = GetUserId();
                if (_userId == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Vui lòng đăng nhập!"
                    });
                }
                var user = _context.FavoriteUserLists.SingleOrDefault(f => f.UserId == _userId && f.FavoriteUserId == userId);
                if (user != null)
                {
                    _context.Remove(user);
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Delete successfull!"
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Danh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("my-profile")]
        public IActionResult GetInfoUserId()
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
                var data = _context.Users.SingleOrDefault(u => u.Id == userId);
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
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Sanh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("edit-profile")]
        public IActionResult EditInfo([FromForm] UserVM userVM)
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
                var user = _context.Users.SingleOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không thấy dữ liệu!"
                    });
                }

                string fileImageName = UploadBookFile(userVM.Avatar);

                user.Fullname = userVM.Fullname;
                user.Age = userVM.Age;
                user.AddressMain = userVM.AddressMain;
                user.AddressSub1 = userVM.AddressSub1;
                user.AddressSub2 = userVM.AddressSub2;
                user.Avatar = fileImageName;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Cập nhật thông tin cá nhân thành công!",
                    Data = user
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("edit-password")]
        public IActionResult EditPassword(ChangePasswordVM passwordVM)
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
                var user = _context.Users.SingleOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không thấy người dùng!"
                    });
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(passwordVM.OldPassword, user.Password);
                if (!isValid)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Mật khẩu cũ không chính xác!"
                    });
                }

                int costParameter = 12;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordVM.NewPassword, costParameter);

                user.Password = hashedPassword;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đổi mật khẩu thành công!",
                    Data = user
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("request-send")]
        public IActionResult ListOfRequestSend()
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

                var myBooks = _context.Books.Where(b => b.UserId == userId && b.Status == StatusRequest.Approved.ToString()).ToList();

                List<ExchangeRequest> LoREx = new List<ExchangeRequest>();

                foreach (var book in myBooks)
                {
                    var data = _context.ExchangeRequests.Where(r => r.BookOfferId == book.Id).ToList();
                    foreach (var item in data)
                    {
                        LoREx.Add(item);
                    }
                }

                if (LoREx != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = LoREx
                    });
                }
                else
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Sanh sách trống!"
                    });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("request-received/{bookId}")]
        public IActionResult ListOfRequestReceived(int bookId)
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

                var check = _context.Books.Where(b => b.Id == bookId && b.UserId == userId).ToList();
                if (check.Count == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy sách của bạn!"
                    });
                }

                var data = _context.ExchangeRequests.Include(r => r.BookOffer.User).Where(r => r.BookId == bookId).ToList();
                if (data.Count != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
                        Data = data
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách trống!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("my-transaction-exchange-all")]
        public IActionResult MyTransaction()
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

                var transaction = _context.Exchanges.Where(b => b.UserId1 == userId || b.UserId2 == userId).ToList();
                if (transaction.Count == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy giao dịch của bạn!"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Get thành công!",
                    Data = transaction
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("my-transaction-exchange-detail")]
        public IActionResult MyTransactionExDetail(int id)
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

                var transactionDetails = _context.ExchangeDetails.Where(b => b.ExchangeId == id).ToList();
                if (transactionDetails.Count == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy giao dịch của bạn!"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách trống!",
                    Data = transactionDetails
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("my-transaction-exchange-bill")]
        public IActionResult MyTransactionExBill(int id)
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

                var transactionBills = _context.ExchangeBills.SingleOrDefault(b => b.ExchangeId == id && b.UserId == userId);
                if (transactionBills == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy giao dịch của bạn!"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách trống!",
                    Data = transactionBills
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("my-exchange-bill-all")]
        public IActionResult MyExBillAll()
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

                var transactionBills = _context.ExchangeBills.Where(b => b.UserId == userId);
                if (transactionBills.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Trống!"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Get thành công!",
                    Data = transactionBills
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("update-info-shipping")]
        public IActionResult UpdateInfoShipping(ShipInfoVM shippingVM)
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
                var shipInfo = _context.ShipInfos.SingleOrDefault(u => u.UserId == userId);
                if (shipInfo == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không thấy dữ liệu!"
                    });
                }

                shipInfo.SendIsMonday = shippingVM.SendIsMonday;
                shipInfo.SendIsWednesday = shippingVM.SendIsWednesday;
                shipInfo.SendIsFriday = shippingVM.SendIsFriday;
                shipInfo.ReceiveIsMonday = shippingVM.ReceiveIsMonday;
                shipInfo.ReceiveIsWednesday = shippingVM.ReceiveIsWednesday;
                shipInfo.ReceiveIsFriday = shippingVM.ReceiveIsFriday;
                shipInfo.RecallIsMonday = shippingVM.RecallIsMonday;
                shipInfo.RecallIsWednesday = shippingVM.RecallIsWednesday;
                shipInfo.RecallIsFriday = shippingVM.RecallIsFriday;
                shipInfo.RefundIsMonday = shippingVM.RefundIsMonday;
                shipInfo.RefundIsWednesday = shippingVM.RefundIsWednesday;
                shipInfo.RefundIsFriday = shippingVM.RefundIsFriday;
                shipInfo.IsUpdate = true;

                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Cập nhật thông tin vận chuyển thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string UploadBookFile(IFormFile Avatar)
        {
            string fileName;
            if (Avatar != null)
            {
                string uploadDir = Path.Combine(_environment.WebRootPath, "UserImage");
                fileName = Avatar.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Avatar.CopyTo(fileStream);
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
            if (userId == null)
            {
                return 0;
            }
            int id = Int32.Parse(userId.Value);
            return id;
        }
    }
}
