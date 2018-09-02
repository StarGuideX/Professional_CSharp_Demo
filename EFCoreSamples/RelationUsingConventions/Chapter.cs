using System;
using System.Collections.Generic;
using System.Text;

namespace RelationUsingConventions
{
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// BookId属性为Book的外键
        /// 如果没有此属性，由于已经有了Book属性，则会按约定创建阴影属性
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// 使用此属性会直接访问Book
        /// </summary>
        public Book Book { get; set; }
    }
}
