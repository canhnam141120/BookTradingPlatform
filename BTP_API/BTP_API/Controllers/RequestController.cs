using BTP_API.Helpers;
using BTP_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using static BTP_API.Helpers.EnumVariable;

namespace BookTradingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {

        private readonly BTPContext _context;

        public RequestController(BTPContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult Create(int bookid, List<int> bookOffer)
        {
            try
            {
                foreach (int i in bookOffer)
                {
                    var request = new ExchangeRequest
                    {
                        BookId = bookid,
                        BookOfferId = i,
                        IsAccept = false,
                        RequestTime = DateTime.Now,
                        Status = StatusRequest.Waiting.ToString(),
                        IsNewest = true,
                        Flag = true
                    };
                    _context.Add(request);

                    var book = _context.Books.SingleOrDefault(b => b.Id == i);
                    if (book != null)
                    {
                        book.IsTrade = true;
                    }
                    _context.SaveChanges();
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Yêu cầu thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Người offer tự hủy
        [HttpPut("cancel/{id}")]
        public IActionResult CancelRequest(int id)
        {
            try
            {
                var request = _context.ExchangeRequests.SingleOrDefault(r => r.Id == id && r.Status == StatusRequest.Waiting.ToString());
                if (request == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy yêu cầu!"
                    });
                }

                request.IsNewest = false;
                request.Status = StatusRequest.Cancel.ToString();
                var book = _context.Books.SingleOrDefault(b => b.Id == id);
                if (book != null)
                {
                    book.IsTrade = false;
                }
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Hủy yêu cầu thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("accept/{id}")]
        public IActionResult AcceptRequest(int id)
        {
            try
            {
                var request = _context.ExchangeRequests.SingleOrDefault(r => r.Id == id && r.Status == StatusRequest.Waiting.ToString());
                if (request == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy yêu cầu!"
                    });
                }

                var listRequect = _context.ExchangeRequests.Where(r => r.BookId == request.BookId
                && r.IsNewest == true && r.Status == StatusRequest.Waiting.ToString());
                List<int> bookIds = new List<int>();
                foreach (var item in listRequect)
                {
                    item.Status = StatusRequest.Denied.ToString();
                    item.IsNewest = false;
                    bookIds.Add(item.BookOfferId);
                }
                foreach (var item in bookIds)
                {
                    var bookOffer = _context.Books.SingleOrDefault(r => r.Id == item);
                    if (bookOffer != null)
                    {
                        bookOffer.IsTrade = false;
                    }
                }

                request.IsAccept = true;
                request.Status = StatusRequest.Approved.ToString();

                var book1 = _context.Books.SingleOrDefault(r => r.Id == request.BookId);
                var book2 = _context.Books.SingleOrDefault(r => r.Id == request.BookOfferId);

                if (book1 == null || book2 == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Không thấy id sách!"
                    });
                }

                book1.IsTrade = true;
                book2.IsTrade = true;

                var newExchange = new Exchange
                {
                    UserId1 = book1.UserId,
                    UserId2 = book2.UserId,
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Status = Status.Waiting.ToString(),
                };

                int numberOfDays = 0;
                if (book1.NumberOfDays > book2.NumberOfDays)
                {
                    numberOfDays = book1.NumberOfDays;
                }
                else
                {
                    numberOfDays = book2.NumberOfDays;
                }

                var check1 = _context.Exchanges.SingleOrDefault(e => e.UserId1 == newExchange.UserId1 && e.UserId2 == newExchange.UserId2 && e.Date == newExchange.Date);

                var check2 = _context.Exchanges.SingleOrDefault(e => e.UserId1 == newExchange.UserId2 && e.UserId2 == newExchange.UserId1 && e.Date == newExchange.Date);


                //Nếu giao dịch chưa có => tạo mới
                if (check1 == null && check2 == null)
                {
                    _context.Add(newExchange);
                    _context.SaveChanges();
                    var exchange = _context.Exchanges.SingleOrDefault(e => e.UserId1 == newExchange.UserId1 && e.UserId2 == newExchange.UserId2 && e.Date == newExchange.Date);
                    //Tạo mới cả chi tiết giao dịch và hóa đơn cho giao dịch đó
                    if (exchange != null)
                    {
                        var newExchangeDetail = new ExchangeDetail
                        {
                            ExchangeId = exchange.Id,
                            Book1Id = book1.Id,
                            StorageStatusBook1 = StorageStatus.Waiting.ToString(),
                            Book2Id = book2.Id,
                            StorageStatusBook2 = StorageStatus.Waiting.ToString(),
                            RequestTime = DateTime.Now,
                            ExpiredDate = DateOnly.FromDateTime(DateTime.Now.AddDays(numberOfDays)),
                            Status = Status.Waiting.ToString(),
                            Flag = true
                        };
                        _context.Add(newExchangeDetail);

                        var newBillUser1 = new ExchangeBill
                        {
                            ExchangeId = exchange.Id,
                            UserId = book1.UserId,
                            TotalBook = 1,
                            TotalAmount = 0,
                            DepositFee = book2.DepositPrice,
                            FeeId1 = feeShipID(book1.Weight),
                            FeeId2 = feeServiceID(1),
                            IsPaid = false,
                            Flag = true
                        };

                        newBillUser1.TotalAmount = totalAmountExchange(newBillUser1);

                        var newBillUser2 = new ExchangeBill
                        {
                            ExchangeId = exchange.Id,
                            UserId = book2.UserId,
                            TotalBook = 1,
                            TotalAmount = 0,
                            DepositFee = book1.DepositPrice,
                            FeeId1 = feeShipID(book2.Weight),
                            FeeId2 = feeServiceID(1),
                            IsPaid = false,
                            Flag = true
                        };
                        newBillUser2.TotalAmount = totalAmountExchange(newBillUser2);

                        _context.Add(newBillUser1);
                        _context.Add(newBillUser2);
                        _context.SaveChanges();
                    }
                }
                //Nếu đã có giao dịch => Update giao dịch
                else
                {
                    if (check1 != null && check2 == null)
                    {
                        var newExchangeDetail = new ExchangeDetail
                        {
                            ExchangeId = check1.Id,
                            Book1Id = book1.Id,
                            StorageStatusBook1 = StorageStatus.Waiting.ToString(),
                            Book2Id = book2.Id,
                            StorageStatusBook2 = StorageStatus.Waiting.ToString(),
                            RequestTime = DateTime.Now,
                            ExpiredDate = DateOnly.FromDateTime(DateTime.Now.AddDays(numberOfDays)),
                            Status = Status.Waiting.ToString(),
                            Flag = true
                        };

                        double totalWeightBook1 = 0;
                        double totalWeightBook2 = 0;
                        var listBook = _context.ExchangeDetails.Include(e => e.Book1).Include(e => e.Book2).Where(e => e.ExchangeId == check1.Id).ToList();
                        foreach (var item in listBook)
                        {
                            totalWeightBook1 += item.Book1.Weight;
                            totalWeightBook2 += item.Book2.Weight;
                        }

                        var billUser1 = _context.ExchangeBills.SingleOrDefault(e => e.ExchangeId == check1.Id && e.UserId == book1.UserId);
                        if (billUser1 != null)
                        {
                            billUser1.TotalBook += 1;
                            billUser1.DepositFee += book2.DepositPrice;
                            billUser1.FeeId1 = feeShipID(totalWeightBook1 + book1.Weight);
                            billUser1.FeeId2 = feeServiceID(1);
                            billUser1.FeeId3 = feeServiceID(2);
                            billUser1.TotalAmount = totalAmountExchange(billUser1);
                        }
                        var billUser2 = _context.ExchangeBills.SingleOrDefault(e => e.ExchangeId == check1.Id && e.UserId == book2.UserId);
                        if (billUser2 != null)
                        {
                            billUser2.TotalBook += 1;
                            billUser2.DepositFee += book1.DepositPrice;
                            billUser2.FeeId1 = feeShipID(totalWeightBook2 + book2.Weight);
                            billUser2.FeeId2 = feeServiceID(1);
                            billUser2.FeeId3 = feeServiceID(2);
                            billUser2.TotalAmount = totalAmountExchange(billUser2);
                        }
                        _context.Add(newExchangeDetail);
                        _context.SaveChanges();
                    }
                    if (check1 == null && check2 != null)
                    {
                        var newExchangeDetail = new ExchangeDetail
                        {
                            ExchangeId = check2.Id,
                            Book1Id = book2.Id,
                            StorageStatusBook1 = StorageStatus.Waiting.ToString(),
                            Book2Id = book1.Id,
                            StorageStatusBook2 = StorageStatus.Waiting.ToString(),
                            RequestTime = DateTime.Now,
                            ExpiredDate = DateOnly.FromDateTime(DateTime.Now.AddDays(numberOfDays)),
                            Status = Status.Waiting.ToString(),
                            Flag = true
                        };

                        double totalWeightBook1 = 0;
                        double totalWeightBook2 = 0;
                        var listBook = _context.ExchangeDetails.Include(e => e.Book1).Include(e => e.Book2).Where(e => e.ExchangeId == check2.Id).ToList();
                        foreach (var item in listBook)
                        {
                            totalWeightBook1 += item.Book1.Weight;
                            totalWeightBook2 += item.Book2.Weight;
                        }

                        var billUser1 = _context.ExchangeBills.SingleOrDefault(e => e.ExchangeId == check2.Id && e.UserId == book2.UserId);
                        if (billUser1 != null)
                        {
                            billUser1.TotalBook += 1;
                            billUser1.DepositFee += book1.DepositPrice;
                            billUser1.FeeId1 = feeShipID(totalWeightBook1 + book1.Weight);
                            billUser1.FeeId2 = feeServiceID(1);
                            billUser1.FeeId3 = feeServiceID(2);
                            billUser1.TotalAmount = totalAmountExchange(billUser1);
                        }
                        var billUser2 = _context.ExchangeBills.SingleOrDefault(e => e.ExchangeId == check2.Id && e.UserId == book1.UserId);
                        if (billUser2 != null)
                        {
                            billUser2.TotalBook += 1;
                            billUser2.DepositFee += book2.DepositPrice;
                            billUser2.FeeId1 = feeShipID(totalWeightBook2 + book2.Weight);
                            billUser2.FeeId2 = feeServiceID(1);
                            billUser2.FeeId3 = feeServiceID(2);
                            billUser2.TotalAmount = totalAmountExchange(billUser2);
                        }
                        _context.Add(newExchangeDetail);
                        _context.SaveChanges();
                    }
                }
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Chấp nhận yêu cầu thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("denied/{id}")]
        public IActionResult DeniedRequest(int id)
        {
            try
            {
                var request = _context.ExchangeRequests.SingleOrDefault(r => r.Id == id && r.Status == StatusRequest.Waiting.ToString());
                if (request == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy yêu cầu!"
                    });
                }
                request.IsNewest = false;
                request.Status = StatusRequest.Denied.ToString();

                var book = _context.Books.SingleOrDefault(b => b.Id == request.BookOfferId);
                if (book != null)
                {
                    book.IsTrade = false;
                }

                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Từ chối yêu cầu thành công!"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("rent/{bookId}")]
        public IActionResult RentBook(int bookId)
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

                var book = _context.Books.SingleOrDefault(b => b.Id == bookId && b.IsRent == true && b.IsTrade == false && b.Status == StatusRequest.Approved.ToString());
                if (book == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Không tìm thấy sách!"
                    });
                }
                book.IsTrade = true;
                var rentNew = new Rent
                {
                    OwnerId = book.UserId,
                    RenterId = userId,
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Status = Status.Waiting.ToString(),
                };

                var check = _context.Rents.SingleOrDefault(r => r.OwnerId == book.UserId && r.RenterId == userId && r.Date == rentNew.Date);
                //Nếu chưa có giao dịch giữa 2 người này
                if (check == null)
                {
                    _context.Add(rentNew);
                    _context.SaveChanges();
                    var rent = _context.Rents.SingleOrDefault(r => r.OwnerId == book.UserId && r.RenterId == userId && r.Date == rentNew.Date);
                    if (rent == null)
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Không tìm thấy giao dịch!"
                        });
                    }

                    var rentDetail = new RentDetail
                    {
                        RentId = rent.Id,
                        BookId = bookId,
                        StorageStatusBook = StorageStatus.Waiting.ToString(),
                        RequestTime = DateTime.Now,
                        ExpiredDate = DateOnly.FromDateTime(DateTime.Now.AddDays(book.NumberOfDays)),
                        Status = Status.Waiting.ToString(),
                        Flag = true
                    };
                    _context.Add(rentDetail);

                    var newBillOwner = new RentBill
                    {
                        RentId = rent.Id,
                        UserId = rent.OwnerId,
                        TotalBook = 1,
                        DepositFee = 0,
                        RentFee = 0,
                        FeeId1 = feeShipID(book.Weight),
                        FeeId2 = feeServiceID(1),
                        IsPaid = false,
                        Flag = true
                    };
                    newBillOwner.TotalAmount = totalAmountRent(newBillOwner);

                    var newBillRenter = new RentBill
                    {
                        RentId = rent.Id,
                        UserId = rent.RenterId,
                        TotalBook = 1,
                        DepositFee = book.DepositPrice,
                        RentFee = book.RentFee,
                        FeeId1 = feeShipID(book.Weight),
                        FeeId2 = feeServiceID(1),
                        IsPaid = false,
                        Flag = true
                    };
                    newBillRenter.TotalAmount = totalAmountRent(newBillRenter);
                    _context.Add(newBillOwner);
                    _context.Add(newBillRenter);
                    _context.SaveChanges();
                }
                //Nếu đã có giao dịch
                else
                {
                    var rentDetail = new RentDetail
                    {
                        RentId = check.Id,
                        BookId = bookId,
                        StorageStatusBook = StorageStatus.Waiting.ToString(),
                        RequestTime = DateTime.Now,
                        ExpiredDate = DateOnly.FromDateTime(DateTime.Now.AddDays(book.NumberOfDays)),
                        Status = Status.Waiting.ToString(),
                        Flag = true
                    };

                    double totalWeightBook = 0;

                    var listBook = _context.RentDetails.Include(e => e.Book).Where(e => e.RentId == check.Id).ToList();
                    foreach (var item in listBook)
                    {
                        totalWeightBook += item.Book.Weight;
                    }

                    var billOnwer = _context.RentBills.SingleOrDefault(b => b.RentId == check.Id && b.UserId == check.OwnerId);
                    if (billOnwer != null)
                    {
                        billOnwer.TotalBook += 1;
                        billOnwer.FeeId1 = feeShipID(totalWeightBook + book.Weight);
                        billOnwer.FeeId2 = feeServiceID(1);
                        billOnwer.FeeId3 = feeServiceID(2);
                        billOnwer.TotalAmount = totalAmountRent(billOnwer);
                    }

                    var billRenter = _context.RentBills.SingleOrDefault(b => b.RentId == check.Id && b.UserId == check.RenterId);
                    if (billRenter != null)
                    {
                        billRenter.TotalBook += 1;
                        billRenter.DepositFee += book.DepositPrice;
                        billRenter.RentFee += book.RentFee;
                        billRenter.FeeId1 = feeShipID(totalWeightBook + book.Weight);
                        billRenter.FeeId2 = feeServiceID(1);
                        billRenter.FeeId3 = feeServiceID(2);
                        billRenter.TotalAmount = totalAmountRent(billRenter);
                    }

                    _context.Add(rentDetail);
                    _context.SaveChanges();
                }
                _context.SaveChanges();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Thuê thành công!"
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
