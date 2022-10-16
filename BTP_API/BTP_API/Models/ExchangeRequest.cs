using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class ExchangeRequest
    {
        /// <summary>
        /// Mã yêu cầu đổi
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã sách
        /// </summary>
        [Required]
        public int BookId { get; set; }
        /// <summary>
        /// Mã sách yêu cầu
        /// </summary>
        [Required]
        public int BookOfferId { get; set; }
        /// <summary>
        /// Đồng ý?
        /// </summary>
        [Required]
        public bool IsAccept { get; set; }
        /// <summary>
        /// Thời gian yêu cầu
        /// </summary>
        [Required]
        public DateTime RequestTime { get; set; }
        /// <summary>
        /// Trạng thái yêu cầu
        /// </summary>
        [Required]
        public string Status { get; set; }
        /// <summary>
        /// Mới nhất?
        /// </summary>
        [Required]
        public bool IsNewest { get; set; }
        /// <summary>
        /// Cờ
        /// </summary>
        [Required]
        public bool Flag { get; set; }

        public virtual Book Book { get; set; }
        public virtual Book BookOffer { get; set; }
    }
}
