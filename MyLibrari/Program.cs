using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

HashSet<Book> library = [];
Book choosenBook = null;

library.Add(new Book("1", "1", 1, "1", 1));//Debug
library.Add(new Book("2", "2", 2, "2", 2));//Debug
library.Add(new Book("3", "3", 3, "3", 3));//Debug
library.Add(new Book("4", "4", 4, "4", 4));//Debug

while (true)
{
        Console.Clear();
        Console.SetCursorPosition(0, 0);

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
                Console.WriteLine($"Перед поиском: choosenBook = {(choosenBook == null ? "null" : choosenBook.Id.ToString())}");
                LibrarySearcher.SearchBook(library, ref choosenBook);
                Console.WriteLine($"После поиска: choosenBook = {(choosenBook == null ? "null" : choosenBook.Id.ToString())}");
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

    public bool IsChoosenMethod()
    {
        if (IsChoosen == true) return true;
        else return false;
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
        DataHandler.StringDataAdder(ref title);
        UI.Divider();

        Console.WriteLine("Введите имя автора:");
        DataHandler.StringDataAdder(ref author);
        UI.Divider();

        Console.WriteLine("Введите год создания:");
        DataHandler.UshortDataAdder(ref year);
        UI.Divider();

        Console.WriteLine("Введите название жанра:");
        DataHandler.StringDataAdder(ref genre);
        UI.Divider();

        Console.WriteLine("Введите количество экземпляров:");
        DataHandler.UshortDataAdder(ref amount);
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
                SearchByTitle(list);
                break;
            case 3:
                Console.Clear();
                SearchByAuthor(list);
                break;
            case 4:
                Console.Clear();
                SearchByYear(list);
                break;
            case 5:
                Console.Clear();
                SearchByGenre(list);
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
                    string userStrInput = Console.ReadLine();
                    if (userStrInput.ToLower() != "y")
                        break;
                    else
                    {
                        choosenBook = foundBook;
                        Console.WriteLine($"Кинга \"{choosenBook.Title}\" успешно выбрана!");
                        break;
                    }
                }
                else
                    Console.WriteLine($"Не удалось найти книгу под идентификатором {userInput}");
            }
            else
                Console.WriteLine("Неверный ввод!");
        }
    }
        
    public static void SearchByTitle(HashSet<Book> list)
    {

    }
    public static void SearchByAuthor(HashSet<Book> list)
    {

    }
    public static void SearchByYear(HashSet<Book> list)
    {

    }
    public static void SearchByGenre(HashSet<Book> list)
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
                              $" Автор: {item.Author}, Год издания:" +
                              $" {item.Year}, Жанр: {item.Genre}, Кол-во: {item.Amount}");
        }
    }

    public static void ShowBook(Book item)
    {
        Console.WriteLine($"Книга {item.Id}: {item.Title}," +
                              $" Автор: {item.Author}, Год издания:" +
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
}

static class DataHandler
{
    static public void StringDataAdder(ref string input)
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

    public static void UshortDataAdder(ref ushort input)
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
}
