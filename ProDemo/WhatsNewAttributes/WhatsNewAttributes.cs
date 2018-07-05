using System;
namespace WhatsNewAttributes
{
    /// <summary>
    /// 在现实生活中，或许想把特性应用到任何对象上。
    /// 为了使代码比较简单，这里仅允许将它应用于类和方法 ，
    /// 并允许它多次应用到同一项上(AllowMultiple = true),
    /// 因为可以多次修改某一项，每次修改都需要用一个不同的特性实例来标记。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class LastModifiedAttribute : Attribute
    {
        /// <summary>
        /// 
        ///
        /// </summary>
        private readonly DateTime _dateModified;

        private readonly string _changes;

        public LastModifiedAttribute(string dateModified, string changes)
        {
            _dateModified = DateTime.Parse(dateModified);
            _changes = changes;
        }
        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime DateModified => _dateModified;
        /// <summary>
        /// 描述修改信息
        /// </summary>
        public string Changes => _changes;
        /// <summary>
        /// 描述该数据项的任何重要问题
        /// </summary>
        public string Issues { get; set; }
    }
    [AttributeUsage(AttributeTargets.Assembly)]
    public class SupportsWhatsNewAttribute : Attribute
    {

    }
}
