namespace EFCoreModelUsingFluentAPI.Models
{
    /// <summary>
    /// 涉及技术：字段映射、从属实体
    /// </summary>
    public class Page
    {
        private Page() { }

        public string Content { get; set; }

        #region 字段映射
        public Page(string remark)
        {
            _remark = remark;
        }
        /// <summary>
        /// 可以将表的列映射到私有字段。 这使得可以创建只读属性并使用在类外无法访问的私有字段。
        /// </summary>
        private int _pageId = 0;
        
        private string _remark;
        public string Remark => _remark;
        #endregion

        /// <summary>
        /// BookId属性为Book的外键
        /// 如果没有此属性，由于已经有了Book属性，则会按约定创建阴影属性
        /// </summary>
        public int BookId { get; set; }
        public Book Book { get; set; }

        #region 从属实体
        public TextFont TextFont { get; set; }
        public TextFont TitleFont { get; set; }
        #endregion


        public override string ToString()
        {
            return $"PageId:{_pageId},Content{Content}";
        }
    }
}