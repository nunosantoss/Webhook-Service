using PaymentAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços à container

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Define uma origem específica (não a wildcard)
        policy.WithOrigins("http://localhost:3000")  // Front-end está na porta 3000
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();  // Permite o envio de credenciais
    });
});

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("webhook-api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5002");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddControllers();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Aprender mais sobre configurar o Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Utiliza a política CORS correta (AllowAll, não AllowWebhookAPI)
app.UseCors("AllowAll");

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapHub<PaymentHub>("/paymentHub");

app.Run();
