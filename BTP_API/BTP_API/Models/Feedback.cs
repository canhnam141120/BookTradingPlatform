using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class Feedback
    {
        /// <summary>
        /// Mã đánh giá
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
        /// Mã người dùng
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        [Required]
        public string Content { get; set; }
        /// <summary>
        /// Ngày đánh giá
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
