using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTP_API.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            FavoritePostLists = new HashSet<FavoritePostList>();
        }

        /// <summary>
        /// Mã bài đăng
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
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
        /// Ảnh
        /// </summary>
        [Required]
        public string Image { get; set; }
        /// <summary>
        /// Thẻ
        /// </summary>
        [Required]
        public string Hashtag { get; set; }
        /// <summary>
        /// Ngày đăng
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Đã ẩn?
        /// </summary>
        [Required]
        public bool IsHide { get; set; }
        /// <summary>
        /// Trạng thái bài đăng
        /// </summary>
        [Required]
        public string Status { get; set; }

        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<FavoritePostList> FavoritePostLists { get; set; }
    }
}
