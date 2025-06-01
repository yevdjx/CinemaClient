using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace CinemaClient.Services;

public class ApiService
{
    private readonly HttpClient _http;
    public string? Jwt { get; private set; }

    public ApiService(string baseUrl)
    {
        // инициализация HttpClient с базовым адресом API
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    // ==================== АУТЕНТИФИКАЦИЯ И ПОЛЬЗОВАТЕЛЬ ====================

    // метод для входа пользователя в систему
    public async Task<bool> LoginAsync(string login, string password)
    {
        // отправляем логин и пароль на сервер для аутентификации
        var resp = await _http.PostAsJsonAsync("/auth/login", new { login, password });
        if (!resp.IsSuccessStatusCode) return false;

        // сохраняем JWT токен и устанавливаем заголовок авторизации
        Jwt = (await resp.Content.ReadFromJsonAsync<LoginResponse>())!.Token;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Jwt);
        return true;
    }

    // метод для регистрации нового пользователя
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

    // метод для получения роли текущего пользователя из JWT токена
    public string? GetUserRole()
    {
        if (string.IsNullOrEmpty(Jwt)) return null;

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(Jwt); // декодируем токен без проверки подписи
        return token.Claims
                   .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // ищем claim с ролью
    }

    // метод для получения email текущего пользователя
    public async Task<string> GetUserEmailAsync()
    {
        var response = await _http.GetAsync("api/user/email");
        return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
    }

    // метод для получения имени текущего пользователя
    public async Task<string> GetUserNameAsync()
    {
        var response = await _http.GetAsync("api/user/name");
        return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "Гость";
    }

    // ==================== ФИЛЬМЫ (ДЛЯ ВСЕХ ПОЛЬЗОВАТЕЛЕЙ) ====================

    // метод для получения списка всех фильмов
    public async Task<IEnumerable<MovieDto>> GetMoviesAsync()
    {
        // отправляем GET-запрос для получения списка всех фильмов
        // возвращаем пустой список, если ответ null
        return await _http.GetFromJsonAsync<IEnumerable<MovieDto>>("/admin/movies")
               ?? Enumerable.Empty<MovieDto>();
    }

    // метод для получения изображения фильма по его ID
    public async Task<Image> GetMovieImageAsync(int movieId)
    {
        try
        {
            var response = await _http.GetAsync($"api/movies/{movieId}/movie_image");
            response.EnsureSuccessStatusCode();

            // получаем содержимое ответа в виде потока
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                // преобразуем поток в изображение
                return Image.FromStream(stream);
            }
        }
        catch (HttpRequestException ex)
        {
            // обработка ошибок запроса
            Console.WriteLine($"Ошибка при получении изображения: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            // обработка других ошибок
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
            return null;
        }
    }

    // ==================== СЕАНСЫ (ДЛЯ ВСЕХ ПОЛЬЗОВАТЕЛЕЙ) ====================

    // метод для получения списка всех сеансов
    public async Task<IEnumerable<SessionDto>> GetSessionsAsync()
        => await _http.GetFromJsonAsync<IEnumerable<SessionDto>>("/sessions")!;

    // метод для получения информации о конкретном сеансе по его ID
    public async Task<SessionDto> GetSessionAsync(int sessionId)
    {
        try
        {
            var response = await _http.GetAsync($"api/sessions/{sessionId}");
            if (response.IsSuccessStatusCode)
            {
                var session = await response.Content.ReadFromJsonAsync<SessionDto>();
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

    // метод для получения списка мест в зале для конкретного сеанса
    public async Task<IEnumerable<SeatDto>> GetSeatsAsync(int sessionId)
        => await _http.GetFromJsonAsync<IEnumerable<SeatDto>>($"/sessions/{sessionId}/seats")!;

    // ==================== БРОНИРОВАНИЕ И ПОКУПКА БИЛЕТОВ ====================

    // метод для бронирования билета
    public async Task<int> BookAsync(int ticketId)
        => (await _http.PostAsJsonAsync("/booking/book", new { ticketId }))
           .StatusCode == System.Net.HttpStatusCode.OK ? 0 : -1;

    // метод для покупки билета
    public async Task<int> BuyAsync(int ticketId)
        => (await _http.PostAsJsonAsync("/booking/buy", new { ticketId }))
           .StatusCode == System.Net.HttpStatusCode.OK ? 0 : -1;

    // метод для подтверждения бронирования
    public async Task<bool> ConfirmBookingAsync(int ticketId)
    {
        var response = await _http.PostAsync($"api/tickets/{ticketId}/confirm", null);
        return response.IsSuccessStatusCode;
    }

    // метод для отмены бронирования
    public async Task<int> CancelBookingAsync(int ticketId)
         => (await _http.DeleteAsync($"/booking/book/{ticketId}")).StatusCode == System.Net.HttpStatusCode.OK ? 0 : -1;

    // метод для получения билетов текущего пользователя
    public async Task<IEnumerable<UserTicketDto>> GetUserTicketsAsync() =>
        await _http.GetFromJsonAsync<IEnumerable<UserTicketDto>>("/booking/my")!;

    // ==================== АДМИНИСТРИРОВАНИЕ ФИЛЬМОВ ====================

    // метод для получения информации о конкретном фильме (админ)
    public async Task<MovieDto?> GetMovieAsync(int movieId)
    {
        // отправляем GET-запрос для получения информации о фильме по его ID
        return await _http.GetFromJsonAsync<MovieDto>($"/admin/movies/{movieId}");
    }

    // метод для создания нового фильма (админ)
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
            MovieImage = Img
        };

        var response = await _http.PostAsJsonAsync("/admin/movies", movie);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // метод для обновления информации о фильме (админ)
    public async Task<(bool Success, string? Error)> UpdateMovieAsync(
        int movieId,
        string title,
        int durationMinutes,
        string director,
        string ageRestriction,
        byte[] Img)
    {
        var movie = new
        {
            movieId,
            movieTitle = title,
            movieDuration = durationMinutes,
            movieAuthor = director,
            movieAgeRating = ageRestriction,
            movieImage = Img
        };

        var response = await _http.PutAsJsonAsync($"/admin/movies/{movieId}", movie);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // метод для удаления фильма (админ)
    public async Task<(bool Success, string? Error)> DeleteMovieAsync(int movieId)
    {
        var response = await _http.DeleteAsync($"/admin/movies/{movieId}");

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // ==================== АДМИНИСТРИРОВАНИЕ СЕАНСОВ ====================

    // метод для получения списка сеансов (админ)
    public async Task<IEnumerable<SessionAdminDto>> GetSessionsAdminAsync()
    {
        return await _http.GetFromJsonAsync<IEnumerable<SessionAdminDto>>("/admin/sessions")
               ?? Enumerable.Empty<SessionAdminDto>();
    }

    // метод для создания нового сеанса (админ)
    public async Task<(bool Success, string? Error)> CreateSessionAsync(
        int hallId,
        int movieId,
        DateTime sessionDateTime,
        int price)
    {
        var session = new
        {
            hallId,
            movieId,
            sessionDateTime,
            sessionPrice = price
        };

        var response = await _http.PostAsJsonAsync("/admin/sessions", session);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // метод для обновления информации о сеансе (админ)
    public async Task<(bool Success, string? Error)> UpdateSessionAsync(
        int sessionId,
        int hallId,
        int movieId,
        DateTime sessionDateTime,
        int price)
    {
        var session = new
        {
            sessionId,
            hallId,
            movieId,
            sessionDateTime,
            sessionPrice = price
        };

        var response = await _http.PutAsJsonAsync($"/admin/sessions/{sessionId}", session);

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    // метод для удаления сеанса (админ)
    public async Task<(bool Success, string? Error)> DeleteSessionAsync(int sessionId)
    {
        var response = await _http.DeleteAsync($"/admin/sessions/{sessionId}");

        if (response.IsSuccessStatusCode)
            return (true, null);

        var error = await response.Content.ReadAsStringAsync();
        return (false, $"Ошибка: {response.StatusCode} - {error}");
    }

    private record LoginResponse(string Token);
}

// ==================== DTO КЛАССЫ ====================

// класс для представления информации о сеансе
public record SessionDto(
    int SessionId,
    DateTime SessionDateTime,
    string HallNumber,
    string MovieTitle,
    byte[] MovieImage,
    decimal Price);

// класс для представления информации о месте в зале
public record SeatDto(int TicketId, int Row, int Number, string Status);

// класс для представления информации о пользователе
public record UserDto(
    int UserId,
    string Login,
    string Role,
    byte[] PasswordHash
);

// класс для представления информации о фильме
public record MovieDto(
    int movieId,
    string movieTitle,
    int movieDuration,
    string movieAuthor,
    string movieAgeRating,
    byte[] movieImage
);

// класс для представления информации о корзине
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

// класс для представления информации о зале
public record HallDto(
    int hallId,
    string hallNumber
);

// класс для представления информации о сеансе (админ)
public record SessionAdminDto(
    int sessionId,
    int hallId,
    string hallNumber,
    int movieId,
    string movieTitle,
    string movieImage,
    DateTime sessionDateTime,
    decimal sessionPrice
);

// класс для представления информации о билете пользователя
public record UserTicketDto(
    int TicketId,
    int Flag,            // 1 = будет, 0 = прошло/бронь
    string Status,          // sold | booked
    string MovieTitle,
    byte[]? MovieImage,
    string HallNumber,
    int Row,
    int Seat,
    DateTime SessionDateTime,
    decimal Price);