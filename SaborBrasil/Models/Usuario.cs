using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborBrasil.Models
{
    [Table("Usuario")] // Mapeia explicitamente para a tabela "Usuario"
    public class Usuario
    {
        [Key]
        [Column("idusuario")] // Mapeia para a coluna "idusuario"
        public int IdUsuario { get; set; }

        [Column("email")] // Mapeia para a coluna "email"
        public string? Email { get; set; }

        [Required]
        [Column("senha")] // Mapeia para a coluna "senha"
        public string? Senha { get; set; }

        [Column("foto_perfil")] // Mapeia para a coluna "foto_perfil"
        public string? FotoPerfil { get; set; }

        [Column("created_at")] // Mapeia para a coluna "created_at"
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
