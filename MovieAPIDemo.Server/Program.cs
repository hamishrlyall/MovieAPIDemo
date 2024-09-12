using MovieAPIDemo.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MovieAPIDemo.Server;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder( args );

//Db Configuration
builder.Services.AddDbContext<MovieDbContext>( options =>
{
    options.UseSqlServer( builder.Configuration.GetConnectionString( "Default" ) );
} );

builder.Services.AddControllers( );
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc( "v1", new OpenApiInfo { Title = "MovieApiDemo", Version = "v1", } );
} );
builder.Services.AddAutoMapper( AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers( );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer( );
builder.Services.AddSwaggerGen( );

builder.Services.AddCors( options => options.AddPolicy( "AllowAll", p => p.AllowAnyOrigin( ).AllowAnyMethod( ).AllowAnyHeader( ) ) );


var app = builder.Build( );

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment( ))
{
    app.UseSwagger( );
    app.UseSwaggerUI( );
}

app.UseStaticFiles( new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider( @"E:\to-delete" ),
    RequestPath = "/StaticFiles"
} );

app.UseHttpsRedirection( );

app.UseAuthorization( );
app.UseCors( "AllowAll" );

app.MapControllers( );

app.Run( );
