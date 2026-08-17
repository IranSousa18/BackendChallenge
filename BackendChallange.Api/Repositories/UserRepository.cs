using BackendChallange.Api.Interfaces;
using BackendChallange.Api.Models;

namespace BackendChallange.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public List<User> GetAll()
    {
        return _users;
    }

    public User? GetById(Guid id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }

    public void Add(User user)
    {
        _users.Add(user);
    }

    public bool EmailExists(string email)
    {
        return _users.Any(user =>
            user.Email.Equals(
                email,
                StringComparison.OrdinalIgnoreCase
            )
        );
    }

    public bool CpfExists(string cpf)
    {
        return _users.Any(user => user.Cpf == cpf);
    }

    public bool EmailExistsForAnotherUser(string email, Guid id)
    {
        return _users.Any(user =>
            user.Id != id &&
            user.Email.Equals(
                email,
                StringComparison.OrdinalIgnoreCase
            )
        );
    }

    public bool CpfExistsForAnotherUser(string cpf, Guid id)
    {
        return _users.Any(user =>
            user.Id != id &&
            user.Cpf == cpf
        );
    }

    public void Update(User user)
    {
        var existingUser = GetById(user.Id);

        if (existingUser == null)
        {
            return;
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Cpf = user.Cpf;
        existingUser.BirthDate = user.BirthDate;
    }

    public void Delete(User user)
    {
        _users.Remove(user);
    }
}