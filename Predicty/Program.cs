using Predicty.Repositories;
using Predicty.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<FootballApiService>();

builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<TeamRepository>();

builder.Services.AddScoped<LeagueRepository>();
builder.Services.AddScoped<LeagueService>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<MatchRepository>();
builder.Services.AddScoped<MatchService>();

builder.Services.AddScoped<DBServices>();

builder.Services.AddHttpClient<FootballApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
