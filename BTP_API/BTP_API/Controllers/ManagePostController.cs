using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagePostController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManagePostController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public IActionResult GetAllPost()
        {
            try
            {
                var posts = _context.Posts.ToList();
                if (posts.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
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

        [HttpGet("approved")]
        public IActionResult GetPostApproved()
        {
            try
            {
                var posts = _context.Posts.Where(b => b.Status == StatusRequest.Approved.ToString());
                if (posts.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
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

        [HttpGet("denied")]
        public IActionResult GetPostDenied()
        {
            try
            {
                var posts = _context.Posts.Where(b => b.Status == StatusRequest.Denied.ToString());
                if (posts.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
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

        [HttpGet("waiting")]
        public IActionResult GetPostWaiting()
        {
            try
            {
                var posts = _context.Posts.Where(b => b.Status == StatusRequest.Waiting.ToString());
                if (posts.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
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

        [HttpGet("{id}")]
        public IActionResult GetPostByID(int id)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(b => b.Id == id);
                if (post != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get thành công!",
                        Data = post
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
        public IActionResult ApprovedPost(int id)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(b => b.Id == id && b.Status == StatusRequest.Waiting.ToString());
                if (post != null)
                {
                    post.Status = StatusRequest.Approved.ToString();
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
        public IActionResult DeniedPost(int id)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(b => b.Id == id && b.Status == StatusRequest.Waiting.ToString());
                if (post != null)
                {
                    post.Status = StatusRequest.Denied.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Hủy thành công!"
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

        [HttpGet("{id}/comment")]
        public IActionResult GetCommentInPost(int id)
        {
            try
            {
                var comments = _context.Comments.Include(p => p.User).Where(p => p.PostId == id);
                if (comments.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = comments
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

        [HttpDelete("delete-comment/{id}")]
        public IActionResult DeleteComment(int id)
        {
            try
            {
                var comment = _context.Comments.SingleOrDefault(b => b.Id == id);
                if (comment != null)
                {
                    _context.Remove(comment);
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Xóa thành công!"
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
