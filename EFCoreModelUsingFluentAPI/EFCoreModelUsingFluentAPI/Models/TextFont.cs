namespace EFCoreModelUsingFluentAPI.Models
{
    /// <summary>
    /// 涉及技术：从属实体
    /// 直观体现：没有自己的主键
    /// </summary>
    public class TextFont
    {
        public string FontName { get; set; }

        public FontColor FontColor { get; set; }
    }
}