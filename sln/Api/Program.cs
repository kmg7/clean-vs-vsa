using Api.Endpoints;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PgDbContext>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapUserEndpoints();
app.MapPresentationEndpoints();
app.MapContentEndpoints();
app.MapSlideEndpoints();
app.MapSessionEndpoints();

app.Run();
