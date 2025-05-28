// Services/ApiService.cs
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;


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
    public async Task<bool> ConfirmBookingAsync(int ticketId)
    {
        var response = await _http.PostAsync($"api/tickets/{ticketId}/confirm", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<string> GetUserEmailAsync()
    {
        var response = await _http.GetAsync("api/user/email");
        return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
    }

    public async Task<string> GetUserNameAsync()
    {
        var response = await _http.GetAsync("api/user/name");
        return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "Гость";
    }
    public async Task<SessionDto> GetSessionAsync(int sessionId)
    {
        try
        {
            var response = await _http.GetAsync($"api/sessions/{sessionId}");
            if (response.IsSuccessStatusCode)
            {
                var session = await response.Content.ReadFromJsonAsync<SessionDto>();

                // Если время приходит в формате DateTime, преобразуем в строку
                if (session != null)
                {
                    // Пример преобразования, если нужно:
                    // session.StartTime = session.StartTimeDateTime.ToString("HH:mm");
                    // session.EndTime = session.EndTimeDateTime.ToString("HH:mm");
                }

                return session;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении информации о сеансе: {ex.Message}");
            return null;
        }
    }

    public async Task<byte[]> GetMovieImageAsync(int movieId)
    {
        try
        {
            var response = await _http.GetAsync($"api/movies/{movieId}/image");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении изображения фильма: {ex.Message}");
            return null;
        }
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

    // ФИЛЬМЫ !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// Получение всех фильмов
    public async Task<IEnumerable<MovieDto>> GetMoviesAsync()
    {
        return await _http.GetFromJsonAsync<IEnumerable<MovieDto>>("/admin/movies")
               ?? Enumerable.Empty<MovieDto>();
    }

    // Получение одного фильма по ID
    public async Task<MovieDto?> GetMovieAsync(int movieId)
    {
        return await _http.GetFromJsonAsync<MovieDto>($"/admin/movies/{movieId}");
    }

    // Создание нового фильма
    public async Task<(bool Success, string? Error)> CreateMovieAsync(
    string title,
    int durationMinutes,
    string director,
    string ageRestriction,
    byte[] Img
        )
    {
        var movie = new
        {
            movieTitle = title,
            movieDuration = durationMinutes,
            movieAuthor = director,
            movieAgeRating = ageRestriction,
            movieImg = Img

        };

        var response = await _http.PostAsJsonAsync("/admin/movies", movie);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    public async Task<(bool Success, string? Error)> UpdateMovieAsync(
        int movieId,
        string title,
        int durationMinutes,
        string director,
        string ageRestriction, byte[] Img)
    {
        var movie = new
        {
            movieId = movieId,
            movieTitle = title,
            movieDuration = durationMinutes,
            movieAuthor = director,
            movieAgeRating = ageRestriction,
            movieImg = Img
            
        };

        var response = await _http.PutAsJsonAsync($"/admin/movies/{movieId}", movie);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }


    // Удаление фильма
    public async Task<(bool Success, string? Error)> DeleteMovieAsync(int movieId)
    {
        var response = await _http.DeleteAsync($"/admin/movies/{movieId}");

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }
    // КИНА НЕ БУДЕТ ФИЛЬМЫ ЗАКОНЧИЛИСЬ 

    // а теперь сеансы!!
    // Обновление существующего сеанса
    public async Task<(bool Success, string? Error)> UpdateSessionAsync(
        int sessionId,
        string hallNumber,
        string movieTitle,
        DateTime sessionDateTime,
        decimal price)
    {
        var session = new
        {
            SessionId = sessionId,
            HallNumber = hallNumber,
            MovieTitle = movieTitle,
            SessionDateTime = sessionDateTime,
            Price = price
        };

        var response = await _http.PutAsJsonAsync($"/admin/sessions/{sessionId}", session);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // Создание нового сеанса
    public async Task<(bool Success, string? Error)> CreateSessionAsync(
        string hallNumber,
        string movieTitle,
        DateTime sessionDateTime,
        decimal price)
    {
        var session = new
        {
            HallNumber = hallNumber,
            MovieTitle = movieTitle,
            SessionDateTime = sessionDateTime,
            Price = price
        };

        var response = await _http.PostAsJsonAsync("/admin/sessions", session);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // Удаление сеанса
    public async Task<(bool Success, string? Error)> DeleteSessionAsync(int sessionId)
    {
        var response = await _http.DeleteAsync($"/admin/sessions/{sessionId}");

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
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

    private record LoginResponse(string Token);
}

public record SessionDto(int SessionId, DateTime SessionDateTime,
                         string HallNumber, string MovieTitle, byte[] MovieImage, decimal Price);


public record SeatDto(int TicketId, int Row, int Number, string Status);

public record UserDto(
    int UserId,
    string Login,
    string Role,
    byte[] PasswordHash
);

public record MovieDto(
    int movieId,
    string movieTitle,
    int movieDuration,
    string movieAuthor,
    string movieAgeRating,
    byte[] movieImg
);

public record KorzinaDto(
    int SessionId,
    int MovieId,
    string MovieTitle,
    string HallName,
    DateTime SessionDate,
    string StartTime,
    string EndTime,
    decimal BasePrice,
    string FormattedDate,
    string FullTimeInfo,
    string FullSessionInfo
);

