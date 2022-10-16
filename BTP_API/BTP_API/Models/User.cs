using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTP_API.Models
{
    public partial class User
    {
        public User()
        {
            Books = new HashSet<Book>();
            Comments = new HashSet<Comment>();
            ExchangeBills = new HashSet<ExchangeBill>();
            ExchangeUserId1Navigations = new HashSet<Exchange>();
            ExchangeUserId2Navigations = new HashSet<Exchange>();
            FavoriteBookLists = new HashSet<FavoriteBookList>();
            FavoritePostLists = new HashSet<FavoritePostList>();
            FavoriteUserListFavoriteUsers = new HashSet<FavoriteUserList>();
            FavoriteUserListUsers = new HashSet<FavoriteUserList>();
            Feedbacks = new HashSet<Feedback>();
            Posts = new HashSet<Post>();
            RefreshTokens = new HashSet<RefreshToken>();
            RentBills = new HashSet<RentBill>();
            RentOwners = new HashSet<Rent>();
            RentRenters = new HashSet<Rent>();
        }

        /// <summary>
        /// Mã người dùng
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã vai trò
        /// </summary>
        [Required]
        public int RoleId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Mật khẩu
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Mã xác thực
        /// </summary>
        [Required]
        public string VerificationToken { get; set; }
        /// <summary>
        /// Đã xác thực?
        /// </summary>
        [Required]
        public bool IsVerify { get; set; }
        /// <summary>
        /// Mã quên mật khẩu
        /// </summary>
        public string ForgotPasswordCode { get; set; }
        /// <summary>
        /// Tên đầy đủ
        /// </summary>
        [Required]
        public string Fullname { get; set; }
        /// <summary>
        /// Tuổi
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [Required]
        public string Phone { get; set; }
        /// <summary>
        /// Địa chỉ chính
        /// </summary>
        [Required]
        public string AddressMain { get; set; }
        /// <summary>
        /// Địa chỉ phụ 1
        /// </summary>
        public string AddressSub1 { get; set; }
        /// <summary>
        /// Địa chỉ phụ 2
        /// </summary>
        public string AddressSub2 { get; set; }
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Đang hoạt động?
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; }
        [JsonIgnore]
        public virtual ShipInfo ShipInfo { get; set; }
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeBill> ExchangeBills { get; set; }
        [JsonIgnore]
        public virtual ICollection<Exchange> ExchangeUserId1Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<Exchange> ExchangeUserId2Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<FavoriteBookList> FavoriteBookLists { get; set; }
        [JsonIgnore]
        public virtual ICollection<FavoritePostList> FavoritePostLists { get; set; }
        [JsonIgnore]
        public virtual ICollection<FavoriteUserList> FavoriteUserListFavoriteUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<FavoriteUserList> FavoriteUserListUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }
        [JsonIgnore]
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        [JsonIgnore]
        public virtual ICollection<RentBill> RentBills { get; set; }
        [JsonIgnore]
        public virtual ICollection<Rent> RentOwners { get; set; }
        [JsonIgnore]
        public virtual ICollection<Rent> RentRenters { get; set; }
    }
}
