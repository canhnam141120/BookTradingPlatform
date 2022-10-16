using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTP_API.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        /// <summary>
        /// Mã vai trò
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Vai trò
        /// </summary>
        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
