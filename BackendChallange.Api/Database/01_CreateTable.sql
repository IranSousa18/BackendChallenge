CREATE TABLE Users
(
    Id UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    Cpf VARCHAR(11) NOT NULL,
    BirthDate DATE NOT NULL,

    CONSTRAINT PK_Users
        PRIMARY KEY (Id),

    CONSTRAINT UQ_Users_Email
        UNIQUE (Email),

    CONSTRAINT UQ_Users_Cpf
        UNIQUE (Cpf)
);