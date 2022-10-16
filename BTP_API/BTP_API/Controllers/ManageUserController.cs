using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUserController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageUserController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            try
            {
                var users = _context.Users.Where(b => b.RoleId == 3);
                if (users.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = users
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

        [HttpGet("ban-list")]
        public IActionResult GetAllUserBan()
        {
            try
            {
                var users = _context.Users.Where(b => b.RoleId == 3 && b.IsActive == false);
                if (users.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = users
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

        [HttpGet("active-list")]
        public IActionResult GetAllUserActive()
        {

            try
            {
                var users = _context.Users.Where(b => b.RoleId == 3 && b.IsActive == true);
                if (users.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = users
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
        public IActionResult GetUserByID(int id)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(b => b.Id == id && b.RoleId == 3);
                if (user != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = user
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

        [HttpGet("search/{search}")]
        public IActionResult SearchUser(string search)
        {
            try
            {
                var user = _context.Users.Where(b => b.RoleId == 3 && b.Email.Contains(search) || b.RoleId == 3 && b.Phone.Contains(search));
                if (user.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = user
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

        [HttpPut("ban/{id}")]
        public IActionResult BanAcc(int id)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Id == id && x.IsActive == true);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng!"
                    });
                }
                user.IsActive = false;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Khóa tài khoản thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("unban/{id}")]
        public IActionResult UnbanAcc(int id)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Id == id && x.IsActive == false);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng!"
                    });
                }
                user.IsActive = true;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Kích hoạt tài khoản thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("authority/{id}")]
        public IActionResult AuthorityAdmin(int id)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Id == id && x.IsActive == true);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng!"
                    });
                }
                if (user.RoleId == 3)
                {
                    user.RoleId = 2;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Ủy quyền admin thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Ủy quyền admin không thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
