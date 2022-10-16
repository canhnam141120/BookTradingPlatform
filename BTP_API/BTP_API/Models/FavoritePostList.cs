using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class FavoritePostList
    {
        /// <summary>
        /// Mã bài đăng yêu thích
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

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
