using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class ExchangeDetail
    {
        /// <summary>
        /// Mã chi tiết giao dịch đổi
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã giao dịch đổi
        /// </summary>
        [Required]
        public int ExchangeId { get; set; }
        /// <summary>
        /// Mã sách 1
        /// </summary>
        [Required]
        public int Book1Id { get; set; }
        /// <summary>
        /// Trạng thái sách 1 trước giao dịch
        /// </summary>
        public string BeforeStatusBook1 { get; set; }
        /// <summary>
        /// Trạng thái sách 1 sau giao dịch
        /// </summary>
        public string AfterStatusBook1 { get; set; }
        /// <summary>
        /// Trạng thái lưu trữ sách1
        /// </summary>
        [Required]
        public string StorageStatusBook1 { get; set; }
        /// <summary>
        /// Mã sách 2
        /// </summary>
        [Required]
        public int Book2Id { get; set; }
        /// <summary>
        /// Trạng thái sách 2 trước giao dịch
        /// </summary>
        public string BeforeStatusBook2 { get; set; }
        /// <summary>
        /// Trạng thái sách 2 sau giao dịch
        /// </summary>
        public string AfterStatusBook2 { get; set; }
        /// <summary>
        /// Trạng thái lưu trữ sách 2
        /// </summary>
        [Required]
        public string StorageStatusBook2 { get; set; }
        /// <summary>
        /// Thời gian tạo
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

        public virtual Book Book1 { get; set; }
        public virtual Book Book2 { get; set; }
        public virtual Exchange Exchange { get; set; }
    }
}
