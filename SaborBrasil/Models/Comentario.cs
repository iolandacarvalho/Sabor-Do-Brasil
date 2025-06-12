using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborBrasil.Models
{
    public class Comentario
    {
        [Key]
        [Column("idcomentario")]
        public int IdComentario { get; set; }

        [Required]
        [Column("descricao")]
        public string Descricao { get; set; } = string.Empty;
        [Column("id_usuario")]
        public int? Id_Usuario { get; set; }   
        [Column("data")]
        public DateTime? Data { get; set; }
        [Column("post_id")]
        public int? Post_Id { get; set; }
        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created_At { get; private set; }
    }
}
