namespace EFCoreModelUsingFluentAPI.Models
{
    public class Page
    {
        private Page() { }

        public Page(string content)
        {
            Content = content;
        }
        /// <summary>
        /// 可以将表的列映射到私有字段。 这使得可以创建只读属性并使用在类外无法访问的私有字段。
        /// </summary>
        private int _pageId = 0;
        public int PageId => _pageId;
        public string Content { get; set; }
        /// <summary>
        /// BookId属性为Book的外键
        /// 如果没有此属性，由于已经有了Book属性，则会按约定创建阴影属性
        /// </summary>
        public int BookId { get; set; }
        public Book Book { get; set; }


    }
}