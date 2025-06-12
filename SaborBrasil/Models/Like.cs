using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaborBrasil.Models
{
    [Table("likes")]
    public class Like
    {
        [Key]
        [Column("idlikes")]
        public int IdLikes { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("id_post")]
        public int IdPost { get; set; }
    }
}
