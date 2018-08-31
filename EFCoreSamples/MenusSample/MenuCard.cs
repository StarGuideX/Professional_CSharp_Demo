using System;
using System.Collections.Generic;

namespace MenusSample
{
    //[Table("MenuCards", Schema = "mc")]
    public class MenuCard
    {
        public int MenuCardId { get; set; }
        //[MaxLength(120)]
        public string Title { get; set; }
        public List<Menu> Menus { get; } = new List<Menu>();
        public override string ToString() => Title;
    }
}
