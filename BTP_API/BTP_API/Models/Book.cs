using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTP_API.Models
{
    public partial class Book
    {
        public Book()
        {
            ExchangeDetailBook1s = new HashSet<ExchangeDetail>();
            ExchangeDetailBook2s = new HashSet<ExchangeDetail>();
            ExchangeRequestBookOffers = new HashSet<ExchangeRequest>();
            ExchangeRequestBooks = new HashSet<ExchangeRequest>();
            FavoriteBookLists = new HashSet<FavoriteBookList>();
            Feedbacks = new HashSet<Feedback>();
            RentDetails = new HashSet<RentDetail>();
        }

        /// <summary>
        /// Mã sách
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã người dùng
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Mã loại
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
        /// <summary>
        /// Tên sách
        /// </summary>
        [Required]
        public string Title { get; set; }
        /// <summary>
        /// Mô tả nội dung
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Tác giả
        /// </summary>
        [Required]
        public string Author { get; set; }
        /// <summary>
        /// Nhà xuất bản
        /// </summary>
        [Required]
        public string Publisher { get; set; }
        /// <summary>
        /// Năm xuất bản
        /// </summary>
        [Required]
        public int Year { get; set; }
        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        [Required]
        public string Language { get; set; }
        /// <summary>
        /// Số trang
        /// </summary>
        [Required]
        public int NumberOfPages { get; set; }
        /// <summary>
        /// Trọng lượng
        /// </summary>
        [Required]
        public float Weight { get; set; }
        /// <summary>
        /// Giá bìa
        /// </summary>
        [Required]
        public float CoverPrice { get; set; }
        /// <summary>
        /// Giá cọc
        /// </summary>
        [Required]
        public float DepositPrice { get; set; }
        /// <summary>
        /// Trạng thái sách
        /// </summary>
        [Required]
        public string StatusBook { get; set; }
        [Required]
        public string Image { get; set; }
        /// <summary>
        /// Ngày đăng
        /// </summary>
        [Required]
        public DateOnly PostedDate { get; set; }
        /// <summary>
        /// Trao đổi?
        /// </summary>
        [Required]
        public bool IsExchange { get; set; }
        /// <summary>
        /// Thuê?
        /// </summary>
        [Required]
        public bool IsRent { get; set; }
        /// <summary>
        /// Phí thuê?
        /// </summary>
        [Required]
        public float RentFee { get; set; }
        /// <summary>
        /// Số ngày
        /// </summary>
        [Required]
        public int NumberOfDays { get; set; }
        /// <summary>
        /// Sẵn sàng?
        /// </summary>
        [Required]
        public bool IsReady { get; set; }
        /// <summary>
        /// Đang giao dịch?
        /// </summary>
        [Required]
        public bool IsTrade { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        [Required]
        public string Status { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeDetail> ExchangeDetailBook1s { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeDetail> ExchangeDetailBook2s { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeRequest> ExchangeRequestBookOffers { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeRequest> ExchangeRequestBooks { get; set; }
        [JsonIgnore]
        public virtual ICollection<FavoriteBookList> FavoriteBookLists { get; set; }
        [JsonIgnore]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [JsonIgnore]
        public virtual ICollection<RentDetail> RentDetails { get; set; }
    }
}
