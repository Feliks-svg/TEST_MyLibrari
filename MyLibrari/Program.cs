using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

//i hate myself

HashSet<Book> library = [];
Book choosenBook = null;

library.Add(new Book("1", "1", 1, "1", 1));//Debug
library.Add(new Book("2", "2", 2, "2", 2));//Debug
library.Add(new Book("3", "3", 3, "3", 3));//Debug
library.Add(new Book("4", "4", 4, "4", 4));//Debugh
library.Add(new Book("5", "5", 5, "6", 5));//Debug
library.Add(new Book("6", "5", 6, "6", 6));//Debugh
library.Add(new Book("7", "7", 6, "7", 7));//Debugh

UI.SelectLanguage();

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
                LibraryFileHandler.LoadFromJson("library.json", library);
                choosenBook = null;
                UI.AwaitingInput();
                break;
            }
        case 8:
            {
                Console.Clear();
                LibraryFileHandler.SaveToJson("library.json", library);
                choosenBook = null;
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
    public uint Id { get; set; }
    private static uint _nextId = 1;
    public string Title { get; set; }
    public string Author { get; set; }
    public ushort Year { get; set; }
    public string Genre { get; set; }
    public ushort AmountOrigin { get; set; }
    public ushort AmountLeft { get; set; }
    public bool IsChoosen { get; set; }

    public Book()
    {

    }

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

    public bool Equals(Book other)
    {
        if (other is null) return false;
        return this.Id == other.Id;
    }

    public override bool Equals(object obj) => Equals(obj as Book);
    public override int GetHashCode() => Id.GetHashCode();

    public static void RecalculateNextId(IEnumerable<Book> books)
    {
        uint max = 0;
        if (books != null && books.Any())
            max = books.Max(b => b.Id);
        _nextId = Math.Max(1, max + 1);
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
            6. Выйти
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
            case 6:
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
    public static void LoadFromJson(string path, HashSet<Book> list)
    {
        Console.WriteLine("Внимание! При загрузке библиотеке будет ПОЛНОСТЬЮ ПЕРЕСЕНО ЕЁ СОСТОЯНИЕ. Несохранённые данные будут УНИЧТОЖЕНЫ! Продолжить? (y/n)");
        string userInput = "";
        DataHandler.StringDataHandler(ref userInput);
        if (userInput.ToLower() != "y")
            return;
        try
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Путь к файлу не найден в:{path}");
                return;
            }

            var json = File.ReadAllText(path);
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var books = JsonSerializer.Deserialize<List<Book>>(json, options);
            if (books == null)
            {
                Console.WriteLine("Файл пуст или повреждён!");
                return;
            }

            list.Clear();
            foreach (var book in books)
                list.Add(book);

            Book.RecalculateNextId(list);

            Console.WriteLine($"Успешно загружено {list.Count} книг из файла!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке: {ex.Message}");
        }
    }

    public static void SaveToJson(string path, HashSet<Book> list)
    {
        Console.WriteLine("Будет полностью сохранено состояние текущей библиотеки. Продолжить? (y/n)");
        string userInput = "";
        DataHandler.StringDataHandler(ref userInput);
        if (userInput != "y")
            return;
        try
        {
            JsonSerializerOptions option = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };  

            var books = list.OrderBy(b =>  b.Id).ToList();
            var json = JsonSerializer.Serialize(books, option);
            File.WriteAllText(path, json);

            Console.WriteLine($"Успешно сохранено {books.Count} книг в файл!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
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
             7. Загрузить библиотеку из JSON файла
             8. Сохранить библиотеку в JSON файл
             9. Выйти
            """);
        if (choosenBook != null)
            UI.ShowChoosenBook(choosenBook);
    }

    public static void SelectLanguage()
    {
        Console.WriteLine("Please, select language (1) | Пожалуйста, выберите язык (2):");
        byte userInput = UI.SelectMenuOption();
        if (userInput == 2) Loc.SetLanguage("en");
        else Loc.SetLanguage("ru");
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

class Loc
{
    private static string _lang = "en";

    private static readonly Dictionary<string, (string ru, string en)> _dict = new()
    {
         { "menu.welcome", (
            "Добро пожаловать в MyLibrary!\nЧто вы хотите сделать?\n 1. Показать список книг\n 2. Добавить книгу\n 3. Редактировать данные выбранной книги\n 4. Взять/Вернуть выбранную книгу\n 5. Удалить выбранную книгу\n 6. Найти и выбрать книгу\n 7. Загрузить библиотеку из JSON файла\n 8. Сохранить библиотеку в JSON файл\n 9. Выйти",
            "Welcome to MyLibrary!\nWhat would you like to do?\n 1. Show book list\n 2. Add a book\n 3. Edit selected book\n 4. Borrow/Return selected book\n 5. Remove selected book\n 6. Find and select a book\n 7. Load library from JSON file\n 8. Save library to JSON file\n 9. Exit") },

        { "select.language", ("Пожалуйста, выберите язык (1)/Please, select language (2):", "Please, select language (1)/Пожалуйста, выберите язык (2):") },
        { "prompt.invalid_digit", ("Неверный выбор. Введите цифру.", "Invalid choice. Enter a digit.") },
        { "list.header", ("Cписок всех книг в библиотеке:", "List of all books in the library:") },
        { "book.format", ("Книга {0}: {1}, Автор: {2}, Год: {3}, Жанр: {4}, Кол-во: {5}/{6}", "Book {0}: {1}, Author: {2}, Year: {3}, Genre: {4}, Qty: {5}/{6}") },
        { "divider", ("------------------------------", "------------------------------") },
        { "awaiting.input", ("Ожидание ввода...", "Awaiting input...") },
        { "show.selected", ("Выбрана книга: \"{0}\"", "Selected book: \"{0}\"") },
        { "exiting", ("Закрытие...", "Closing...") },
        { "invalid.input", ("Неверный ввод!", "Invalid input!") },

        { "input.cannot_empty", ("Входные данные не могут быть пустыми!", "Input cannot be empty!") },
        { "input.only_letters_digits", ("Входные данные могут содержать только буквы и цифры!", "Input may only contain letters and digits!") },
        { "enter.correct_number", ("Введите корректное числовое значение!", "Enter a valid numeric value!") },
        { "value.out_of_range", ("Значение слишком большое или слишком маленькое!", "Value too large or too small!") },
        { "choose.book.for_action", ("Не выбрана книга для совершения действия!", "No book selected for the action!") },
        { "no_copies_available", ("Нету доступных экземпляров книги!", "No available copies of the book!") },
        { "borrow.request_amount", ("Сколько экземпляров книги вы хотите позаимствовать? (0 для отмены)", "How many copies would you like to borrow? (0 to cancel)") },
        { "cannot_borrow_more", ("Невозможно взять больше книг, чем осталось в библиотеке!", "Cannot borrow more books than are available!") },
        { "success.borrowed", ("Успешно позаимствована книга \"{0}\" в количестве {1}", "Successfully borrowed book \"{0}\" in quantity {1}") },
        { "return.prompt", ("Возможно вернуть экземпляр книги в размере {0}! Желаете продолжить? (y/n)", "You can return {0} copies! Continue? (y/n)") },
        { "return.possible", ("Возможно вернуть {0} экземпляров. Введите желаемое количество экземпляров для возврата: (0 для отмены)", "You can return {0} copies. Enter the number to return: (0 to cancel)") },
        { "cannot_return_more", ("Невозможно вернуть экземпляров больше изначального количества!", "Cannot return more copies than the original amount!") },
        { "success.returned", ("Успешно возвращена книга \"{0}\" в количестве {1}", "Successfully returned book \"{0}\" in quantity {1}") },

        { "confirm.delete", ("Вы действительно хотите удалить выбранную книгу? (y/n)", "Do you really want to delete the selected book? (y/n)") },
        { "success.deleted", ("Книга под названием \"{0}\" успешно удалена!", "Book titled \"{0}\" was successfully deleted!") },

        { "edit.prompt", ("Какой параметр книги вы хотите отредактировать?\n1. Название\n2. Автор\n3. Год\n4. Жанр", "Which book parameter do you want to edit?\n1. Title\n2. Author\n3. Year\n4. Genre") },
        { "enter.new.title", ("Введите новое название:", "Enter new title:") },
        { "enter.new.author", ("Введите нового автора:", "Enter new author:") },
        { "enter.new.year", ("Введите новый год:", "Enter new year:") },
        { "enter.new.genre", ("Введите новый жанр:", "Enter new genre:") },
        { "success.title_changed", ("Название успешно заменено на \"{0}\"", "Title successfully changed to \"{0}\"") },
        { "success.author_changed", ("Автор успешно замененён на \"{0}\"", "Author successfully changed to \"{0}\"") },
        { "success.year_changed", ("Год успешно заменён на\"{0}\"", "Year successfully changed to \"{0}\"") },
        { "success.genre_changed", ("Жанр успешно замененён на \"{0}\"", "Genre successfully changed to \"{0}\"") },

        { "search.by.parameter", ("Выберите параметр, по которому вы хотите найти желаемую книгу:\n1. По ID\n2. По названию\n3. По автору\n4. По году\n5. По жанру\n6. Выйти", "Choose parameter to search by:\n1. By ID\n2. By title\n3. By author\n4. By year\n5. By genre\n6. Exit") },
        { "enter.id", ("Введите идентификатор желаемой книги: ", "Enter the ID of the desired book: ") },
        { "book.not_found_id", ("Не удалось найти книгу под идентификатором {0}", "Could not find a book with ID {0}") },
        { "enter.title", ("Введите название желаемой книги: ", "Enter the title of the desired book: ") },
        { "book.not_found_title", ("Не удалось найти книгу под названием {0}", "Could not find a book titled {0}") },
        { "enter.author", ("Введите имя и фамилию автора:", "Enter the author's name:") },
        { "no_books_by_author", ("Не удалось ни одной книги за авторством \"{0}\"", "No books found by author \"{0}\"") },
        { "enter.year", ("Введите год книги:", "Enter the book year:") },
        { "no_books_by_year", ("Не удалось найти ни одной книги за авторством \"{0}\"", "No books found for year \"{0}\"") },
        { "enter.genre", ("Введите жанр книги:", "Enter book genre:") },
        { "no_books_by_genre", ("Не удалось найти ни одной книги за авторством \"{0}\"", "No books found for genre \"{0}\"") },

        { "load.confirmation", ("Внимание! При загрузке библиотеке будет ПОЛНОСТЬЮ ПЕРЕСЕНО ЕЁ СОСТОЯНИЕ. Несохранённые данные будут УНИЧТОЖЕНЫ! Продолжить? (y/n)", "Warning! Loading will FULLY REPLACE the library state. Unsaved data will be LOST! Continue? (y/n)") },
        { "file.not_found", ("Путь к файлу не найден в:{0}", "File path not found: {0}") },
        { "file.empty_or_corrupted", ("Файл пуст или повреждён!", "File is empty or corrupted!") },
        { "load.success", ("Успешно загружено {0} книг из файла!", "Successfully loaded {0} books from file!") },
        { "load.error", ("Ошибка при загрузке: {0}", "Error during loading: {0}") },

        { "save.confirmation", ("Будет полностью сохранено состояние текущей библиотеки. Продолжить? (y/n)", "This will save the entire current library state. Continue? (y/n)") },
        { "save.success", ("Успешно сохранено {0} книг в файл!", "Successfully saved {0} books to file!") },
        { "save.error", ("Ошибка сохранения: {0}", "Error saving: {0}") },

        { "input.invalid", ("Невернный ввод!", "Invalid input!") },
        { "select.choose_book_question", ("Хотите выбрать найденную книгу для дальнейшего взаимодействия? (y/n)", "Do you want to select the found book for further interaction? (y/n)") },
        { "book.selected_success", ("Книга \"{0}\" успешно выбрана!", "Book \"{0}\" successfully selected!") },
        { "add.enter_params", ("Введите параметры для добавления книги!", "Enter parameters to add a book!") },
        { "enter.book.title", ("Введите название книги:", "Enter book title:") },
        { "enter.book.author", ("Введите имя автора:", "Enter author name:") },
        { "enter.book.year", ("Введите год создания:", "Enter year of publication:") },
        { "enter.book.genre", ("Введите название жанра:", "Enter genre name:") },
        { "enter.book.amount", ("Введите количество экземпляров:", "Enter number of copies:") },
        { "book.created", ("Книга под названием \"{0}\" успешно создана!", "Book titled \"{0}\" successfully created!") },
        { "book.select.multiple", ("Введите идентификтор книги, которую вы хотите выбрать (0 для завершения): ", "Enter the ID of the book you want to select (0 to finish): ") },
        { "no_book_with_id", ("Нету книги с указанным идентификатором!", "No book with the specified ID!") }

    };

    public static void SetLanguage(string lang)
    {
        if (lang == "ru") _lang = "ru";
        else _lang = "en";
    }

    public static string T(string key) //T - Translation;
    {
        if (_dict.TryGetValue(key, out var pair))
            return _lang == "en" ? pair.en : pair.ru;
        return $"Missing \"{key}\"";
    }

    public static string Tf(string key, params object[] args) => string.Format(T(key), args); //Tf - Translation with formatting
}