public class NotificationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public NotificationService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task Send(Job job,string CompanyName)
    {
        var token = _config["BOT_TOKEN"];
        var chatId = _config["CHAT_ID"];

        var msg = $"🚀 Role : {job.Title}\nCompany : {CompanyName}\n Location : {job.Location}\nApply Link : {job.Url}\nPosted on : {job.CreatedAt}";

        var url = $"https://api.telegram.org/bot{token}/sendMessage" +
                  $"?chat_id={chatId}&text={Uri.EscapeDataString(msg)}";

        await _httpClient.GetAsync(url);
    }
}