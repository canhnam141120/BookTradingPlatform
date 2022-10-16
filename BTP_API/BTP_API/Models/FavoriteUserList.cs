using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class FavoriteUserList
    {
        /// <summary>
        /// Mã yêu thích bài đăng
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã người dùng được yêu thích
        /// </summary>
        [Required]
        public int FavoriteUserId { get; set; }
        /// <summary>
        /// Mã người dùng yêu thích
        /// </summary>
        [Required]
        public int UserId { get; set; }

        public virtual User FavoriteUser { get; set; }
        public virtual User User { get; set; }
    }
}
