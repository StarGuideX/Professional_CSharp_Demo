using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    /// <summary>
    /// 涉及技术：表拆分
    /// 一个数据库表分为两个实体User和Address
    /// </summary>
public class Address
{
    public int AddressId { get; set; }
    public string AddressDetail { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
}
