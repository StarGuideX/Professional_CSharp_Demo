using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RelationUsingAnnotations
{
    /// <summary>
    /// Book与Author(作者)、Reviewer(审阅者)、ProjectEditor(编辑)关联。
    /// 外键属性类型int?，这使他们为可选的。
    /// 通过强制关系，EF Core创建了级联删除; 删除本书时，相关作者，编辑和审阅者也将被删除
    /// </summary>
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; } = new List<Chapter>();
        public int? AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }
        public int? ReviewerId { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        public User Reviewer { get; set; }
        public int? ProjectEditorId { get; set; }
        [ForeignKey(nameof(ProjectEditorId))]
        public User ProjectEditor { get; set; }
    }
}
