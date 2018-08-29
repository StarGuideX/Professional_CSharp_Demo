using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Intro
{
    [Table("Books")]
    public class Book
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Title
        /// required—NOT NULL
        /// NVARCHAR(25)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        /// <summary>
        /// NVARCHAR(30)
        /// </summary>
        [StringLength(30)]
        public string Publisher { get; set; }
    }
}
