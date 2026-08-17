SELECT
    Id,
    Name,
    Email,
    Cpf,
    BirthDate
FROM Users
WHERE Id = @Id;