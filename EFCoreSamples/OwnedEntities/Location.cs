using System;
using System.Collections.Generic;
using System.Text;

namespace OwnedEntities
{
    /// <summary>
    /// Location只包含Country和City属性，并且作为从属实体，它也不定义主键
    /// </summary>
    public class Location
    {
        public string Country { get; set; }
        public string City { get; set; }
    }
}
