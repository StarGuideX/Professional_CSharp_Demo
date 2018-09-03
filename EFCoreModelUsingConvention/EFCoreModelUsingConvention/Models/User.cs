using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreModelUsingConvention.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<Book> AuthoredBooks { get; set; }
    }
}
