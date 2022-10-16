using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTP_API.Models
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        /// <summary>
        /// Mã loại
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Tên loại
        /// </summary>
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
    }
}
