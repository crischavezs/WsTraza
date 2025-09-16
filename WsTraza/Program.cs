using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services) // Permite que Serilog acceda a los servicios configurados
        .Enrich.FromLogContext()
//.WriteTo.Console()
//.WriteTo.File("logs/log-.log", rollingInterval: RollingInterval.Day)
//.WriteTo.Debug() // Esto te permitir� ver los mensajes de log en la consola de depuraci�n
);

// A�adir servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingrese el token JWT en el formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Registrar AuthorizeAttribute para inyecci�n de dependencias
builder.Services.AddScoped<WsTraza.Middleware.AuthorizeAttribute>();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WsTraza.WebApi v1");
        c.RoutePrefix = "swagger"; // Configurar la ruta para Swagger UI en desarrollo
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WsTraza.WebApi v1");
        c.RoutePrefix = "swagger"; // Configurar la ruta para Swagger UI en producci�n
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll"); // Agregar esta l�nea para habilitar CORS
app.UseAuthorization();
app.MapControllers();

app.Run();