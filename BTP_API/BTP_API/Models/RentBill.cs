using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class RentBill
    {
        /// <summary>
        /// Mã hóa đơn giao dịch thuê
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
        /// Mã người dùng
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Tổng sách
        /// </summary>
        [Required]
        public int TotalBook { get; set; }
        /// <summary>
        /// Tổng tiền
        /// </summary>
        [Required]
        public float TotalAmount { get; set; }
        /// <summary>
        /// Phí đặt cocj
        /// </summary>
        [Required]
        public float DepositFee { get; set; }
        /// <summary>
        /// Phí thuê
        /// </summary>
        [Required]
        public float RentFee { get; set; }
        /// <summary>
        /// Mã phí 1
        /// </summary>
        [Required]
        public int FeeId1 { get; set; }
        /// <summary>
        /// Mã phí 2
        /// </summary>
        [Required]
        public int FeeId2 { get; set; }
        /// <summary>
        /// Mã phí 3
        /// </summary>
        public int? FeeId3 { get; set; }
        /// <summary>
        /// Ngày gửi
        /// </summary>
        public DateOnly? SendDate { get; set; }
        /// <summary>
        /// Ngày nhận
        /// </summary>
        public DateOnly? ReceiveDate { get; set; }
        /// <summary>
        /// Ngày thu hồi
        /// </summary>
        public DateOnly? RecallDate { get; set; }
        /// <summary>
        /// Ngày hoàn trả
        /// </summary>
        public DateOnly? RefundDate { get; set; }
        /// <summary>
        /// Đã thanh toán?
        /// </summary>
        [Required]
        public bool IsPaid { get; set; }
        /// <summary>
        /// Ngày thanh toán
        /// </summary>
        public DateTime? PaidDate { get; set; }
        /// <summary>
        /// Phương thức thanh toán
        /// </summary>
        public string Payment { get; set; }
        /// <summary>
        /// Cờ
        /// </summary>
        [Required]
        public bool Flag { get; set; }

        public virtual Fee FeeId1Navigation { get; set; }
        public virtual Fee FeeId2Navigation { get; set; }
        public virtual Fee FeeId3Navigation { get; set; }
        public virtual Rent Rent { get; set; }
        public virtual User User { get; set; }
    }
}
