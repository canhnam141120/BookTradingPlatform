using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BTP_API.Models
{
    public partial class Fee
    {
        public Fee()
        {
            ExchangeBillFeeId1Navigations = new HashSet<ExchangeBill>();
            ExchangeBillFeeId2Navigations = new HashSet<ExchangeBill>();
            ExchangeBillFeeId3Navigations = new HashSet<ExchangeBill>();
            RentBillFeeId1Navigations = new HashSet<RentBill>();
            RentBillFeeId2Navigations = new HashSet<RentBill>();
            RentBillFeeId3Navigations = new HashSet<RentBill>();
        }

        /// <summary>
        /// Mã phí
        /// </summary>
        [Key]
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Mã code phí
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// Tên phí
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        [Required]
        public float Price { get; set; }
        /// <summary>
        /// Đang hoạt động
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<ExchangeBill> ExchangeBillFeeId1Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeBill> ExchangeBillFeeId2Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<ExchangeBill> ExchangeBillFeeId3Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<RentBill> RentBillFeeId1Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<RentBill> RentBillFeeId2Navigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<RentBill> RentBillFeeId3Navigations { get; set; }
    }
}
