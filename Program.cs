using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSSQLTEST.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Authentication>();
//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:KEY")))
    };
}
);
//JWT办北
//builder.Services.AddMvc(option =>
//{
//    option.Filters.Add(new AuthorizeFilter());
//});

//阁办砞﹚
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS",
        builder =>
        {
            builder.WithOrigins("http://localhost:3003")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddDbContext<ModelContext>(a => a.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//办甅ノ阁办砞﹚
app.UseCors("CORS");

//喷靡砞﹚(JWT) ㄢ砞﹚ゲ斗Τ
app.UseAuthentication(); //

app.UseAuthorization();//

app.MapControllers();

app.Run();
