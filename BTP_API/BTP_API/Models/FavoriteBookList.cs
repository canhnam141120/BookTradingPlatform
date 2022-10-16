using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTP_API.Models
{
    public partial class FavoriteBookList
    {
        /// <summary>
        /// Mã sách yêu thích
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

        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
