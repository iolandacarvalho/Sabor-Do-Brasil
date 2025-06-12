using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using SaborBrasil.Data;
using SaborBrasil.Models;

[ApiController]
[Route("api/[controller]")]
public class ComentariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ComentariosController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/comentarios?postId=1
    [HttpGet]
    public async Task<IActionResult> GetComentarios([FromQuery] int postId)
    {
        var comentarios = await _context.Comentarios
            .Where(c => c.Post_Id == postId)
            .OrderByDescending(c => c.Created_At).Join(_context.Usuarios,
              c => c.Id_Usuario,
              u => u.IdUsuario,
              (c, u) => new {
                  c.IdComentario,
                  c.Descricao,
                  c.Id_Usuario,
                  c.Data,
                  c.Post_Id,
                  c.Created_At,
                  NomeUsuario = u.Email // Exibe o e-mail do usuário, já que não existe mais Nome
              })
            .ToListAsync();

        return Ok(comentarios);
    }

    // POST: api/comentarios
    [HttpPost]
    public async Task<IActionResult> PostComentario([FromBody] Comentario comentario)
    {
        if (comentario == null || string.IsNullOrWhiteSpace(comentario.Descricao))
            return BadRequest(new { message = "Comentário inválido." });

        
        

        _context.Comentarios.Add(comentario);
        await _context.SaveChangesAsync();

        return Ok(comentario);
    }

    // PUT: api/comentarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> EditarComentario(int id, [FromBody] Comentario comentarioEditado)
    {
        var comentario = await _context.Comentarios.FindAsync(id);
        if (comentario == null)
            return NotFound(new { message = "Comentário não encontrado." });

        // Só permite editar se for o dono do comentário
        if (comentario.Id_Usuario != comentarioEditado.Id_Usuario)
            return Forbid();

        comentario.Descricao = comentarioEditado.Descricao;
        comentario.Data = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok(comentario);
    }
}