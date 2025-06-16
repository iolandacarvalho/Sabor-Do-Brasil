using Microsoft.AspNetCore.Mvc;
using SaborBrasil.Data;
using SaborBrasil.Models; // Certifique-se de importar o namespace correto

[Route("Usuario")] // Define a rota base para o controlador
public class UsuarioController : Controller
{
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("Cadastrar")]
    public IActionResult Cadastrar([FromForm] string Email, [FromForm] string Senha, [FromForm] string Nome, [FromForm] IFormFile? fotoPerfil)
    {
        try
        {
            // Criar objeto usuário
            var usuario = new Usuario
            {
                Email = Email ?? "",
                Senha = Senha,
                Nome = Nome,
                CreatedAt = DateTime.Now
            };

            // Processar foto de perfil se fornecida
            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                var uploads = Path.Combine("wwwroot/uploads/profiles");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);
                var ext = Path.GetExtension(fotoPerfil.FileName);
                var uniqueName = $"profile_{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, uniqueName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fotoPerfil.CopyTo(stream);
                }
                usuario.FotoPerfil = "/uploads/profiles/" + uniqueName;
            }

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return Ok(new { message = "Cadastro realizado com sucesso!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Erro ao cadastrar: {ex.Message}" });
        }
    }
    
    [HttpPost("Login")] // Define que este método será acessado via POST em /Usuario/Login
    public IActionResult Login([FromBody] Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Senha))
            return BadRequest(new { message = "E-mail e senha são obrigatórios." });

        var usuarioExistente = _context.Usuarios
            .FirstOrDefault(u => (u.Email ?? "").ToLower() == (usuario.Email ?? "").ToLower());

        if (usuarioExistente == null)
            return BadRequest(new { message = "Usuário não encontrado." });

        if (usuarioExistente.Senha != usuario.Senha)
            return BadRequest(new { message = "Senha incorreta." });

        // Retorne idusuario, nome e foto de perfil
        return Ok(new {
            idusuario = usuarioExistente.IdUsuario,
            nome = usuarioExistente.Nome,
            fotoPerfil = usuarioExistente.FotoPerfil
        });
    }

    [HttpPost("UploadFotoPerfil")]
    public IActionResult UploadFotoPerfil([FromForm] int userId, [FromForm] IFormFile foto)
    {
        try
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == userId);
            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado." });

            if (foto != null && foto.Length > 0)
            {
                var uploads = Path.Combine("wwwroot/uploads/profiles");
                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                // Gera um nome único para a foto
                var ext = Path.GetExtension(foto.FileName);
                var uniqueName = $"profile_{userId}_{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploads, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    foto.CopyTo(stream);
                }

                // Remove foto anterior se existir
                if (!string.IsNullOrEmpty(usuario.FotoPerfil))
                {
                    var oldPhotoPath = Path.Combine("wwwroot", usuario.FotoPerfil.TrimStart('/'));
                    if (System.IO.File.Exists(oldPhotoPath))
                    {
                        System.IO.File.Delete(oldPhotoPath);
                    }
                }

                usuario.FotoPerfil = "/uploads/profiles/" + uniqueName;
                _context.SaveChanges();

                return Ok(new { 
                    message = "Foto de perfil atualizada com sucesso!",
                    fotoPerfil = usuario.FotoPerfil
                });
            }

            return BadRequest(new { message = "Nenhuma foto foi enviada." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Erro ao fazer upload da foto: " + ex.Message });
        }
    }

    [HttpGet("ExisteEmail")]
    public IActionResult ExisteEmail([FromQuery] string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest(new { exists = false });

        var existe = _context.Usuarios.Any(u => u.Email != null && u.Email.ToLower() == email.ToLower());
        return Ok(new { exists = existe });
    }
}
