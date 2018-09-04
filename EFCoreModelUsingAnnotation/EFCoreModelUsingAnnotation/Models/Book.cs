using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFCoreModelUsingAnnotation.Models
{
    /// <summary>
    /// Book与Author(作者)、Reviewer(审阅者)、ProjectEditor(编辑)关联。
    /// 外键属性类型int?，这使他们为可选的。
    /// 如果外键属性为int，通过强制关系，EF Core创建了级联删除; 删除本书时，相关作者，编辑和审阅者也将被删除
    /// </summary>
    /// 
    [Table("Books", Schema = "myAnnotation")]
    public class Book
    {
        public int BookId { get; set; }
        [Column(TypeName = "Money")]
        public decimal Price { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; } = new List<Chapter>();
        /// <summary>
        /// Page会通过Context自动创建实例
        /// </summary>
        public List<Page> Pages { get; set; }
        /// <summary>
        /// 一个作者会有多本书
        /// 在多对一的关系中，Book作为多的一方，User作为一的一方。Book会有User的外键。
        /// 有可能会添加阴影属性AuthorId，外键到指定User中的主键。
        /// </summary>
        public User Author { get; set; }
        public int? ReviewerId { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        public User Reviewer { get; set; }
        public int ProjectEditorId { get; set; }
        [ForeignKey(nameof(ProjectEditorId))]
        public User ProjectEditor { get; set; }
    }
}
