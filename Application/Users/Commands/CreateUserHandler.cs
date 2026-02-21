

using Domain.Aggregates.User;
using Domain.Aggregates.Wallet;
using MediatR;
using WalletDemo.Application.Interfaces;
using WalletDemo.Domain.Common;
using WalletDemo.Domain.Exceptions;
using WalletDemo.Domain.ValueObjects;

namespace WalletDemo.Application.Users.Commands;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;


    public CreateUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Email email = new Email(request.email);
        var existingUser = await _userRepository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            throw new DomainException("A user with the same email already exists.");
        }

        if (request.Password != request.confirmPassword)
        {
            throw new DomainException("Password and confirm password do not match.");
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);

        var user = User.Create(Guid.NewGuid(), request.firstName, request.lastName, email, hashedPassword);

        await _userRepository.AddAsync(user);

        Wallet wallet = Wallet.Create(user.Id, Currencies.GBP);

        _unitOfWork.Track(wallet);

        await _unitOfWork.SaveChangesAsync();

        return user.Id;
    }
}
