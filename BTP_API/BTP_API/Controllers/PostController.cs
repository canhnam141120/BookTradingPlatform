using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using Org.BouncyCastle.Utilities;
using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BTPContext _context;
        private readonly IWebHostEnvironment _environment;

        public PostController(BTPContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var posts = _context.Posts.Include(p => p.User).Where(p => p.Status == StatusRequest.Approved.ToString() && p.IsHide == false).ToList();
                if (posts.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "GetAll successfull!",
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
        public IActionResult GetPostById(int id)
        {
            try
            {
                var post = _context.Posts.Include(p => p.User).SingleOrDefault(p => p.Id == id && p.Status == StatusRequest.Approved.ToString() && p.IsHide == false);
                if (post != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
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

        [HttpGet("search-by-hashtag/{search}")]
        public IActionResult SearchPostByHashtag(string search)
        {
            try
            {
                var posts = _context.Posts.Include(b => b.User).Where(b => b.Hashtag.Contains(search) && b.Status == StatusRequest.Approved.ToString() && b.IsHide == false);
                if (posts.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Search successfull!",
                        Data = posts
                    });
                }
                else
                {
                    return Ok(new ApiResponse
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
        public IActionResult CreatePost([FromForm] PostVM postVM)
        {
            try
            {
                string fileName = UploadPostFile(postVM);
                int userId = GetUserId();
                if (userId == 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Vui lòng đăng nhập!"
                    });
                }
                var post = new Post
                {
                    UserId = userId,
                    Content = postVM.Content,
                    Image = fileName,
                    Hashtag = postVM.Hashtag,
                    CreatedDate = DateTime.Now,
                    IsHide = false,
                    Status = Status.Waiting.ToString()
                };
                _context.Add(post);
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Bài đăng của bạn đã được gửi cho quản trị viên duyệt!",
                    Data = post
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("post{postID}/comment")]
        public IActionResult CommentPost(int postID, CommentVM commentVM)
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

                var post = new Comment
                {
                    PostId = postID,
                    UserId = userId,
                    Content = commentVM.Content,
                    CreatedDate = DateTime.Now
                };
                _context.Add(post);
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Bình luận thành công!",
                    Data = post
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("hide/{id}")]
        public IActionResult HidePost(int id)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(p => p.Id == id && p.Status == StatusRequest.Approved.ToString() && p.IsHide == false);
                if (post == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Không tìm thấy bài đăng để ẩn!"
                    });
                }
                post.IsHide = true;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Ẩn bài đăng thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("show/{id}")]
        public IActionResult ShowPost(int id)
        {
            try
            {
                var post = _context.Posts.SingleOrDefault(p => p.Id == id && p.Status == StatusRequest.Approved.ToString() && p.IsHide == true);
                if (post == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Không tìm thấy bài đăng để ẩn!"
                    });
                }
                post.IsHide = false;
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Ẩn bài đăng thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private string UploadPostFile(PostVM postVM)
        {
            string fileName;
            if (postVM != null)
            {
                //byte[] bytes = Convert.FromBase64String(postVM.Image.FileName);
                //fileName = Convert.ToBase64String(bytes);

                //using (var fileStream = new FileStream(postVM.Image.FileName, FileMode.OpenOrCreate)) ;
                //byte[] bytes = new byte[postVM.Image.Length];
                //fileName = Convert.ToBase64String(bytes);

                string uploadDir = Path.Combine(_environment.WebRootPath, "PostImage");
                fileName = postVM.Image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    postVM.Image.CopyTo(fileStream);
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
