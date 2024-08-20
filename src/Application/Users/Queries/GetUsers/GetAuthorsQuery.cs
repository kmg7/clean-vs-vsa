namespace Application.Users.Queries.GetUsers;

using Entities;
using MediatR;

public class GetUsersQuery : IRequest<List<User>> { }
