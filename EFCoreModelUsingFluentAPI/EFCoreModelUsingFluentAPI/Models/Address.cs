using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    /// <summary>
    /// 将两个实体User和Address在数据合并为一个表
    /// </summary>
    public class Address
    {
        public int AddressId { get; set; }
        public string AddressDetail { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
