using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SaborBrasil.Models
{
    [Table("Publicacao")]
    public class Publicacao
    {
        [Key]
        [Column("idpost")]
        public int IdPost { get; set; }

        [Required]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Column("descricao")]
        public string Descricao { get; set; } = string.Empty;

        [Column("Imagem")]
        public string? Imagem { get; set; }

        [Column("UsuarioId")]
        public int? UsuarioId { get; set; }

        [Column("dataPublicao")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Gera automaticamente o valor para a coluna
        public DateTime? DataPublicao { get; set; }

        [Column("local")]
        public string? Local { get; set; }

        [Column("cidade")]
        public string? Cidade { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("excluido")]
        public bool Excluido { get; set; } = false;

        [Column("dataExclusao")]
        public DateTime? DataExclusao { get; set; }

        [Column("excluidoPor")]
        public int? ExcluidoPor { get; set; }
    }
}