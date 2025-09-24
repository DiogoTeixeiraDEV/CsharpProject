using Application.Services;
using Domain.Repositories;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.filters;

var builder = WebApplication.CreateBuilder(args);

var jwtSecret = builder.Configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret não encontrado!");

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header usando o Bearer scheme. Exemplo: 'Bearer {token}'"
    });

     c.OperationFilter<AuthorizeCheckOperationFilter>();
});

// Configura DbContext (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// DI - Injeção de dependência
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>( sp =>
{
    var userRepository = sp.GetRequiredService<IUserRepository>();
    var jwtSecret = builder.Configuration["Jwt:Secret"] 
        ?? throw new InvalidOperationException("JWT Secret não encontrado!");
    return new AuthService(userRepository, jwtSecret);
}); 

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.Migrate(); // garante que o DB está atualizado

    if (!context.Users.Any(u => u.Role == UserRole.Admin))
    {
        var admin = new User(
            name: "Admin",
            email: "admin@domain.com",
            password: "SenhaAdmin123!",
            role: UserRole.Admin
        );

        context.Users.Add(admin);
        context.SaveChanges();

        Console.WriteLine("Usuário admin criado com sucesso");
    }
    else
    {
        Console.WriteLine("Usuário admin já existe");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
