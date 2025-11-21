List<Book> library = [];

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
         6. Выйти
        """);
    UI.Divider();

    string input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Вы нажали Enter. Введите число!");
        continue;
    }
    if (byte.TryParse(input, out byte userInput)) {}
    else
    {
        Console.WriteLine("Неверный ввод!");
        continue;
    }

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
    public List<Book> bookInLibrary;

    public Library(List<Book> bookInLibrary) => bookInLibrary = [];

    public void AddBook(List<Book> list)
    {

    }

    public void BorrowBook(List<Book> list)
    {

    }
    public void RemoveBook(List<Book> list)
    {

    }

    public void EditBook(List<Book> list)
    {

    }

    public void SearchBook(List<Book> list)
    {

    }
}

static class UI
{
    public static void ShowList(List<Book> list)
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
}
