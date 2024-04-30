// Definindo o namespace para o modelo Book
using System.Text.Json.Serialization;

namespace Library.Models
{
    // Definindo o modelo Book como um registro (record)
    public record Book
    {
        public Guid Id { get; init; }
        public string Cliente { get; init; }
        public string NomeLivro { get; init; }
        public string Lancamento { get; init; }
        public int QntdLivro { get; init; }

        // Atributo JsonConstructor indica que este construtor deve ser usado para desserializar JSON
        [JsonConstructor]
        // Construtor para criar um novo livro
                public Book(Guid id, string cliente, string nomeLivro, string lancamento, int qntdLivro)
        {
            Id = id;
            Cliente = cliente;
            NomeLivro = nomeLivro;
            Lancamento = lancamento;
            QntdLivro = qntdLivro;
        }

        // Construtor para criar um novo livro
        public Book(string cliente, string nomeLivro, string lancamento, int qntdLivro)
            : this(Guid.NewGuid(), cliente, nomeLivro, lancamento, qntdLivro)
        {
        }
    }
}
