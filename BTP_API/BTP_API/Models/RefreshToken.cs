using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class RefreshToken
    {
        /// <summary>
        /// Mã refresh token
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
        /// Chuỗi token
        /// </summary>
        [Required]
        public string Token { get; set; }
        /// <summary>
        /// Mã access token
        /// </summary>
        [Required]
        public string JwtId { get; set; }
        /// <summary>
        /// Đã sử dụng?
        /// </summary>
        [Required]
        public bool IsUsed { get; set; }
        /// <summary>
        /// Đã hủy
        /// </summary>
        [Required]
        public bool IsRevoked { get; set; }
        /// <summary>
        /// Ngày đăng ký
        /// </summary>
        [Required]
        public DateTime IssueDate { get; set; }
        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [Required]
        public DateTime ExpiredDate { get; set; }

        public virtual User User { get; set; }
    }
}
