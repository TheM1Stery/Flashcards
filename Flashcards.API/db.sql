CREATE TABLE IF NOT EXISTS Cards(
    Id TEXT NOT NULL constraint PK_Cards PRIMARY KEY,
    Question TEXT NOT NULL,
    Answer TEXT NOT NULL 
);

CREATE TABLE IF NOT EXISTS Tags(
    Name TEXT NOT NULL,
    Id TEXT NOT NULL constraint PK_Tags PRIMARY KEY 
);

CREATE TABLE IF NOT EXISTS CardTags(
    CardId TEXT NOT NULL constraint FK_CardTags_Cards REFERENCES Cards(Id),
    TagId TEXT NOT NULL constraint FK_CardTags_Tags REFERENCES Tags(Id),
    constraint PK_CardTags PRIMARY KEY (CardId, TagId)
);
