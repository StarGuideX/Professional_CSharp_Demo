using System;
using System.Collections.Generic;
using System.Text;

namespace RelationUsingFluentAPI
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; } = new List<Chapter>
        ();
        public User Author { get; set; }
        public User Reviewer { get; set; }
        public User Editor { get; set; }
    }
}
