using BTP_API.Helpers;
using BTP_API.Models;
using BTP_API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageTransactionController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageTransactionController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet("exchange/all")]
        public IActionResult GetAllTransactionEx()
        {
            try
            {
                var exchanges = _context.Exchanges.ToList();
                if (exchanges.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = exchanges
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

        [HttpGet("exchange/{id}")]
        public IActionResult GetTransactionExDetail(int id)
        {
            try
            {
                var exchanges = _context.ExchangeDetails.Where(b => b.ExchangeId == id).ToList();
                if (exchanges.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = exchanges
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

        [HttpGet("exchange/{id}/bill")]
        public IActionResult GetTransactionExBill(int id)
        {
            try
            {
                var exchangeBills = _context.ExchangeBills.Where(b => b.ExchangeId == id).ToList();
                if (exchangeBills.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = exchangeBills
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

        [HttpPut("exchange/update-status/{id}")]
        public IActionResult UpdateStatusTransactionExchange(int id, Status status)
        {
            try
            {
                var exchange = _context.Exchanges.SingleOrDefault(b => b.Id == id);
                if (exchange != null)
                {
                    exchange.Status = status.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Cập nhật trạng thái thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy giao dịch!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("exchange-detail/update-status/{id}")]
        public IActionResult UpdateStatusTransactionDetail(int id, ExchangeDetailVM listOfExchangeDetailVM)
        {
            try
            {
                var exchange = _context.ExchangeDetails.SingleOrDefault(b => b.Id == id);
                if (exchange != null)
                {
                    exchange.BeforeStatusBook1 = listOfExchangeDetailVM.BeforeStatusBook1;
                    exchange.AfterStatusBook1 = listOfExchangeDetailVM.AfterStatusBook1;
                    exchange.StorageStatusBook1 = listOfExchangeDetailVM.StorageStatusBook1;
                    exchange.BeforeStatusBook2 = listOfExchangeDetailVM.BeforeStatusBook2;
                    exchange.AfterStatusBook2 = listOfExchangeDetailVM.AfterStatusBook2;
                    exchange.StorageStatusBook2 = listOfExchangeDetailVM.StorageStatusBook2;
                    exchange.Status = listOfExchangeDetailVM.Status;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Cập nhật trạng thái thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy giao dịch!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("rent/all")]
        public IActionResult GetAllTransactionRent()
        {
            try
            {
                var rents = _context.Rents.ToList();
                if (rents.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = rents
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

        [HttpGet("rent/{id}")]
        public IActionResult GetTransactionRentDetail(int id)
        {
            try
            {
                var rents = _context.RentDetails.Where(b => b.RentId == id).ToList();
                if (rents.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = rents
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

        [HttpGet("rent/{id}/bill")]
        public IActionResult GetTransactionRentBill(int id)
        {
            try
            {
                var rentBills = _context.RentBills.Where(b => b.RentId == id).ToList();
                if (rentBills.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = rentBills
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

        [HttpPut("rent/update-status/{id}")]
        public IActionResult UpdateStatusTransactionRent(int id, Status status)
        {
            try
            {
                var rent = _context.Rents.SingleOrDefault(b => b.Id == id);
                if (rent != null)
                {
                    rent.Status = status.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Cập nhật trạng thái thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy giao dịch!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("rent-detail/update-status/{id}")]
        public IActionResult UpdateStatusTransactionRentDetail(int id, RentDetailVM listOfrentDetailVM)
        {
            try
            {
                var rent = _context.RentDetails.SingleOrDefault(b => b.Id == id);
                if (rent != null)
                {
                    rent.BeforeStatusBook = listOfrentDetailVM.BeforeStatusBook;
                    rent.AfterStatusBook = listOfrentDetailVM.AfterStatusBook;
                    rent.StorageStatusBook = listOfrentDetailVM.StorageStatusBook;
                    rent.Status = listOfrentDetailVM.Status;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Cập nhật trạng thái thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy giao dịch!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
