using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class Comment
    {
        /// <summary>
        /// Mã bình luận
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã bài đăng
        /// </summary>
        [Required]
        public int PostId { get; set; }
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
        /// Ngày bình luận
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
