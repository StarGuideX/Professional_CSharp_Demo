using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreModelUsingAnnotation.Models
{
    [Table("Pages", Schema = "myAnnotation")]
    public class Page
    {
        public int PageId { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// BookId属性为Book的外键
        /// 如果没有此属性，由于已经有了Book属性，则会按约定创建阴影属性
        /// </summary>
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}