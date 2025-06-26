using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ChatbotAI.BL.ExternalAPIs;
using ChatbotAI.BL.ExternalAPIs.Interfaces;
using ChatbotAI.BL.Services;
using ChatbotAI.BL.Services.Interfaces;
using ChatbotAI.Core;
using ChatbotAI.DAL.DTOs.Message;
using ChatbotAI.DAL.Repositories;
using ChatbotAI.DAL.Repositories.Interfaces;
using ChatbotAI.DAL.Services;
using ChatbotAI.DAL.Services.Interfaces;

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
builder.Services.AddScoped<IAIServiceMock, AIServiceMock>();
builder.Services.AddScoped<IRequestHandler<MessageUserRequestDTO, IAsyncEnumerable<MessageAIResponseDTO>>, MediatRChatMessageHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
