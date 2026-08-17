using System.Net.Mail;
using BackendChallange.Api.DTOs;
using BackendChallange.Api.Interfaces;
using BackendChallange.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace BackendChallange.Api.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public List<User> GetAll()
    {
        return _userRepository.GetAll();
    }

    public User GetById(Guid id)
    {
        var user = _userRepository.GetById(id);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        return user;
    }

    public User Create(CreateUserRequest request)
    {
        var normalizedCpf = NormalizeCpf(request.Cpf);

        ValidateName(request.Name);
        ValidateEmail(request.Email);
        ValidatePassword(request.Password);
        ValidateCpf(normalizedCpf);
        ValidateBirthDate(request.BirthDate);

        if (_userRepository.EmailExists(request.Email))
        {
            throw new InvalidOperationException(
                "Já existe um usuário com este e-mail."
            );
        }

        if (_userRepository.CpfExists(normalizedCpf))
        {
            throw new InvalidOperationException(
                "Já existe um usuário com este CPF."
            );
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Cpf = normalizedCpf,
            BirthDate = request.BirthDate
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        _userRepository.Add(user);

        return user;
    }

    public User Update(Guid id, UpdateUserRequest request)
    {
        var user = _userRepository.GetById(id);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        var normalizedCpf = NormalizeCpf(request.Cpf);

        ValidateName(request.Name);
        ValidateEmail(request.Email);
        ValidateCpf(normalizedCpf);
        ValidateBirthDate(request.BirthDate);

        if (_userRepository.EmailExistsForAnotherUser(request.Email, id))
        {
            throw new InvalidOperationException(
                "Já existe outro usuário com este e-mail."
            );
        }

        if (_userRepository.CpfExistsForAnotherUser(normalizedCpf, id))
        {
            throw new InvalidOperationException(
                "Já existe outro usuário com este CPF."
            );
        }

        user.Name = request.Name.Trim();
        user.Email = request.Email.Trim();
        user.Cpf = normalizedCpf;
        user.BirthDate = request.BirthDate;

        _userRepository.Update(user);

        return user;
    }

    public void Delete(Guid id)
    {
        var user = _userRepository.GetById(id);

        if (user == null)
        {
            throw new KeyNotFoundException("Usuário não encontrado.");
        }

        _userRepository.Delete(user);
    }

    private static string NormalizeCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return string.Empty;
        }

        return new string(
            cpf.Where(char.IsDigit).ToArray()
        );
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("O nome é obrigatório.");
        }
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("O e-mail é obrigatório.");
        }

        try
        {
            var address = new MailAddress(email);

            if (address.Address != email.Trim())
            {
                throw new ArgumentException(
                    "O e-mail informado é inválido."
                );
            }
        }
        catch (FormatException)
        {
            throw new ArgumentException(
                "O e-mail informado é inválido."
            );
        }
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(
                "A senha é obrigatória."
            );
        }

        if (password.Length < 6)
        {
            throw new ArgumentException(
                "A senha deve possuir pelo menos 6 caracteres."
            );
        }
    }

    private static void ValidateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            throw new ArgumentException(
                "O CPF é obrigatório."
            );
        }

        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
        {
            throw new ArgumentException(
                "O CPF deve possuir exatamente 11 números."
            );
        }

        if (cpf.Distinct().Count() == 1)
        {
            throw new ArgumentException(
                "O CPF informado é inválido."
            );
        }

        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (cpf[i] - '0') * (10 - i);
        }

        int remainder = sum % 11;

        int firstDigit = remainder < 2
            ? 0
            : 11 - remainder;

        if (firstDigit != cpf[9] - '0')
        {
            throw new ArgumentException(
                "O CPF informado é inválido."
            );
        }

        sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += (cpf[i] - '0') * (11 - i);
        }

        remainder = sum % 11;

        int secondDigit = remainder < 2
            ? 0
            : 11 - remainder;

        if (secondDigit != cpf[10] - '0')
        {
            throw new ArgumentException(
                "O CPF informado é inválido."
            );
        }
    }

    private static void ValidateBirthDate(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (birthDate > today)
        {
            throw new ArgumentException(
                "A data de nascimento não pode estar no futuro."
            );
        }
    }
}