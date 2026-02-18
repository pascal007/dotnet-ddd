

namespace WalletDemo.Application.Users.Commands;

using MediatR;

public record LoginUserCommand(string Email, string Password) : IRequest<string>;
