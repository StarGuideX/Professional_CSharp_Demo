using System;
using System.Collections.Generic;
using System.Text;

namespace OwnedEntities
{
    /// <summary>
    /// Person：拥有主键PersonId的从属实体的所有者。并包含两个Address
    /// </summary>
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public Address PrivateAddress { get; set; }
        public Address CompanyAddress { get; set; }
    }
}
