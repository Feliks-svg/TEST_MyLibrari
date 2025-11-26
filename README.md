Thing to improve:
1) Refactor the behaviour of choosenBook: It should be a inherited class from Book class
Working with choosenBooks in a simple variable causes a bug when saving to json saves IsChoosen state. Dunno if it can cause serious problems but it's not good in general
2) Refactor methods in LibraryDataHandler class, such as BookSelector, BookHash and BookMultiple
   It would be better to make them as overrided methods of base method BookSelector
