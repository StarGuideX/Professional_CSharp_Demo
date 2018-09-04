using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingFluentAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Book> AuthoredBooks { get; set; }
        public List<Book> EditedBooks { get; set; }
        /// <summary>
        /// 将两个实体User和Address在数据合并为一个表
        /// </summary>
        public Address Address { get; set; }
    }
}
