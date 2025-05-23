// Services/ApiService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CinemaClient.Services;

public class ApiService
{
    private readonly HttpClient _http;
    public string? Jwt { get; private set; }

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
                         string HallNumber, string MovieTitle, decimal Price);

public record SeatDto(int TicketId, int Row, int Number, string Status);
