using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RelationUsingAnnotations
{
    /// <summary>
    /// User类与Book类型有多个关联。 
    /// WrittenBooks:作者写的书
    /// ReviewBooks：审阅者的书。
    /// EditedBooks：编辑者的书。
    /// 如果相同类型之间存在多于一个的关系。则需要使用InverseProperty对属性进行注释。 
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        [InverseProperty("Author")]
        public List<Book> WrittenBooks { get; set; }
        [InverseProperty("Reviewer")]
        public List<Book> ReviewedBooks { get; set; }
        [InverseProperty("ProjectEditor")]
        public List<Book> EditedBooks { get; set; }
    }
}
