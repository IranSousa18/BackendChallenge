namespace BackendChallange.Api.DTOs;

public class UserResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public DateOnly BirthDate { get; set; }
}