using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborBrasil.Models
{
    [Table("dislikes")]
    public class Dislike
    {
        [Key]
        [Column("iddislikes")]
        public int IdDislikes { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_post")]
        public int IdPost { get; set; }
    }
} 