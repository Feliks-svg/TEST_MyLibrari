using System.Collections.Generic;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

HashSet<Book> library = [];
Book choosenBook = null;

library.Add(new Book("1", "1", 1, "1", 1));//Debug
library.Add(new Book("2", "2", 2, "2", 2));//Debug
library.Add(new Book("3", "3", 3, "3", 3));//Debug
library.Add(new Book("4", "4", 4, "4", 4));//Debugh
library.Add(new Book("5", "5", 5, "6", 5));//Debug
library.Add(new Book("6", "5", 6, "6", 6));//Debugh
library.Add(new Book("7", "7", 6, "7", 7));//Debugh

while (true)
{
    Console.Clear();
    Console.SetCursorPosition(0, 0);

    UI.ShowMenu(ref choosenBook);
    UI.Divider();
   
    byte userInput = UI.SelectMenuOption();
    switch (userInput)
    {
        case 1:
            {
                Console.Clear();
                Console.WriteLine("Cписок всех книг в библиотеке:");
                UI.ShowList(library);
                UI.AwaitingInput();
                break;
            }
        case 2:
            {
                Console.Clear();
                Library.AddBook(library);
                UI.AwaitingInput();
                break;
            }
        case 3:
            {
                Console.Clear();
                Library.EditBook(library, ref choosenBook);
                UI.AwaitingInput();
                break;
            }
        case 4:
            {
                Console.Clear();
                Library.BorrowBookMenuOption(library, ref choosenBook);
                UI.AwaitingInput();
                break;
            }
        case 5:
            {
                Console.Clear();
                Library.RemoveBook(library, ref choosenBook);
                UI.AwaitingInput();
                break;
            }
        case 6:
            {
                Console.Clear();
                LibrarySearcher.SearchBookMenuOption(library, ref choosenBook);
                UI.AwaitingInput();
                break;
            }
        case 7:
            {
                Console.Clear();
                Console.WriteLine("Плейсхолдер загрузки из текстовика");
                UI.AwaitingInput();
                break;
            }
        case 8:
            {
                Console.Clear();
                Console.WriteLine("плейсхолдре сохранения в текстовик");
                UI.AwaitingInput();
                break;
            }
        case 9:
            {
                Console.WriteLine("Закрытие...");
                Thread.Sleep(1000);
                return;
            }
        default:
            {
                Console.WriteLine("Неверный ввод!");
                break;
            }
    }
}

class Book
{
    public int Id { get; set; }
    private static ushort _nextId = 1;
    public string Title { get; set; }
    public string Author { get; set; }
    public ushort Year { get; set; }
    public string Genre { get; set; }
    public ushort AmountOrigin { get; set; }
    public ushort AmountLeft { get; set; }
    public bool IsChoosen { get; set; }

    public Book(string title, string author, ushort year, string genre, ushort amount)
    {
        Id = _nextId++;
        Title = title;
        Author = author;
        Year = year;
        Genre = genre;
        AmountOrigin = amount;
        IsChoosen = false;
        AmountLeft = amount;
    }

    public static bool CheckIfChoosen(Book choosenBook)
    {
        if (choosenBook == null) return false;
        else return true;
    }
}

class Library
{
    static public void AddBook(HashSet<Book> list)
    {
        string title = "";
        string author = "";
        ushort year = 0;
        string genre = "";
        ushort amount = 0;

        Console.WriteLine("Введите параметры для добавления книги!");
        UI.Divider();

        Console.WriteLine("Введите название книги:");
        DataHandler.StringDataHandler(ref title);
        UI.Divider();

        Console.WriteLine("Введите имя автора:");
        DataHandler.StringDataHandler(ref author);
        UI.Divider();

        Console.WriteLine("Введите год создания:");
        DataHandler.UshortDataHandler(ref year);
        UI.Divider();

        Console.WriteLine("Введите название жанра:");
        DataHandler.StringDataHandler(ref genre);
        UI.Divider();

        Console.WriteLine("Введите количество экземпляров:");
        DataHandler.UshortDataHandler(ref amount);
        UI.Divider();

        Console.WriteLine($"Книга под названием \"{title}\" успешно создана!");
        Book newBook = new(title, author, year, genre, amount);
        list.Add(newBook);
    }

    static public void BorrowBookMenuOption(HashSet<Book> list, ref Book choosenBook)
    {
        if (Book.CheckIfChoosen(choosenBook))
        {
            UI.ShowBook(choosenBook);
            if (choosenBook.AmountLeft < choosenBook.AmountOrigin)
            {
                UI.Divider();
                Console.WriteLine($"Возможно вернуть экземпляр книги в размере {choosenBook.AmountOrigin - choosenBook.AmountLeft}!" +
                                  $" Желаете продолжить? (y/n)");
                var userInput = "";
                DataHandler.StringDataHandler(ref userInput);
                if (userInput.ToLower() == "y")
                {
                    Console.Clear();
                    ReturnBook(list, ref choosenBook);
                    return;
                }
            }

            if (choosenBook.AmountLeft > 0)
            {
                Console.Clear();
                BorrowBook(list, ref choosenBook);
                return;
            }
            else
                Console.WriteLine("Нету доступных экземпляров книги!");
        }
        else
            Console.WriteLine("Не выбрана книга для совершения действия!");
    }
    static public void BorrowBook(HashSet<Book> list, ref Book choosenBook)
    {
        if (Book.CheckIfChoosen(choosenBook))
        {
            UI.ShowBook(choosenBook);
            UI.Divider();
            Console.WriteLine("Сколько экземпляров книги вы хотите позаимствовать? (0 для отмены)");
            ushort userInput = 0;
            while (true)
            {
                
                DataHandler.UshortDataHandler(ref userInput);

                if (userInput > choosenBook.AmountLeft)
                {
                    Console.WriteLine("Невозможно взять больше книг, чем осталось в библиотеке!");
                    continue;
                }
                else if (userInput == 0)
                    break;
                else
                {
                    list.Remove(choosenBook);
                    choosenBook.AmountLeft -= userInput;
                    list.Add(choosenBook);
                    Console.WriteLine($"Успешно позаимствована книга \"{choosenBook.Title}\" в количестве {userInput}");
                    break;
                }
            }
        }
        else
            Console.WriteLine("Не выбрана книга для совершения действия!");
    }
    static public void ReturnBook(HashSet<Book> list, ref Book choosenBook)
    {
        if (Book.CheckIfChoosen(choosenBook))
        {
            UI.ShowBook(choosenBook);
            UI.Divider();
            Console.WriteLine($"Возможно вернуть {choosenBook.AmountOrigin - choosenBook.AmountLeft} экземпляров." +
                              $" Введите желаемое количество экземпляров для возврата: (0 для отмены)");
            ushort userInput = 0;
            while (true)
            {
                DataHandler.UshortDataHandler(ref userInput);

                if (userInput > choosenBook.AmountOrigin)
                {
                    Console.WriteLine("Невозможно вернуть экземпляров больше изначального количества!");
                    continue;
                }
                else if (userInput == 0)
                    break;
                else
                {
                    list.Remove(choosenBook);
                    choosenBook.AmountLeft += userInput;
                    list.Add(choosenBook);
                    Console.WriteLine($"Успешно возвращена книга \"{choosenBook.Title}\" в количестве {userInput}");
                    break;
                }    
            }
        }
        else
            Console.WriteLine("Не выбрана книга для совершения действия!");
    }
    static public void RemoveBook(HashSet<Book> list, ref Book choosenBook)
    {
        if (Book.CheckIfChoosen(choosenBook))
        {
            UI.ShowBook(choosenBook);
            UI.Divider();
            Console.WriteLine("Вы действительно хотите удалить выбранную книгу? (y/n)");
            string userInput = "";
            DataHandler.StringDataHandler(ref userInput);
            if (userInput.ToLower() != "y")
                return;
            else
            {
                list.Remove(choosenBook);
                Console.WriteLine($"Книга под названием \"{choosenBook.Title}\" успешно удалена!");
                choosenBook = null;
            }
        }
        else
            Console.WriteLine("Не выбрана книга для совершения действия!");
    }

    static public void EditBook(HashSet<Book> list, ref Book choosenBook)
    {
        string userStrInput = "";
        if (Book.CheckIfChoosen(choosenBook))
        {
            UI.ShowChoosenBook(choosenBook);
            Console.WriteLine("""
                Какой параметр книги вы хотите отредактировать?
                1. Название
                2. Автор
                3. Год
                4. Жанр
                """);
            UI.Divider();

            ushort userInput = UI.SelectMenuOption();
            switch (userInput)
            {
                case 1:
                    Console.WriteLine("Введите новое название:");
                    DataHandler.StringDataHandler(ref userStrInput);
                    list.Remove(choosenBook);
                    choosenBook.Title = userStrInput;
                    list.Add(choosenBook);
                    Console.WriteLine($"Название успешно заменено на \"{choosenBook.Title}\"");
                    break;
                case 2:
                    Console.WriteLine("Введите нового автора:");
                    DataHandler.StringDataHandler(ref userStrInput);
                    list.Remove(choosenBook);
                    choosenBook.Author = userStrInput;
                    list.Add(choosenBook);
                    Console.WriteLine($"Автор успешно замененён на \"{choosenBook.Author}\"");
                    break;
                case 3:
                    Console.WriteLine("Введите новый год:");
                    DataHandler.UshortDataHandler(ref userInput);
                    list.Remove(choosenBook);
                    choosenBook.Year = userInput;
                    list.Add(choosenBook);
                    Console.WriteLine($"Год успешно заменён на\"{choosenBook.Year}\"");
                    break;
                case 4:
                    Console.WriteLine("Введите новый жанр:");
                    DataHandler.StringDataHandler(ref userStrInput);
                    list.Remove(choosenBook);
                    choosenBook.Genre = userStrInput;
                    list.Add(choosenBook);
                    Console.WriteLine($"Жанр успешно замененён на \"{choosenBook.Genre}\"");
                    break;
            }
        }
        else
            Console.WriteLine("Не выбрана книга для совершения действия!");
    }
}

class LibrarySearcher
{
    public static void SearchBookMenuOption(HashSet<Book> list, ref Book choosenBook)
    {
        Console.WriteLine("""
            Выберите параметр, по которому вы хотите найти желаемую книгу:
            1. По ID
            2. По названию
            3. По автору
            4. По году
            5. По жанру
            """);

        byte userInput = UI.SelectMenuOption();
        switch (userInput)
        {
            case 1:
                Console.Clear();
                SearchById(list, ref choosenBook);
                break;
            case 2:
                Console.Clear();
                SearchByTitle(list, ref choosenBook);
                break;
            case 3:
                Console.Clear();
                SearchByAuthor(list, ref choosenBook);
                break;
            case 4:
                Console.Clear();
                SearchByYear(list, ref choosenBook);
                break;
            case 5:
                Console.Clear();
                SearchByGenre(list, ref choosenBook);
                break;
            default:
                Console.WriteLine("Неверный ввод!");
                break;
        }
    }
    public static void SearchById(HashSet<Book> list, ref Book choosenBook)
    {
        while (true)
        {
            Console.WriteLine("Введите идентификатор желаемой книги: ");
            int userInput;
            if (int.TryParse(Console.ReadLine(), out userInput))
            {
                Book foundBook = list.FirstOrDefault(b => b.Id == userInput);

                if (foundBook != null)
                {
                    UI.ShowBook(foundBook);
                    UI.Divider();
                    DataHandler.BookSelecter(foundBook, ref choosenBook);
                    return;
                }
                else
                    Console.WriteLine($"Не удалось найти книгу под идентификатором {userInput}");
            }
            else
                Console.WriteLine("Неверный ввод!");
        }
    }
    public static void SearchByTitle(HashSet<Book> list, ref Book choosenBook)
    {
        while (true)
        {
            Console.WriteLine("Введите название желаемой книги: ");
            string userInput = "";
            DataHandler.StringDataHandler(ref userInput);

            Book foundBook = list.FirstOrDefault(b => b.Title == userInput);

            if (foundBook != null)
            {
                UI.ShowBook(foundBook);
                UI.Divider();
                DataHandler.BookSelecter(foundBook, ref choosenBook);
                return;
            }
            else
                Console.WriteLine($"Не удалось найти книгу под названием {userInput}");
        }
    }                
    public static void SearchByAuthor(HashSet<Book> list, ref Book choosenBook)
    {
        while(true)
        {
            Console.WriteLine("Введите имя и фамилию автора:");
            string userStrInput = "";
            DataHandler.StringDataHandler(ref userStrInput);

            HashSet<Book> foundBooks = list.Where(b => b.Author == userStrInput).ToHashSet();

            if (foundBooks != null)
                UI.ShowList(foundBooks);
            else
                Console.WriteLine($"Не удалось ни одной книги за авторством \"{userStrInput}\"");

            UI.Divider();

            if (foundBooks.Count == 1 && foundBooks != null)
            {
                DataHandler.BookSelecterHashSet(foundBooks, ref choosenBook);
                return;
            }
            else if (foundBooks.Count > 1 && foundBooks != null)
            {
                DataHandler.BookSelecterMultiple(foundBooks, ref choosenBook);
                return;
            }
        }
    }
    public static void SearchByYear(HashSet<Book> list, ref Book choosenBook)
    {
        while (true)
        {
            Console.WriteLine("Введите год книги:");
            ushort userInput = 0;
            DataHandler.UshortDataHandler(ref userInput);

            HashSet<Book> foundBooks = list.Where(b => b.Year == userInput).ToHashSet();

            if (foundBooks != null)
                UI.ShowList(foundBooks);
            else
                Console.WriteLine($"Не удалось найти ни одной книги за авторством \"{userInput}\"");

            UI.Divider();

            if (foundBooks.Count == 1 && foundBooks != null)
            {
                DataHandler.BookSelecterHashSet(foundBooks, ref choosenBook);
                return;
            }
            else if (foundBooks.Count > 1 && foundBooks != null)
            {
                DataHandler.BookSelecterMultiple(foundBooks, ref choosenBook);
                return;
            }
        }
    }
    public static void SearchByGenre(HashSet<Book> list, ref Book choosenBook)
    {
        while (true)
        {
            Console.WriteLine("Введите жанр книги:");
            string userInput = "";
            DataHandler.StringDataHandler(ref userInput);

            HashSet<Book> foundBooks = list.Where(b => b.Genre == userInput).ToHashSet();

            if (foundBooks != null)
                UI.ShowList(foundBooks);
            else
                Console.WriteLine($"Не удалось найти ни одной книги за авторством \"{userInput}\"");

            UI.Divider();

            if (foundBooks.Count == 1 && foundBooks != null)
            {
                DataHandler.BookSelecterHashSet(foundBooks, ref choosenBook);
                return;
            }
            else if (foundBooks.Count > 1 && foundBooks != null)
            {
                DataHandler.BookSelecterMultiple(foundBooks, ref choosenBook);
                return;
            }
        }
    }
}

class LibraryFileHandler
{
    public void ReadTextFile(string path, HashSet<Book> list)
    {

    }
}

static class UI
{
    public static byte SelectMenuOption()
    {
        while (true)
        {
            var keyInfo = Console.ReadKey(true);
            char inputChar = keyInfo.KeyChar;

            if (char.IsDigit(inputChar))
            {
                byte result = byte.Parse(inputChar.ToString());
                return result;
            }
            else
            {
                Console.WriteLine("\nНеверный выбор. Введите цифру.");
            }
        }
    }
    public static void ShowList(HashSet<Book> list)
    {
        foreach (Book item in list.OrderBy(i => i.Id))
        {
            Console.WriteLine($"Книга {item.Id}: {item.Title}," +
                              $" Автор: {item.Author}, Год:" +
                              $" {item.Year}, Жанр: {item.Genre}, Кол-во: {item.AmountLeft}/{item.AmountOrigin}");
        }
    }

    public static void ShowBook(Book item)
    {
        Console.WriteLine($"Книга {item.Id}: {item.Title}," +
                              $" Автор: {item.Author}, Год:" +
                              $" {item.Year}, Жанр: {item.Genre}, Кол-во: {item.AmountLeft}/{item.AmountOrigin}");
    }
    public static void Divider()
    {
        Console.WriteLine("------------------------------");
    }

    public static void AwaitingInput()
    {
        Console.WriteLine("Ожидание ввода...");
        Console.ReadKey();
    }

    public static void ShowChoosenBook(Book book)
    {
        if ((book.IsChoosen == true) && book != null)
            Console.WriteLine($"Выбрана книга: \"{book.Title}\"");
    }
    public static void ShowMenu(ref Book choosenBook)
    {
        Console.WriteLine("""
            Добро пожаловать в MyLibrary!
            Что вы хотите сделать?
             1. Показать список книг
             2. Добавить книгу
             3. Редактировать данные выбранной книги
             4. Взять/Вернуть выбранную книгу
             5. Удалить выбранную книгу
             6. Найти и выбрать книгу
             7. Загрузить библиотеку из текстового файла
             8. Сохранить библиотеку в текстовый файл
             9. Выйти
            """);
        if (choosenBook != null)
            UI.ShowChoosenBook(choosenBook);
    }
}

static class DataHandler
{
    static public void StringDataHandler(ref string input)
    {
        while (true)
        {
            input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Входные данные не могут быть пустыми!");
                continue;
            }
            if (!input.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            {
                Console.WriteLine("Входные данные могут содержать только буквы и цифры!");
                continue;
            }
            break;
        }
    }

    public static void UshortDataHandler(ref ushort input)
    {
        while (true)
        {
            try
            {
                input = Convert.ToUInt16(Console.ReadLine());
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Введите корректное числовое значение!");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Значение слишком большое или слишком маленькое!");
            }
        }
    }
    public static void BookSelecter(Book foundBook, ref Book choosenBook)
    {
        string userInput = "";
        Console.WriteLine("Хотите выбрать найденную книгу для дальнейшего взаимодействия? (y/n)");
        DataHandler.StringDataHandler(ref userInput);
        if (userInput.ToLower() != "y")
            return;
        else
        {
            choosenBook = foundBook;
            choosenBook.IsChoosen = true;
            Console.WriteLine($"Кинга \"{choosenBook.Title}\" успешно выбрана!");
        }
    }
    public static void BookSelecterHashSet(HashSet<Book> foundBooks, ref Book choosenBook)
    {
        string userInput = "";
        Console.WriteLine("Хотите выбрать найденную книгу для дальнейшего взаимодействия? (y/n)");
        DataHandler.StringDataHandler(ref userInput);
        if (userInput.ToLower() != "y")
            return;
        else
        {
            choosenBook = foundBooks.FirstOrDefault();
            choosenBook.IsChoosen = true;
            Console.WriteLine($"Кинга \"{choosenBook.Title}\" успешно выбрана!");
        }
    }

    public static void BookSelecterMultiple(HashSet<Book> foundBooks, ref Book choosenBook)
    {
        while (true)
        {
            Console.WriteLine("Введите идентификтор книги, которую вы хотите выбрать (0 для завершения): ");
            int userInput;
            if (int.TryParse(Console.ReadLine(), out userInput))
            {
                if (userInput != 0)
                {
                    choosenBook = foundBooks.FirstOrDefault(b => b.Id == userInput);
                    if (choosenBook != null)
                    {
                        choosenBook.IsChoosen = true;
                        Console.WriteLine($"Книга \"{choosenBook.Title}\" успешно выбрана!");
                    }
                    else
                    {
                        Console.WriteLine("Нету книги с указанным идентификатором!");
                    }
                }
                else
                    return;
            }
            else
                Console.WriteLine("Невернный ввод!");
        }
    }
}

static class Loc
{
    private static string _lang = "ru"; // "ru" or "en"

    private static readonly Dictionary<string, (string ru, string en)> _dict = new()
    {
        {"invalid_choice_digit", ("\nНеверный выбор. Введите цифру.", "\nInvalid choice. Enter a digit.")},
        {"menu_text", (
            // RU
@"Добро пожаловать в MyLibrary!
Что вы хотите сделать?
 1. Показать список книг
 2. Добавить книгу
 3. Редактировать данные выбранной книги
 4. Взять/Вернуть выбранную книгу
 5. Удалить выбранную книгу
 6. Найти и выбрать книгу
 7. Загрузить библиотеку из текстового файла
 8. Сохранить библиотеку в текстовый файл
 9. Выйти",
            // EN
@"Welcome to MyLibrary!
What do you want to do?
 1. Show list of books
 2. Add a book
 3. Edit selected book data
 4. Borrow/Return selected book
 5. Remove selected book
 6. Find and select a book
 7. Load library from text file
 8. Save library to text file
 9. Exit"
        )},
        {"list_all_books", ("Cписок всех книг в библиотеке:", "List of all books in the library:")},
        {"enter_params", ("Введите параметры для добавления книги!", "Enter parameters to add a book!")},
        {"enter_title", ("Введите название книги:", "Enter book title:")},
        {"enter_author", ("Введите имя автора:", "Enter author name:")},
        {"enter_year", ("Введите год создания:", "Enter year of publication:")},
        {"enter_genre", ("Введите название жанра:", "Enter genre name:")},
        {"enter_amount", ("Введите количество экземпляров:", "Enter number of copies:")},
        {"book_created", ("Книга под названием \"{0}\" успешно создана!", "Book titled \"{0}\" created successfully!")},
        {"return_possible", ("Возможно вернуть экземпляр книги в размере {0}! Желаете продолжить? (y/n)", "You can return up to {0} copies! Continue? (y/n)")},
        {"no_copies_available", ("Нету доступных экземпляров книги!", "No copies available!")},
        {"no_book_selected", ("Не выбрана книга для совершения действия!", "No book selected for action!")},
        {"how_many_borrow", ("Сколько экземпляров книги вы хотите позаимствовать? (0 для отмены)", "How many copies would you like to borrow? (0 to cancel)")},
        {"cannot_borrow_more", ("Невозможно взять больше книг, чем осталось в библиотеке!", "Cannot borrow more copies than available!")},
        {"success_borrowed", ("Успешно позаимствована книга \"{0}\" в количестве {1}", "Successfully borrowed \"{0}\" in amount {1}")},
        {"possible_return", ("Возможно вернуть {0} экземпляров. Введите желаемое количество экземпляров для возврата: (0 для отмены)", "You can return up to {0} copies. Enter amount to return: (0 to cancel)")},
        {"cannot_return_more", ("Невозможно вернуть экземпляров больше изначального количества!", "Cannot return more than original amount!")},
        {"success_returned", ("Успешно возвращена книга \"{0}\" в количестве {1}", "Successfully returned \"{0}\" amount {1}")},
        {"confirm_delete", ("Вы действительно хотите удалить выбранную книгу? (y/n)", "Do you really want to delete the selected book? (y/n)")},
        {"success_deleted", ("Книга под названием \"{0}\" успешно удалена!", "Book titled \"{0}\" successfully deleted!")},
        {"edit_prompt", (
@"Какой параметр книги вы хотите отредактировать?
1. Название
2. Автор
3. Год
4. Жанр",
@"Which parameter do you want to edit?
1. Title
2. Author
3. Year
4. Genre"
        )},
        {"enter_new_title", ("Введите новое название:", "Enter new title:")},
        {"title_changed", ("Название успешно заменено на \"{0}\"", "Title changed to \"{0}\"")},
        {"enter_new_author", ("Введите нового автора:", "Enter new author:")},
        {"author_changed", ("Автор успешно замененён на \"{0}\"", "Author changed to \"{0}\"")},
        {"enter_new_year", ("Введите новый год:", "Enter new year:")},
        {"year_changed", ("Год успешно заменён на\"{0}\"", "Year changed to \"{0}\"")},
        {"enter_new_genre", ("Введите новый жанр:", "Enter new genre:")},
        {"genre_changed", ("Жанр успешно замененён на \"{0}\"", "Genre changed to \"{0}\"")},
        {"search_choice", (
@"Выберите параметр, по которому вы хотите найти желаемую книгу:
1. По ID
2. По названию
3. По автору
4. По году
5. По жанру",
@"Select parameter to search by:
1. By ID
2. By title
3. By author
4. By year
5. By genre"
        )},
        {"enter_id", ("Введите идентификатор желаемой книги: ", "Enter desired book ID: ")},
        {"not_found_id", ("Не удалось найти книгу под идентификатором {0}", "Could not find a book with ID {0}")},
        {"enter_title_search", ("Введите название желаемой книги: ", "Enter desired book title: ")},
        {"not_found_title", ("Не удалось найти книгу под названием {0}", "Could not find a book titled {0}")},
        {"enter_author_search", ("Введите имя и фамилию автора:", "Enter author name:")},
        {"no_books_by_author", ("Не удалось ни одной книги за авторством \"{0}\"", "No books by author \"{0}\"")},
        {"enter_year_search", ("Введите год книги:", "Enter book year:")},
        {"no_books_by_year", ("Не удалось найти ни одной книги за год {0}", "No books found for year {0}")},
        {"enter_genre_search", ("Введите жанр книги:", "Enter genre:")},
        {"no_books_by_genre", ("Не удалось найти ни одной книги по жанру \"{0}\"", "No books found for genre \"{0}\"")},
        {"invalid_input", ("Неверный ввод!", "Invalid input!") },
        {"press_any_key", ("Ожидание ввода...", "Press any key to continue...")},
        {"show_chosen_book", ("Выбрана книга: \"{0}\"", "Selected book: \"{0}\"")},
        {"want_select_found", ("Хотите выбрать найденную книгу для дальнейшего взаимодействия? (y/n)", "Do you want to select the found book for further actions? (y/n)")},
        {"book_selected_success", ("Книга \"{0}\" успешно выбрана!", "Book \"{0}\" successfully selected!")},
        {"enter_id_to_select", ("Введите идентификтор книги, которую вы хотите выбрать (0 для завершения): ", "Enter ID of the book you want to select (0 to finish): ")},
        {"no_book_with_id", ("Нету книги с указанным идентификатором!", "No book with specified ID!")},
        {"invalid_number", ("Введите корректное числовое значение!", "Enter a valid numeric value!")},
        {"overflow_number", ("Значение слишком большое или слишком маленькое!", "Value too large or too small!")},
        {"empty_input", ("Входные данные не могут быть пустыми!", "Input cannot be empty!")},
        {"only_letters_digits", ("Входные данные могут содержать только буквы и цифры!", "Input may contain only letters, digits and spaces!")},
        {"goodbye", ("Закрытие...", "Closing...")},
        {"placeholder_load", ("Плейсхолдер загрузки из текстовика", "Placeholder for loading from a text file")},
        {"placeholder_save", ("плейсхолдре сохранения в текстовик", "Placeholder for saving to a text file")},
        // Format for displaying book info: {0}=Id {1}=Title {2}=Author {3}=Year {4}=Genre {5}=Left {6}=Origin
        {"book_line_format", ("Книга {0}: {1}, Автор: {2}, Год: {3}, Жанр: {4}, Кол-во: {5}/{6}", "Book {0}: {1}, Author: {2}, Year: {3}, Genre: {4}, Amount: {5}/{6}")},
    };

    public static void SetLanguage(string code)
    {
        if (code == "en") _lang = "en";
        else _lang = "ru";
    }

    public static string T(string key)
    {
        if (_dict.TryGetValue(key, out var pair))
        {
            return _lang == "en" ? pair.en : pair.ru;
        }
        return key;
    }
}