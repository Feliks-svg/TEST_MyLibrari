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
                Console.WriteLine("Вот список всех книг в библиотеке:");
                UI.ShowList(library);
                UI.Divider();
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
                Library.EditBook(library);
                UI.AwaitingInput();
                break;
            }
        case 4:
            {
                Library.RemoveBook(library);
                UI.AwaitingInput();
                break;
            }
        case 5:
            {
                Console.Clear();
                LibrarySearcher.SearchBook(library, ref choosenBook);
                UI.AwaitingInput();
                break;
            }
        case 6:
            {
                Console.WriteLine("Плейсхолдер загрузки из текстовика");
                break;
            }
        case 7:
            {
                Console.WriteLine("плейсхолдре сохранения в текстовик");
                break;
            }
        case 8:
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
    public ushort Amount { get; set; }
    public bool IsChoosen { get; set; }

    public Book(string title, string author, ushort year, string genre, ushort amount)
    {
        Id = _nextId++;
        Title = title;
        Author = author;
        Year = year;
        Genre = genre;
        Amount = amount;
        IsChoosen = false;
    }
}

class Library
{
    private HashSet<Book> _bookInLibrary;

    public Library()
    {
        _bookInLibrary = new HashSet<Book>();
    }

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
        Book newBook = new Book(title, author, year, genre, amount);
        list.Add(newBook);
    }

    static public void BorrowBook(HashSet<Book> list)
    {

    }
    static public void RemoveBook(HashSet<Book> list)
    {

    }

    static public void EditBook(HashSet<Book> list)
    {

    }
}

class LibrarySearcher
{
    public static void SearchBook(HashSet<Book> list, ref Book choosenBook)
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
                    Console.WriteLine("Хотите выбрать найденную книгу для дальнейшего взаимодействия? (y/n)");
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
                Console.WriteLine($"Не удалось ни одной книги за авторством \"{userInput}\"");

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
                AwaitingInput();
            }
        }
    }
    public static void ShowList(HashSet<Book> list)
    {
        foreach (Book item in list.OrderBy(i => i.Id))
        {
            Console.WriteLine($"Книга {item.Id}: {item.Title}," +
                              $" Автор: {item.Author}, Год:" +
                              $" {item.Year}, Жанр: {item.Genre}, Кол-во: {item.Amount}");
        }
    }

    public static void ShowBook(Book item)
    {
        Console.WriteLine($"Книга {item.Id}: {item.Title}," +
                              $" Автор: {item.Author}, Год:" +
                              $" {item.Year}, Жанр: {item.Genre}, Кол-во: {item.Amount}");
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
             4. Удалить выбранную книгу
             5. Найти и выбрать книгу
             6. Загрузить библиотеку из текстового файла
             7. Сохранить библиотеку в текстовый файл
             8. Выйти
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
                Console.WriteLine("Невернный ввод!");
        }
    }
}
