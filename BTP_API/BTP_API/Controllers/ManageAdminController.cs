using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageAdminController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageAdminController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public IActionResult GetAllAdmin()
        {
            try
            {
                var admins = _context.Users.Where(b => b.RoleId == 2 && b.IsActive == true);
                if (admins.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = admins
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

        [HttpPut("remove/{id}")]
        [Authorize(Roles = "1")]
        public IActionResult RemoveAdmin(int id)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Id == id);
                if (user == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng!"
                    });
                }
                if (user.RoleId == 1)
                {
                    user.RoleId = 2;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Hủy quyền admin thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hủy quyền admin không thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search/{search}")]
        public IActionResult SearchAdmin(string search)
        {
            try
            {
                var user = _context.Users.Where(b => b.RoleId == 2 && b.Email.Contains(search) || b.RoleId == 2 && b.Phone.Contains(search));
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
    }
}
