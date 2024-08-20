namespace Application.Presentations.Queries.GetPresentations;

using Entities;
using MediatR;

public class GetPresentationsQuery : IRequest<List<Presentation>> { }
