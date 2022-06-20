using System;
using Xunit;

namespace Library.Test
{
    public class UnitTest
    {
        [Fact]
        public void AddBookTest()
        {
            Book book = new Book();
            book.BookID = 1;
            book.Author = "John";
            book.Category = "Novel";
            book.ISBN = "435466";
            book.Availability = "Available";
            book.Language = "English";
            book.Name = "Book1";
            book.PublicationDate = DateTime.Parse("2021-01-01");
            var result = Program.AddBook(book);

            Assert.True(result);
        }
        [Fact]
        public void DeleteBookTest()
        {
            Book book = new Book();
            book.BookID = 1;
            book.Author = "John";
            book.Category = "Novel";
            book.ISBN = "435466";
            book.Availability = "Available";
            book.Language = "English";
            book.Name = "Book1";
            book.PublicationDate = DateTime.Parse("2021-02-05");
            Program.AddBook(book);

            var result = Program.DeleteBook(1);
            Assert.True(result);
        }
    }
}
