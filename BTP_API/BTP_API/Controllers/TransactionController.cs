using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly BTPContext _context;

        public TransactionController(BTPContext context)
        {
            _context = context;
        }

        [HttpPut("exchange/cancel/{id}")]
        public IActionResult CancelTransactionExchange(int id)
        {
            try
            {
                var exchange = _context.Exchanges.SingleOrDefault(b => b.Id == id);
                if (exchange != null)
                {
                    var detail = _context.ExchangeDetails.Where(b => b.ExchangeId == id).ToList();
                    List<int> bookIds = new List<int>();
                    foreach (var item in detail)
                    {
                        item.Status = Status.Cancel.ToString();
                        bookIds.Add(item.Book1Id);
                        bookIds.Add(item.Book2Id);
                    }

                    foreach (var item in bookIds)
                    {
                        var book = _context.Books.SingleOrDefault(r => r.Id == item);
                        if (book != null)
                        {
                            book.IsTrade = false;
                        }
                    }


                    var bill = _context.ExchangeBills.Where(b => b.ExchangeId == id).ToList();
                    foreach (var item in bill)
                    {
                        _context.Remove(item);
                    }
                    exchange.Status = Status.Cancel.ToString();
                    _context.SaveChanges();
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Hủy giao dịch thành công!"
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

        [HttpPut("exchange-detail/cancel/{id}")]
        public IActionResult CancelTransactionExchangeDetail(int id)
        {
            try
            {
                var exchangeDetail = _context.ExchangeDetails.SingleOrDefault(b => b.Id == id);
                if (exchangeDetail == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy giao dịch!"
                    });
                }

                var book1 = _context.Books.SingleOrDefault(b => b.Id == exchangeDetail.Book1Id);
                var book2 = _context.Books.SingleOrDefault(b => b.Id == exchangeDetail.Book2Id);

                book1.IsTrade = false;
                book2.IsTrade = false;

                double totalWeightBook1 = 0;
                double totalWeightBook2 = 0;
                var listBook = _context.ExchangeDetails.Include(e => e.Book1).Include(e => e.Book2).Where(e => e.ExchangeId == exchangeDetail.ExchangeId).ToList();
                foreach (var item in listBook)
                {
                    totalWeightBook1 += item.Book1.Weight;
                    totalWeightBook2 += item.Book2.Weight;
                }

                var bill1 = _context.ExchangeBills.SingleOrDefault(b => b.ExchangeId == exchangeDetail.ExchangeId && b.UserId == book1.UserId);
                if (bill1 != null)
                {
                    if (listBook.Count == 1)
                    {
                        bill1.TotalBook = 0;
                        bill1.DepositFee = 0;
                        bill1.TotalAmount = 0;
                    }
                    if (listBook.Count > 2)
                    {
                        bill1.TotalBook -= 1;
                        bill1.DepositFee -= book2.DepositPrice;
                        bill1.FeeId1 = feeShipID(totalWeightBook1 - book1.Weight);
                        bill1.FeeId2 = feeServiceID(1);
                        bill1.FeeId3 = feeServiceID(listBook.Count() - 1);
                    }
                    if (listBook.Count == 2)
                    {
                        bill1.TotalBook -= 1;
                        bill1.DepositFee -= book2.DepositPrice;
                        bill1.FeeId1 = feeShipID(totalWeightBook1 - book1.Weight);
                        bill1.FeeId2 = feeServiceID(1);
                        bill1.FeeId3 = null;
                    }
                    bill1.TotalAmount = totalAmountExchange(bill1);
                }

                var bill2 = _context.ExchangeBills.SingleOrDefault(b => b.ExchangeId == exchangeDetail.ExchangeId && b.UserId == book2.UserId);
                if (bill2 != null)
                {
                    if (listBook.Count == 1)
                    {
                        bill2.TotalBook = 0;
                        bill2.DepositFee = 0;
                        bill2.TotalAmount = 0;
                    }
                    if (listBook.Count > 2)
                    {
                        bill2.TotalBook -= 1;
                        bill2.DepositFee -= book1.DepositPrice;
                        bill2.FeeId1 = feeShipID(totalWeightBook2 - book2.Weight);
                        bill2.FeeId2 = feeServiceID(1);
                        bill2.FeeId3 = feeServiceID(listBook.Count() - 1);
                    }
                    if (listBook.Count == 2)
                    {
                        bill2.TotalBook -= 1;
                        bill2.DepositFee -= book1.DepositPrice;
                        bill2.FeeId1 = feeShipID(totalWeightBook2 - book2.Weight);
                        bill2.FeeId2 = feeServiceID(1);
                        bill2.FeeId3 = null;
                    }
                    bill2.TotalAmount = totalAmountExchange(bill2);
                }

                exchangeDetail.Status = Status.Cancel.ToString();

                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hủy chi tiết giao dịch thành công!"
                });

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("rent/cancel/{id}")]
        public IActionResult CancelTransactionRent(int id)
        {
            try
            {
                var rent = _context.Rents.SingleOrDefault(r => r.Id == id);
                if (rent == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy giao dịch!"
                    });
                }

                var rentDetails = _context.RentDetails.Where(r => r.RentId == id).ToList();
                List<int> bookIds = new List<int>();
                foreach (var item in rentDetails)
                {
                    item.Status = Status.Cancel.ToString();
                    bookIds.Add(item.BookId);
                }

                foreach (var item in bookIds)
                {
                    var book = _context.Books.SingleOrDefault(r => r.Id == item);
                    if (book != null)
                    {
                        book.IsTrade = false;
                    }
                }

                var bill = _context.RentBills.Where(b => b.RentId == id).ToList();
                foreach (var item in bill)
                {
                    _context.Remove(item);
                }
                rent.Status = Status.Cancel.ToString();
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hủy giao dịch thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("rent-detail/cancel/{id}")]
        public IActionResult CancelTransactionRentDetail(int id)
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

                var rentDetail = _context.RentDetails.SingleOrDefault(b => b.Id == id);
                if (rentDetail == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy giao dịch!"
                    });
                }

                var book = _context.Books.SingleOrDefault(b => b.Id == rentDetail.BookId);

                book.IsTrade = false;

                double totalWeightBook = 0;
                var listBook = _context.RentDetails.Include(e => e.Book).Where(e => e.RentId == rentDetail.RentId).ToList();
                foreach (var item in listBook)
                {
                    totalWeightBook += item.Book.Weight;
                }

                var billOnwer = _context.RentBills.SingleOrDefault(b => b.RentId == rentDetail.RentId && b.UserId == book.UserId);
                if (billOnwer != null)
                {
                    if (listBook.Count == 1)
                    {
                        billOnwer.TotalBook = 0;
                        billOnwer.DepositFee = 0;
                        billOnwer.TotalAmount = 0;
                    }
                    if (listBook.Count > 2)
                    {
                        billOnwer.TotalBook -= 1;
                        billOnwer.FeeId1 = feeShipID(totalWeightBook - book.Weight);
                        billOnwer.FeeId2 = feeServiceID(1);
                        billOnwer.FeeId3 = feeServiceID(listBook.Count() - 1);
                    }
                    if (listBook.Count == 2)
                    {
                        billOnwer.TotalBook -= 1;
                        billOnwer.FeeId1 = feeShipID(totalWeightBook - book.Weight);
                        billOnwer.FeeId2 = feeServiceID(1);
                        billOnwer.FeeId3 = null;
                    }
                    billOnwer.TotalAmount = totalAmountRent(billOnwer);
                }

                var billRenter = _context.RentBills.SingleOrDefault(b => b.RentId == rentDetail.RentId && b.UserId == userId);
                if (billRenter != null)
                {
                    if (listBook.Count == 1)
                    {
                        billRenter.TotalBook = 0;
                        billRenter.DepositFee = 0;
                        billRenter.RentFee = 0;
                        billRenter.TotalAmount = 0;
                    }
                    if (listBook.Count > 2)
                    {
                        billRenter.TotalBook -= 1;
                        billRenter.DepositFee -= book.DepositPrice;
                        billRenter.RentFee -= book.RentFee;
                        billRenter.FeeId1 = feeShipID(totalWeightBook - book.Weight);
                        billRenter.FeeId2 = feeServiceID(1);
                        billRenter.FeeId3 = feeServiceID(listBook.Count() - 1);
                    }
                    if (listBook.Count == 2)
                    {
                        billRenter.TotalBook -= 1;
                        billRenter.DepositFee -= book.DepositPrice;
                        billRenter.RentFee -= book.RentFee;
                        billRenter.FeeId1 = feeShipID(totalWeightBook - book.Weight);
                        billRenter.FeeId2 = feeServiceID(1);
                        billRenter.FeeId3 = null;
                    }
                    billRenter.TotalAmount = totalAmountRent(billRenter);
                }

                rentDetail.Status = Status.Cancel.ToString();

                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Hủy chi tiết giao dịch thành công!"
                });

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private int feeShipID(double bookWeight)
        {
            int feeShipID = 0;
            if (bookWeight <= 1000)
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == Fees.S1.ToString() && f.IsActive == true);
                if (fee != null)
                {
                    feeShipID = fee.Id;
                }
            }
            if (bookWeight > 1000 && bookWeight <= 3000)
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == Fees.S13.ToString() && f.IsActive == true);
                if (fee != null)
                {
                    feeShipID = fee.Id;
                }
            }
            if (bookWeight > 3000 && bookWeight <= 5000)
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == Fees.S35.ToString() && f.IsActive == true);
                if (fee != null)
                {
                    feeShipID = fee.Id;
                }
            }
            if (bookWeight > 5000)
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == Fees.S5.ToString() && f.IsActive == true);
                if (fee != null)
                {
                    feeShipID = fee.Id;
                }
            }
            return feeShipID;
        }

        private int feeServiceID(int i)
        {
            if (i == 1)
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == Fees.B1.ToString() && f.IsActive == true);
                if (fee != null)
                {
                    return fee.Id;
                }
            }
            if (i > 1)
            {
                var fee = _context.Fees.SingleOrDefault(f => f.Code == Fees.BM.ToString() && f.IsActive == true);
                if (fee != null)
                {
                    return fee.Id;
                }
            }
            return 0;
        }

        private float totalAmountRent(RentBill bill)
        {
            var fee1 = _context.Fees.SingleOrDefault(f => f.Id == bill.FeeId1);
            var fee2 = _context.Fees.SingleOrDefault(f => f.Id == bill.FeeId2);
            var fee3 = _context.Fees.SingleOrDefault(f => f.Id == bill.FeeId3);
            if (fee1 != null && fee2 != null && fee3 == null)
            {
                return bill.DepositFee + bill.RentFee + fee1.Price + fee2.Price;
            }
            if (fee1 != null && fee2 != null && fee3 != null)
            {
                return bill.DepositFee + bill.RentFee + fee1.Price + fee2.Price + fee3.Price * (bill.TotalBook - 1);
            }
            return 0;
        }

        private float totalAmountExchange(ExchangeBill bill)
        {
            var fee1 = _context.Fees.SingleOrDefault(f => f.Id == bill.FeeId1);
            var fee2 = _context.Fees.SingleOrDefault(f => f.Id == bill.FeeId2);
            var fee3 = _context.Fees.SingleOrDefault(f => f.Id == bill.FeeId3);
            if (fee1 != null && fee2 != null && fee3 == null)
            {
                return bill.DepositFee + fee1.Price + fee2.Price;
            }
            if (fee1 != null && fee2 != null && fee3 != null)
            {
                return bill.DepositFee + fee1.Price + fee2.Price + fee3.Price * (bill.TotalBook - 1);
            }
            return 0;
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
