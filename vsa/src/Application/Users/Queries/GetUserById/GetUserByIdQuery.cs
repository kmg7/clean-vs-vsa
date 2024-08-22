namespace Application.Users.Queries.GetUserById;

using System.ComponentModel.DataAnnotations;
using Entities;
using MediatR;

public class GetUserByIdQuery : IRequest<User>
{
    [Required]
    public Guid Id { get; init; }
}
