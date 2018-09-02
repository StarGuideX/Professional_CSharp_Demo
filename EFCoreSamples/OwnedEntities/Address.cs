using System;
using System.Collections.Generic;
using System.Text;

namespace OwnedEntities
{
    /// <summary>
    /// Address是一个从属实体——没有自己的主键的类型。
    /// 有两个字符串属性，以及Location类型的Location属性，Location属性是另一个从属实体。
    /// </summary>
    public class Address
    {
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public Location Location { get; set; }
    }
}
