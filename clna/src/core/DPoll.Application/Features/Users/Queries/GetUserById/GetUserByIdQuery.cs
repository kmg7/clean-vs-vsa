using Dpoll.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPoll.Application.Features.Users.Queries.GetUserById;
public class GetUserByIdQuery : IRequest<User>
{
    [Required]
    public Guid Id { get; init; }
}
