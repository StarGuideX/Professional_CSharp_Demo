using System;
using System.Collections.Generic;
using System.Text;

namespace BooksSample
{
    public class Book
    {
        private Book() { }

        public Book(int bookId, string publisher)
        {
            _bookId = bookId;
            _publisher = publisher;
        }

        private int _bookId = 0;
        public string Title { get; set; }
        private string _publisher;
        public string Publisher => _publisher;

        public override string ToString() => $"id{_bookId},title:{Title}, publisher:{Publisher}";
    }
}
