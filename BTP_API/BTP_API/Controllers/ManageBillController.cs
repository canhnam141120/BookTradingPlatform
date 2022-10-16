using BTP_API.ViewModels;
using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageBillController : ControllerBase
    {
        private readonly BTPContext _context;

        public ManageBillController(BTPContext context)
        {
            _context = context;
        }

        [HttpGet("exchange-bill/all")]
        public IActionResult GetAllExBill()
        {
            try
            {
                var transactionsBills = _context.ExchangeBills.ToList();
                if (transactionsBills.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = transactionsBills
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

        [HttpGet("exchange-bill/{id}")]
        public IActionResult GetExBillDetail(int id)
        {
            try
            {
                var bill = _context.ExchangeBills.SingleOrDefault(b => b.Id == id);
                if (bill != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = bill
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

        [HttpPut("exchange-bill/update/{id}")]
        public IActionResult UpdateStatusTransactionDetail(int id, ExchangeBillVM listOfExchangeBillVM)
        {
            try
            {
                var exchangeBill = _context.ExchangeBills.SingleOrDefault(b => b.Id == id);
                if (exchangeBill != null)
                {
                    exchangeBill.SendDate = listOfExchangeBillVM.SendDate;
                    exchangeBill.ReceiveDate = listOfExchangeBillVM.ReceiveDate;
                    exchangeBill.RecallDate = listOfExchangeBillVM.RecallDate;
                    exchangeBill.RefundDate = listOfExchangeBillVM.RefundDate;
                    exchangeBill.PaidDate = listOfExchangeBillVM.PaidDate;
                    exchangeBill.IsPaid = listOfExchangeBillVM.IsPaid;
                    exchangeBill.Payments = listOfExchangeBillVM.Payments;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Cập nhật trạng thái hóa đơn thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("rent-bill/all")]
        public IActionResult GetAllRentBill()
        {
            try
            {
                var transactionsBills = _context.RentBills.ToList();
                if (transactionsBills.Count() != 0)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = transactionsBills
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

        [HttpGet("rent-bill/{id}")]
        public IActionResult GetRentBillDetail(int id)
        {
            try
            {
                var bill = _context.RentBills.SingleOrDefault(b => b.Id == id);
                if (bill != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Get thành công!",
                        Data = bill
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

        [HttpPut("rent-bill/update/{id}")]
        public IActionResult UpdateStatusTransactionRentDetail(int id, RentBillVM listOfRentBillVM)
        {
            try
            {
                var rentBill = _context.RentBills.SingleOrDefault(b => b.Id == id);
                if (rentBill != null)
                {
                    rentBill.SendDate = listOfRentBillVM.SendDate;
                    rentBill.ReceiveDate = listOfRentBillVM.ReceiveDate;
                    rentBill.RecallDate = listOfRentBillVM.RecallDate;
                    rentBill.RefundDate = listOfRentBillVM.RefundDate;
                    rentBill.PaidDate = listOfRentBillVM.PaidDate;
                    rentBill.IsPaid = listOfRentBillVM.IsPaid;
                    rentBill.Payment = listOfRentBillVM.Payment;
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Cập nhật trạng thái hóa đơn thành công!"
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
