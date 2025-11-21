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

    byte userInput = 0;
    UI.BasicInput(ref userInput);

    switch (userInput)
    {
        case 1:
            {
                Console.Clear();
                Console.WriteLine("Вот список всех книг в библиотеке:");
                UI.ShowList(library);
                break;
            }
        case 2:
            {    
                break;
            }
        case 3:
            {
                break;
            }
        case 4:
            {
                break;
            }
        case 5:
            {
                break;
            }
        case 6:
            {
                break;
            }
        case 7:
            {
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

    Console.WriteLine("Ожидание ввода...");
    Console.ReadKey();
}

class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public ushort Year { get; set; }
    public string Genre { get; set; }
    public ushort Amount { get; set; }

    public Book(int id, string title, string author, ushort year, string genre, ushort amount)
    {
        Id = id;
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

    public void AddBook(HashSet<Book> list)
    {

    }

    public void BorrowBook(HashSet<Book> list)
    {

    }
    public void RemoveBook(HashSet<Book> list)
    {

    }

    public void EditBook(HashSet<Book> list)
    {

    }

    public void SearchBook(HashSet<Book> list)
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

    public static void BasicInput(ref byte byteInput)
    {
        string strInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(strInput))
        {
            Console.WriteLine("Вы нажали Enter. Введите число!");
            return;
        }
        if (byte.TryParse(strInput, out byteInput)) { }
        else
        {
            Console.WriteLine("Неверный ввод!");
            return;
        }
    }
}
