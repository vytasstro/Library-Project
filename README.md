# Library-Project

Functional console application that acts as a library.

Options for user:
- Add a new book
- Borrow a book
- Return a book
- List of books
- Delete a book from the list
- EXIT from the application

Features:
- When adding a new book program validates the Publication Date(can not be earlier than today).
- When adding a new book program validates the ISBN (has to be 13 characters).
- When trying to borrow a book the program presents the list of books (available books are presented in green and not available books are presented in red texts).
- When trying to borrow a book the program validates if the chosen book is available.
- When trying to borrow a book the program validates if the library user can borrow more books (no more than 3 books are allowed).
- When returning a book the program presents a list of rented books by user first name and last name
- When returning a book the program dectects if the book is late for return ant prints out a funny message.
- Filter list of books by author, category, language, ISBN, name, available books or taken books.
- When trying to delete a book the program validates if the entered ISBN of a book is present in the list.
- Unit tests (AddBookTest() and DeleteBookTest())
- Program saves the information of the last session into a .json file, so it can be closed in between sessions.
