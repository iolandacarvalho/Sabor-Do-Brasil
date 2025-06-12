using Microsoft.AspNetCore.Mvc;
using SaborBrasil.Data;
using SaborBrasil.Models;

[Route("api/publicacoes")]
[ApiController]
public class PublicacaoController : ControllerBase
{
    private readonly AppDbContext _context;

    public PublicacaoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("Criarpublicacao")]
    public IActionResult CriarPublicacao([FromForm] Publicacao publicacao, [FromForm] IFormFile? imagem)
    {
        Console.WriteLine("Método CriarPublicacao chamado!");
        try
        {
            if (imagem != null)
            {
                var uploads = Path.Combine("wwwroot/uploads");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                // Gera um nome único para a imagem
                var ext = Path.GetExtension(imagem.FileName);
                var uniqueName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imagem.CopyTo(stream);
                }
                publicacao.Imagem = "/uploads/" + uniqueName;
            }

            // Garantir que os campos de localização não sejam nulos
            publicacao.Local = publicacao.Local ?? "";
            publicacao.Cidade = publicacao.Cidade ?? "";
            publicacao.Estado = publicacao.Estado ?? "";

            _context.Publicacoes.Add(publicacao);
            _context.SaveChanges();

            return Ok(new { message = "Publicação criada com sucesso!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERRO AO CADASTRAR PUBLICAÇÃO: " + ex.Message);
            return BadRequest(new { message = "Erro ao criar publicação: " + ex.Message });
        }
    }

    [HttpGet("Listar")]
    public IActionResult Listar()
    {
        try
        {
            Console.WriteLine("Iniciando listagem de publicações...");
            
            var publicacoes = _context.Publicacoes.Where(pub => !pub.Excluido)
            .Join(_context.Usuarios,
                  pub => pub.UsuarioId,
                  user => user.IdUsuario,
                  (pub, user) => new
                  {
                      pub.IdPost,
                      pub.Nome,
                      pub.Descricao,
                      pub.Imagem,
                      pub.UsuarioId,
                      pub.DataPublicao,
                      pub.Local,
                      pub.Cidade,
                      pub.Estado,
                      Autor = user.Nome
                  })
                .OrderByDescending(p => p.DataPublicao)
                .ToList();
                
            Console.WriteLine($"Publicações encontradas: {publicacoes.Count}");
            return Ok(publicacoes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO AO LISTAR PUBLICAÇÕES: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            
            // Se o erro for relacionado às colunas de localização, tentar sem elas
            if (ex.Message.Contains("local") || ex.Message.Contains("cidade") || ex.Message.Contains("estado"))
            {
                try
                {
                    Console.WriteLine("Tentando listar sem colunas de localização...");
                    var publicacoesFallback = _context.Publicacoes.Where(pub => !pub.Excluido)
                    .Join(_context.Usuarios,
                          pub => pub.UsuarioId,
                          user => user.IdUsuario,
                          (pub, user) => new
                          {
                              pub.IdPost,
                              pub.Nome,
                              pub.Descricao,
                              pub.Imagem,
                              pub.UsuarioId,
                              pub.DataPublicao,
                              Local = "",
                              Cidade = "",
                              Estado = "",
                              Autor = user.Nome
                          })
                        .OrderByDescending(p => p.DataPublicao)
                        .ToList();
                        
                    Console.WriteLine($"Publicações encontradas (fallback): {publicacoesFallback.Count}");
                    return Ok(publicacoesFallback);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"ERRO NO FALLBACK: {ex2.Message}");
                }
            }
            
            return BadRequest(new { message = "Erro ao carregar publicações: " + ex.Message });
        }
    }

    public class LikeRequest
    {
        public int idPost { get; set; }
        public int idUsuario { get; set; }
    }

    public class DislikeRequest
    {
        public int idPost { get; set; }
        public int idUsuario { get; set; }
    }

    [HttpPost("Like")]
    public IActionResult Like([FromBody] LikeRequest req)
    {
        var jaCurtiu = _context.Likes.Any(l => l.IdUsuario == req.idUsuario && l.IdPost == req.idPost);
        if (jaCurtiu)
            return BadRequest(new { message = "Você já curtiu esta publicação." });

        // Remove dislike se existir
        var dislikeExistente = _context.Dislikes.FirstOrDefault(d => d.IdUsuario == req.idUsuario && d.IdPost == req.idPost);
        if (dislikeExistente != null)
        {
            _context.Dislikes.Remove(dislikeExistente);
        }

        var like = new Like { IdUsuario = req.idUsuario, IdPost = req.idPost };
        _context.Likes.Add(like);
        _context.SaveChanges();
        return Ok(new { message = "Like registrado!" });
    }

    [HttpDelete("Like")]
    public IActionResult RemoverLike([FromBody] LikeRequest req)
    {
        var like = _context.Likes.FirstOrDefault(l => l.IdUsuario == req.idUsuario && l.IdPost == req.idPost);
        if (like == null)
            return NotFound(new { message = "Like não encontrado." });

        _context.Likes.Remove(like);
        _context.SaveChanges();
        return Ok(new { message = "Like removido com sucesso." });
    }

    [HttpPost("Dislike")]
    public IActionResult Dislike([FromBody] DislikeRequest req)
    {
        var jaDeuDislike = _context.Dislikes.Any(d => d.IdUsuario == req.idUsuario && d.IdPost == req.idPost);
        if (jaDeuDislike)
            return BadRequest(new { message = "Você já deu dislike nesta publicação." });

        // Remove like se existir
        var likeExistente = _context.Likes.FirstOrDefault(l => l.IdUsuario == req.idUsuario && l.IdPost == req.idPost);
        if (likeExistente != null)
        {
            _context.Likes.Remove(likeExistente);
        }

        var dislike = new Dislike { IdUsuario = req.idUsuario, IdPost = req.idPost };
        _context.Dislikes.Add(dislike);
        _context.SaveChanges();
        return Ok(new { message = "Dislike registrado!" });
    }

    [HttpDelete("Dislike")]
    public IActionResult RemoverDislike([FromBody] DislikeRequest req)
    {
        var dislike = _context.Dislikes.FirstOrDefault(d => d.IdUsuario == req.idUsuario && d.IdPost == req.idPost);
        if (dislike == null)
            return NotFound(new { message = "Dislike não encontrado." });

        _context.Dislikes.Remove(dislike);
        _context.SaveChanges();
        return Ok(new { message = "Dislike removido com sucesso." });
    }

    [HttpGet("LikesCount/{idPost}")]
    public IActionResult LikesCount(int idPost)
    {
        var count = _context.Likes.Count(l => l.IdPost == idPost);
        return Ok(new { likes = count });
    }

    [HttpGet("DislikesCount/{idPost}")]
    public IActionResult DislikesCount(int idPost)
    {
        var count = _context.Dislikes.Count(d => d.IdPost == idPost);
        return Ok(new { dislikes = count });
    }

    [HttpGet("UserLiked")]
    public IActionResult UserLiked([FromQuery] int postId, [FromQuery] int userId)
    {
        var liked = _context.Likes.Any(l => l.IdPost == postId && l.IdUsuario == userId);
        return Ok(new { liked });
    }

    [HttpGet("UserDisliked")]
    public IActionResult UserDisliked([FromQuery] int postId, [FromQuery] int userId)
    {
        var disliked = _context.Dislikes.Any(d => d.IdPost == postId && d.IdUsuario == userId);
        return Ok(new { disliked });
    }

    [HttpDelete("Deletar/{idPost}")]
    public IActionResult Deletar(int idPost, [FromQuery] int usuarioId)
    {
        var publicacao = _context.Publicacoes.FirstOrDefault(p => p.IdPost == idPost);
        if (publicacao == null)
            return NotFound(new { message = "Publicação não encontrada." });

        if (publicacao.UsuarioId != usuarioId)
            return Forbid();

        publicacao.Excluido = true;
        publicacao.DataExclusao = DateTime.Now;
        publicacao.ExcluidoPor = usuarioId;
        
        
        _context.SaveChanges();

        return Ok(new { message = "Publicação removida com sucesso!" });

    }

    [HttpGet("TotalLikes")]
    public IActionResult TotalLikes()
    {
        try
        {
            var totalLikes = _context.Likes.Count();
            return Ok(new { total = totalLikes });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao carregar total de likes: " + ex.Message });
        }
    }

    [HttpGet("TotalDislikes")]
    public IActionResult TotalDislikes()
    {
        try
        {
            var totalDislikes = _context.Dislikes.Count();
            return Ok(new { total = totalDislikes });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao carregar total de dislikes: " + ex.Message });
        }
    }

    [HttpGet("CommentsCount/{postId}")]
    public IActionResult CommentsCount(int postId)
    {
        try
        {
            var commentsCount = _context.Comentarios.Count(c => c.Post_Id == postId);
            return Ok(new { comments = commentsCount });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao carregar comentários: " + ex.Message });
        }
    }
}
