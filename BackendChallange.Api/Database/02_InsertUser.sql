INSERT INTO Users
(
    Id,
    Name,
    Email,
    PasswordHash,
    Cpf,
    BirthDate
)
VALUES
(
    @Id,
    @Name,
    @Email,
    @PasswordHash,
    @Cpf,
    @BirthDate
);