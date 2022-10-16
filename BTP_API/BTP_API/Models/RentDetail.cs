using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class RentDetail
    {
        /// <summary>
        /// Mã chi tiết giao dịch thuê
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã giao dịch thuê
        /// </summary>
        [Required]
        public int RentId { get; set; }
        /// <summary>
        /// Mã sách
        /// </summary>
        [Required]
        public int BookId { get; set; }
        /// <summary>
        /// Trạng thái sách trước giao dịch
        /// </summary>
        public string BeforeStatusBook { get; set; }
        /// <summary>
        /// Trang thái sách sau giao dịch
        /// </summary>
        public string AfterStatusBook { get; set; }
        /// <summary>
        /// Trạng thái lưu trữ sách
        /// </summary>
        [Required]
        public string StorageStatusBook { get; set; }
        /// <summary>
        /// Thời gian yêu cầu
        /// </summary>
        [Required]
        public DateTime RequestTime { get; set; }
        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [Required]
        public DateOnly ExpiredDate { get; set; }
        /// <summary>
        /// Trạng thái chi tiết giao dịch
        /// </summary>
        [Required]
        public string Status { get; set; }
        /// <summary>
        /// Cờ
        /// </summary>
        [Required]
        public bool Flag { get; set; }

        public virtual Book Book { get; set; }
        public virtual Rent Rent { get; set; }
    }
}
