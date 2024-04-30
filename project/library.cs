/*                                                */
/*            TRABALHO C#                        */
/*            EVELYN CELESTINO TEIXEIRA          */

using NSwag.AspNetCore;
using Library.Models;
using Library.Data;
using Microsoft.EntityFrameworkCore;

class HelloWeb
{
    static void Main(string[] args)
    {
        // Criando um aplicativo web usando o ASP.NET Core
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(config =>
        {
            // Configurando o nome, título e versão da documentação
            config.DocumentName = "Library API";
            config.Title = "LibraryAPI v1";
            config.Version = "v1";
        });

        // Adicionando o contexto do banco de dados
        builder.Services.AddDbContext<AppDbContext>();

        WebApplication app = builder.Build();

        // Habilitando a interface do Swagger apenas em ambientes de desenvolvimento
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi(config =>
            {
                // Configurando o título e os caminhos para a interface do Swagger
                config.DocumentTitle = "Library API";
                config.Path = "/swagger";
                config.DocumentPath = "/swagger/{documentName}/swagger.json";
                config.DocExpansion = "list"; // Expandir todos os itens por padrão
            });
        }

// TODOS OS 7 ENDPOINTES
//  .GET  .POST  .PUT  .PATCH  .DELETE

    // Endpoint GET para listar todos os livros
        app.MapGet("/todos-livros", (AppDbContext context) =>
        {
            var books = context.Book.ToList();
            return Results.Ok(books);
        }).Produces<Book>();


    // Este endpoint GET busca livros por nome
        app.MapGet("/books/by-name/{name}", (string name, AppDbContext context) =>
        {
            // Aqui estamos consultando o contexto do banco de dados para obter os livros com o nome fornecido.
            var books = context.Book.Where(b => b.NomeLivro.Equals(name)).ToList();
            
            // Verificamos se encontramos algum livro com o nome fornecido.
            if (books.Count == 0)
            {
                // Se nenhum livro for encontrado, retornamos um código de status 404 - Not Found.
                return Results.NotFound();
            }
            
            // Se encontrarmos livros, retornamos um código de status 200 - OK, juntamente com os livros encontrados.
            return Results.Ok(books);
        }).Produces<Book>(); // Definimos que este endpoint produz objetos do tipo Book.

        
    // Endpoint GET para obter um livro por ID
        app.MapGet("/books/{id}", (Guid id, AppDbContext context) =>
        {
            var book = context.Book.Find(id);
            if (book == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(book);
        }).Produces<Book>();

    // Endpoint GET para buscar livros por ano de lançamento
        app.MapGet("/books/by-release/{year}", (string year, AppDbContext context) =>
        {
            var books = context.Book.Where(b => b.Lancamento == year).ToList();
            return Results.Ok(books);
        }).Produces<Book>();

    // Endpoint POST para adicionar um novo livro
        app.MapPost("/add-book", (Book book, AppDbContext context) =>
        {
            context.Book.Add(book);
            context.SaveChanges();
            return Results.Created($"/books/{book.Id}", book);
        }).Produces<Book>();

    // Endpoint PUT para atualizar um livro
        app.MapPut("/atualizar/{id}", (Guid id, Book book, AppDbContext context) =>
        {
            if (id != book.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var existingBook = context.Book.Find(id);
            if (existingBook == null)
            {
                return Results.NotFound();
            }
            context.Entry(existingBook).CurrentValues.SetValues(book);
            context.SaveChanges();
            return Results.NoContent();
        }).Produces<Book>();

    //  Metodo Patch
        app.MapPatch("/atualizar/{id}", (AppDbContext context, Guid id, string NnomeLivro) => {
            var book = context.Book.Find(id);
            if (book == null) {
                return Results.NotFound("Livro não encontrado.");
            }
            
            var updatedGame = book with { NomeLivro = NnomeLivro };
            context.Entry(book).CurrentValues.SetValues(updatedGame);
            context.SaveChanges();
            
            return Results.Ok(updatedGame);
        }).Produces<Book>();

            
    // Endpoint DELETE para excluir um livro por ID
        app.MapDelete("/remove-book/{id}", (Guid id, AppDbContext context) =>
        {
            var book = context.Book.Find(id);
            if (book == null)
            {
                return Results.NotFound();
            }
            context.Book.Remove(book);
            context.SaveChanges();
            return Results.NoContent();
        }).Produces<Book>();

// TODOS OS 7 ENDPOINTES
//  .GET  .POST  .PUT  .PATCH  .DELETE

        app.Run();
    }
}
