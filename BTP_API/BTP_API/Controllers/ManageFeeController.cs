using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageFeeController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageFeeController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var fees = _context.Fees.Where(p => p.IsActive == true).ToList();
                if (fees.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = fees
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
        public IActionResult GetFeeById(int id)
        {
            try
            {
                var fee = _context.Fees.SingleOrDefault(p => p.Id == id);
                if (fee != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get successfull!",
                        Data = fee
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

        [HttpPost("create")]
        public IActionResult CreateNew(FeeVM feeVM)
        {
            try
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == feeVM.Code);
                if (fee != null)
                {
                    var feeNew = new Fee
                    {
                        Code = feeVM.Code,
                        Name = feeVM.Name,
                        Price = feeVM.Price,
                        IsActive = true
                    };
                    fee.IsActive = false;
                    _context.Add(feeNew);
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Thêm thành công!",
                        Data = feeNew
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy phí!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpPut("edit/{id}")]
        //public IActionResult EditFee(int id, FeeVM feeVM)
        //{
        //    try
        //    {
        //        var fee = _context.Fees.SingleOrDefault(f => f.Id == id);
        //        if (fee == null)
        //        {
        //            return Ok(new ApiResponse
        //            {
        //                Success = true,
        //                Message = "Không tìm thấy!"
        //            });
        //        }

        //        fee.Name = feeVM.Name;
        //        fee.Price = feeVM.Price;
        //        _context.SaveChanges();

        //        return Ok(new ApiResponse
        //        {
        //            Success = false,
        //            Message = "Sửa thành công!"
        //        });
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}

        //[HttpPut("hide/{id}")]
        //public IActionResult HideFee(int id)
        //{
        //    try
        //    {
        //        var fee = _context.Fees.SingleOrDefault(p => p.Id == id);
        //        if (fee == null)
        //        {
        //            return Ok(new ApiResponse
        //            {
        //                Success = true,
        //                Message = "Không tìm thấy!"
        //            });
        //        }

        //        if(fee.Flag == false)
        //        {
        //            fee.Flag = true;
        //            _context.SaveChanges();
        //            return Ok(new ApiResponse
        //            {
        //                Success = false,
        //                Message = "Hiện thành công!"
        //            });
        //        }
        //        fee.Flag = false;
        //        _context.SaveChanges();
        //        return Ok(new ApiResponse
        //        {
        //            Success = false,
        //            Message = "Ẩn thành công!"
        //        });

        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}
