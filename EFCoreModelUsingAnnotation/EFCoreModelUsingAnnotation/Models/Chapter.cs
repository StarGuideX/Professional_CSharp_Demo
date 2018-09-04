﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreModelUsingAnnotation.Models
{
    /// <summary>
    /// 章节
    /// </summary>
    /// 
    [Table("Chapters", Schema = "myAnnotation")]
    public class Chapter
    {
        public int ChapterId { get; set; }
        public int Number { get; set; }
        [MaxLength(120)]
        public string Title { get; set; }
        /// <summary>
        /// BookId属性为Book的外键
        /// 如果没有此属性，由于已经有了Book属性，则会按约定创建阴影属性
        /// </summary>
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}