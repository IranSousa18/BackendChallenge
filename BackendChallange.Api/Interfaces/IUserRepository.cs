using BackendChallange.Api.Models;

namespace BackendChallange.Api.Interfaces;

public interface IUserRepository
{
    List<User> GetAll();

    User? GetById(Guid id);

    void Add(User user);

    void Update(User user);

    void Delete(User user);

    bool EmailExists(string email);

    bool CpfExists(string cpf);

    bool EmailExistsForAnotherUser(string email, Guid id);

    bool CpfExistsForAnotherUser(string cpf, Guid id);
}