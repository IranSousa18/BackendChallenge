UPDATE Users
SET
    Name = @Name,
    Email = @Email,
    Cpf = @Cpf,
    BirthDate = @BirthDate
WHERE Id = @Id;