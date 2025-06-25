using ChatbotAI.BL.Services;
using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.Core;
using ChatbotAI.DAL.Repositories;
using ChatbotAI.DAL.Repositories.Interfaces;
using ChatbotAI.DAL.Services;
using ChatbotAI.DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChatbotAIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

// Repositories
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
