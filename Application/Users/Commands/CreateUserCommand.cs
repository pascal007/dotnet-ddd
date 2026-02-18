
using MediatR;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Application.Users.Commands;

public record CreateUserCommand(string firstName, string lastName, string email, string Password, string confirmPassword) : IRequest<Guid>;
