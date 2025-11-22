using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

HashSet<Book> library = [];

while (true)
{
    Console.Clear();
    Console.SetCursorPosition(0, 0);

    Console.WriteLine("""
        Добро пожаловать в MyLibrary!
        Что вы хотите сделать?
         1. Показать список книг
         2. Добавить книгу
         3. Редактировать данные книги
         4. Удалить книгу
         5. Найти книгу
         6. Загрузить библиотеку из текстового файла
         7. Сохранить библиотеку в текстовый файл
         8. Выйти
        """);
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
                Console.WriteLine("Плейсхолдер найти книгу");
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

    public Book(string title, string author, ushort year, string genre, ushort amount)
    {
        Id = _nextId++;
        Title = title;
        Author = author;
        Year = year;
        Genre = genre;
        Amount = amount;
    }
}

class Library
{
    private HashSet<Book> _bookInLibrary;

    public Library(HashSet<Book> bookInLibrary) => bookInLibrary = [];

    static public void AddBook(HashSet<Book> list)
    {
        string title = "";
        string author = "";
        ushort year = 0;
        string genre = "";
        ushort amount = 0;

        Console.Clear();
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

    static public void SearchBook(HashSet<Book> list)
    {

    }
}

class FileHandler
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
                              $" Автор: {item.Author}, Год издания:" +
                              $" {item.Year}, Жанр: {item.Genre}, Кол-во: {item.Amount}");
        }
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
