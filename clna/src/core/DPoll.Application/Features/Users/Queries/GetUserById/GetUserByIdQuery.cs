using Dpoll.Domain.Entities;
using DPoll.Application.Shared;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DPoll.Application.Features.Users.Queries.GetUserById;
public class GetUserByIdQuery : IRequest<Result<User>>
{
    [Required]
    public Guid Id { get; init; }
}
