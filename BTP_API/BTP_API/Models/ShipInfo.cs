using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class ShipInfo
    {
        /// <summary>
        /// Mã người dùng
        /// </summary>
        [Key]
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Gửi thứ 2?
        /// </summary>
        public bool? SendIsMonday { get; set; }
        /// <summary>
        /// Gửi thứ 4?
        /// </summary>
        public bool? SendIsWednesday { get; set; }
        /// <summary>
        /// Gửi thứ 6?
        /// </summary>
        public bool? SendIsFriday { get; set; }
        /// <summary>
        /// Nhận thứ 2?
        /// </summary>
        public bool? ReceiveIsMonday { get; set; }
        /// <summary>
        /// Nhận thứ 4?
        /// </summary>
        public bool? ReceiveIsWednesday { get; set; }
        /// <summary>
        /// Nhận thứ 6?
        /// </summary>
        public bool? ReceiveIsFriday { get; set; }
        /// <summary>
        /// Thu hồi thứ 2
        /// </summary>
        public bool? RecallIsMonday { get; set; }
        /// <summary>
        /// Thu hồi thứ 4
        /// </summary>
        public bool? RecallIsWednesday { get; set; }
        /// <summary>
        /// Thu hồi thứ 6
        /// </summary>
        public bool? RecallIsFriday { get; set; }
        /// <summary>
        /// Hoàn trả thứ 2
        /// </summary>
        public bool? RefundIsMonday { get; set; }
        /// <summary>
        /// Hoàn trả thứ 4
        /// </summary>
        public bool? RefundIsWednesday { get; set; }
        /// <summary>
        /// Hoàn trả thứ 6
        /// </summary>
        public bool? RefundIsFriday { get; set; }
        /// <summary>
        /// Đã cập nhật?
        /// </summary>
        [Required]
        public bool IsUpdate { get; set; }

        public virtual User User { get; set; }
    }
}
