// Services/ApiService.cs
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace CinemaClient.Services;

public class ApiService
{
    private readonly HttpClient _http;
    public string? Jwt { get; private set; }

    public string? GetUserRole()
    {
        if (string.IsNullOrEmpty(Jwt)) return null;

        if (string.IsNullOrEmpty(Jwt))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(Jwt); // Декодируем токен без проверки подписи
        return token.Claims
                   .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // Ищем claim с ролью
    }

    public ApiService(string baseUrl)
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
        // доверяем self-signed, если вдруг понадобится:
        // ServicePointManager.ServerCertificateValidationCallback += (_,_,_,_) => true;
    }

    public async Task<bool> LoginAsync(string login, string password)
    {
        var resp = await _http.PostAsJsonAsync("/auth/login", new { login, password });
        if (!resp.IsSuccessStatusCode) return false;

        Jwt = (await resp.Content.ReadFromJsonAsync<LoginResponse>())!.Token;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);
        return true;
    }

    public async Task<string?> RegisterAsync(string login, string pass, string pass2,
                                         string first, string last, string email)
    {
        var body = new
        {
            Login = login,
            Password = pass,
            ConfirmPassword = pass2,
            FirstName = first,
            LastName = last,
            Email = email
        };

        var resp = await _http.PostAsJsonAsync("/auth/register", body);

        // 200 – успех
        if (resp.IsSuccessStatusCode) return null;

        // извлекаем текст ошибки
        string msg = await resp.Content.ReadAsStringAsync();
        return resp.StatusCode switch
        {
            HttpStatusCode.Conflict => "Пользователь с таким логином уже существует",
            HttpStatusCode.BadRequest => "Пароли не совпадают",
            _ => $"Сервер вернул ошибку: {msg}"
        };
    }

    public async Task<IEnumerable<SessionDto>> GetSessionsAsync()
        => await _http.GetFromJsonAsync<IEnumerable<SessionDto>>("/sessions")!;

    public async Task<IEnumerable<SeatDto>> GetSeatsAsync(int sessionId)
        => await _http.GetFromJsonAsync<IEnumerable<SeatDto>>($"/sessions/{sessionId}/seats")!;

    public async Task<int> BookAsync(int ticketId)
        => (await _http.PostAsJsonAsync("/booking/book", new { ticketId }))
           .StatusCode == System.Net.HttpStatusCode.OK ? 0 : -1;

    public async Task<int> BuyAsync(int ticketId)
        => (await _http.PostAsJsonAsync("/booking/buy", new { ticketId }))
           .StatusCode == System.Net.HttpStatusCode.OK ? 0 : -1;

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    => await _http.GetFromJsonAsync<IEnumerable<UserDto>>("/users")!;

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var response = await _http.DeleteAsync($"/users/{userId}");
        return response.IsSuccessStatusCode;
    }

    private record LoginResponse(string Token);
}

public record SessionDto(int SessionId, DateTime SessionDateTime,
                         string HallNumber, string MovieTitle, decimal Price);

public record SeatDto(int TicketId, int Row, int Number, string Status);

public record UserDto(
    int UserId,
    string Login,
    string Role,
    byte[] PasswordHash
);

