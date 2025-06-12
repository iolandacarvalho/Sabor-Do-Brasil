using Microsoft.EntityFrameworkCore;
using SaborBrasil.Models; // Certifique-se de importar o namespace correto

namespace SaborBrasil.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

         public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura explicitamente o nome da tabela como "Usuario"
            modelBuilder.Entity<Usuario>().ToTable("Usuario");

             modelBuilder.Entity<Comentario>().ToTable("comentarios");
        modelBuilder.Entity<Comentario>().HasKey(c => c.IdComentario);
        }

        
    }
}

