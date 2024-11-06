using Microsoft.AspNetCore.Mvc;
using System.Linq;
using API.Data;

[Route("api/[controller]")]
[ApiController]
public class JWTController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly ApplicationDbContext _context;

    public JWTController(JwtService jwtService, ApplicationDbContext context)
    {
        _jwtService = jwtService;
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Проверка логина и пароля в таблице employees
        var user = _context.employees
            .SingleOrDefault(e => e.login == model.Username && e.password == model.Password);

        if (user == null)
        {
            return Unauthorized("Неверный логин или пароль");
        }

        var token = _jwtService.GenerateToken(user);
        return Ok(new { token });
    }
}

public class LoginModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
