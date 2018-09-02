using System;
using System.Collections.Generic;
using System.Text;

namespace RelationUsingConventions
{
    public class Book
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int BookId { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 一本书有多个章节
        /// </summary>
        public List<Chapter> Chapters { get; } = new List<Chapter>();
        /// <summary>
        /// 一个作者会有多本书
        /// 在多对一的关系中，Book作为多的一方，User作为一的一方。Book会有User的外键。
        /// 有可能会添加阴影属性AuthorId，外键到指定User中的主键。
        /// </summary>
        public User Author { get; set; }
    }
}
