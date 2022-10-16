using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageCategoryController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageCategoryController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var categories = _context.Categories;
                if (categories.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "GetAll successfull!",
                        Data = categories
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách trống"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var category = _context.Categories.SingleOrDefault(c => c.Id == id);
                if (category != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = category
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

        [HttpPost("create")]
        public IActionResult Create(CategoryVM categoryVM)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryVM.Name
                };
                _context.Add(category);
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Create successfull!",
                    Data = category
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("edit/{id}")]
        public IActionResult UpdateLoaiById(int id, CategoryVM categoryVM)
        {
            try
            {
                var _category = _context.Categories.SingleOrDefault(c => c.Id == id);
                if (_category != null)
                {
                    _category.Name = categoryVM.Name;
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
                    Message = "Update failed!",
                    Data = null
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var category = _context.Categories.SingleOrDefault(c => c.Id == id);
                if (category != null)
                {
                    _context.Remove(category);
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Delete successfull!",
                        Data = null
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Delete failed!",
                    Data = null
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

