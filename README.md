Thing to improve:
1) Refactor the behaviour of choosenBook: It should be a inherited class from Book class
Working with choosenBooks in a simple variable causes a bug when saving to json saves IsChoosen state. Dunno if it can cause serious problems but it's not good in general
2) Refactor methods in LibraryDataHandler class, such as BookSelector, BookHash and BookMultiple
   It would be better to make them as overrided methods of base method BookSelector
3) Somehow improve JSON files handling
   When writing Cyrillic to a JSON file it writes it for example as: "\u041A\u0430\u043A\u0441\u0442\u0430\u0442\u044C       \u043C\u0438\u043B\u043B\u0438\u043E\u043D\u0435\u0440\u043E\u043C"
