using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;

namespace Library
{
    public class Program
    {

        public static List<Book> BookList = new List<Book>();
        static void Main(string[] args)
        {
            var path = @".\BookLibrary.json";
            string jsonFile = File.ReadAllText(path);
            int BookID = 0;
            int x = 0;
            BookList = JsonConvert.DeserializeObject<List<Book>>(jsonFile);
            foreach (var p in BookList)
            {
                if (p.BookID >= BookID)
                    BookID = p.BookID + 1;
            }
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Library");
                Console.WriteLine("Add a new book[1]");
                Console.WriteLine("Borrow a book[2]");
                Console.WriteLine("Return a book[3]");
                Console.WriteLine("List of books[4]");
                Console.WriteLine("Delete a book[5]");
                Console.WriteLine("Exit[0]");
                Console.WriteLine("Choose an action by entering a number:");
                try
                {
                    x = Convert.ToInt32(Console.ReadLine());
                    if (x == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Exiting...");
                        Thread.Sleep(1000);
                        break;
                    }
                    else
                        Menu(x, BookList);

                    jsonFile = JsonConvert.SerializeObject(BookList);
                    File.WriteAllText(path, jsonFile);
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e + "\nPress Enter to go back.");
                    Console.ReadLine();
                    break;
                }

            }
        }
        public static void Menu(int x, List<Book> BookList)
        {
            int c;
            int n = 0;
            int position = 0;
            string name;
            int days;
            switch (x)
            {

                case 1:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Book book = new Book();
                    Console.WriteLine("Enter the name of the book:");
                    book.Name = Console.ReadLine().ToUpper();
                    Console.WriteLine("Enter the Author:");
                    book.Author = Console.ReadLine().ToUpper();
                    Console.WriteLine("Enter the category:");
                    book.Category = Console.ReadLine().ToUpper();
                    Console.WriteLine("Enter the language:");
                    book.Language = Console.ReadLine().ToUpper();
                    Console.WriteLine("Enter the publication Date (e.g. 1952 02 26):");
                    try
                    {
                        book.PublicationDate = Convert.ToDateTime(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("This is not a valid date.\nPress Enter to go back.");
                        Console.ReadLine();
                        break;
                    }
                    if (book.PublicationDate > DateTime.Now)
                    {
                        Console.WriteLine("This is not a valid date.\nPress Enter to go back.");
                        Console.ReadLine();
                        break;
                    }
                    Console.WriteLine("Enter the ISBN (13 digits):");
                    book.ISBN = Console.ReadLine();
                    if (book.ISBN.Count() != 13)
                    {
                        Console.WriteLine("This is not a valid ISBN.\nPress Enter to go back.");
                        Console.ReadLine();
                        break;

                    }
                    book.Availability = "Available";
                    book.BookID = BookList.Max(x => x.BookID) + 1;

                    if (AddBook(book))
                    {
                        Console.Clear();
                        Console.WriteLine("Book added...\nPress Enter to go back.");
                        Console.ReadLine();
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("List of available books:");
                    Console.ForegroundColor = ConsoleColor.Green;
                    for (int i = 0; i < BookList.Count; i++)
                    {
                        if (BookList[i].Availability == "Available")
                            Console.WriteLine(BookList[i].FullInfo);
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Enter chosen BookID:");
                    c = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < BookList.Count; i++)
                    {
                        if (BookList[i].Availability == "Available" && BookList[i].BookID == c)
                        {
                            position = i;
                            break;
                        }
                        else if (i == BookList.Count - 1)
                            n = -1;

                    }
                    if (n == -1)
                    {
                        Console.WriteLine("BookID: " + c + " is not available\nPress Enter to go back.");
                        n = 0;
                        Console.ReadLine();
                        break;
                    }
                    Console.WriteLine("Enter your first and last name:");
                    name = Console.ReadLine().ToUpper();
                    for (int i = 0; i < BookList.Count; i++)
                    {
                        if (BookList[i].Availability == name)
                            n++;
                    }
                    if (n >= 3)
                    {
                        Console.WriteLine("Taking more than 3 books is not allowed!\nPress Enter to go back.");
                        Console.ReadLine();
                        break;
                    }
                    Console.WriteLine("Enter for how many days the book will be taken (maximum 60 days):");
                    days = Convert.ToInt32(Console.ReadLine());
                    if (days > 60)
                    {
                        Console.WriteLine("Taking a book for more than two months (60 days) is not allowed.\nPress Enter to go back.");
                        Console.ReadLine();
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Enjoy your book :)\nPlease bring it back by " + (DateTime.Now.AddDays(days).ToString("yyyy-MM-dd")) + "\nPress Enter to go back.");
                        BookList[position].Availability = name.ToUpper();
                        BookList[position].ReturnDate = DateTime.Now.AddDays(days);
                        Console.ReadLine();
                        break;
                    }
                case 3:
                    Console.Clear();
                    Console.WriteLine("Please enter your first and last name to see your list of taken books:");
                    name = Console.ReadLine().ToUpper();
                    for (int i = 0; i < BookList.Count; i++)
                    {
                        if (BookList[i].Availability == name)
                        {
                            if (BookList[i].ReturnDate < DateTime.Now)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(BookList[i].FullInfo + "\nLATE ON RETURN");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(BookList[i].FullInfo);
                            }

                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("To return a book enter the BookID");
                    n = Convert.ToInt32(Console.ReadLine());
                    for (int i = 0; i < BookList.Count; i++)
                    {
                        if (n == BookList[i].BookID && name == BookList[i].Availability)
                        {
                            if (BookList[i].ReturnDate < DateTime.Now)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("I've waited for years...");
                                Thread.Sleep(1300);
                                Console.WriteLine("By Now it must be...");
                                Thread.Sleep(1300);
                                Console.WriteLine("Twenty-three years...");
                                Thread.Sleep(1300);
                                BookList[i].Availability = "Available";
                            }
                            else
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Thank you for returning in time :)");
                                BookList[i].Availability = "Available";
                            }

                        }
                        else if (n == BookList[i].BookID && name != BookList[i].Availability)
                        {
                            Console.WriteLine("You don't have this book.");
                            break;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("\nPress Enter to go back.");
                    Console.ReadLine();
                    break;

                case 4:
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("Library");
                        Console.WriteLine("All books[1]");
                        Console.WriteLine("Filter by author[2]");
                        Console.WriteLine("Filter by category[3]");
                        Console.WriteLine("Filter by language[4]");
                        Console.WriteLine("Filter by ISBN[5]");
                        Console.WriteLine("Filter by name[6]");
                        Console.WriteLine("Available books[7]");
                        Console.WriteLine("Taken books[8]");
                        Console.WriteLine("Exit[0]");
                        Console.WriteLine("Choose an action by entering a number:");
                        n = Convert.ToInt32(Console.ReadLine());
                        if (n == 0)
                        {
                            break;
                        }
                        if (n == 1)
                        {
                            Console.Clear();
                            Console.WriteLine("Available books:");
                            foreach (var p in BookList)
                            {
                                if (p.Availability == "Available")
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(p.FullInfo);
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("Taken books:");
                            foreach (var p in BookList)
                            {
                                if (p.Availability != "Available")
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(p.FullInfo);
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 2)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter your chosen author's full name:");
                            name = Console.ReadLine().ToUpper();
                            if (name.Count() < 1)
                            {
                                Console.WriteLine("This is not a valid name.\nPress Enter to go back.");
                                Console.ReadLine();
                                break;
                            }
                            foreach (var p in BookList)
                            {
                                if (p.Author == name)
                                    Console.WriteLine(p.FullInfo);
                            }
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 3)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter your chosen category:");
                            name = Console.ReadLine().ToUpper();
                            if (name.Count() < 1)
                            {
                                Console.WriteLine("This is not a valid category.\nPress Enter to go back.");
                                Console.ReadLine();
                                break;
                            }
                            foreach (var p in BookList)
                            {
                                if (p.Category == name)
                                    Console.WriteLine(p.FullInfo);
                            }
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 4)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter a language:");
                            name = Console.ReadLine().ToUpper();
                            if (name.Count() < 1)
                            {
                                Console.WriteLine("This is not a valid language.\nPress Enter to go back.");
                                Console.ReadLine();
                                break;
                            }
                            foreach (var p in BookList)
                            {
                                if (p.Language == name)
                                    Console.WriteLine(p.FullInfo);
                            }
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 5)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter an ISBN (13 digits):");
                            name = Console.ReadLine();
                            if (name.Count() != 13)
                            {
                                Console.WriteLine("This is not a valid ISBN.\nPress Enter to go back.");
                                Console.ReadLine();
                                break;

                            }
                            foreach (var p in BookList)
                            {
                                if (p.ISBN == name)
                                    Console.WriteLine(p.FullInfo);
                            }
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 6)
                        {
                            Console.Clear();
                            Console.WriteLine("Enter the title of the book:");
                            name = Console.ReadLine().ToUpper();
                            if (name.Count() < 1)
                            {
                                Console.WriteLine("This is not a valid title.\nPress Enter to go back.");
                                Console.ReadLine();
                                break;
                            }
                            foreach (var p in BookList)
                            {
                                if (p.Name == name)
                                    Console.WriteLine(p.FullInfo);
                            }
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 7)
                        {
                            Console.Clear();
                            Console.WriteLine("Available books:");
                            foreach (var p in BookList)
                            {
                                if (p.Availability == "Available")
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine(p.FullInfo);
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                        else if (n == 8)
                        {
                            Console.Clear();
                            Console.WriteLine("Taken books:");
                            foreach (var p in BookList)
                            {
                                if (p.Availability != "Available")
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine(p.FullInfo);
                                }
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("\nPress Enter to go back.");
                            Console.ReadLine();
                        }
                    }
                    break;

                case 5:
                    Console.Clear();
                    Console.WriteLine("List of available books:");
                    foreach (var p in BookList)
                    {
                        if (p.Availability == "Available")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(p.FullInfo);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("To delete a book enter it's BookID:");
                    c = Convert.ToInt32(Console.ReadLine());

                    if (DeleteBook(c))
                        Console.WriteLine("Book removed...");
                    else
                        Console.WriteLine("Invalid BookID...");

                    Console.WriteLine("\nPress Enter to go back.");
                    Console.ReadLine();
                    break;



            }
        }

        public static bool AddBook(Book book)
        {
            bool result = false;
            try
            {
                if (BookList == null)
                    BookList = new List<Book>();
                BookList.Add(book);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }
        public static bool DeleteBook(int bookId)
        {
            bool result = false;
            try
            {
                var book = BookList.First(x => x.BookID == bookId);
                if (book != null && book.Availability == "Available")
                {
                    BookList.Remove(book);
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

    }
}
