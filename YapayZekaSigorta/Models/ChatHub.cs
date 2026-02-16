using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YapayZekaSigorta.Models
{
    public class ChatHub : Hub
    {
        private const string apiKey = "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";

        private const string modelName = "gpt-4o-mini";

        private readonly IHttpClientFactory httpClientFactory;

        public ChatHub(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        private static readonly Dictionary<string, List<Dictionary<string, string>>> _history = new();
        //kullanıcı girdiğinde bir id tanımlar ve ona sen bir asistansın talimatını ekler
        public override Task OnConnectedAsync()
        {
            _history[Context.ConnectionId] =
            [
                new()
                {
                    ["role"]="system",
                    ["content"]="You are a helpful assistant. Keep answers concise."
                }
            ];

            return base.OnConnectedAsync();
        }
        //bağlantı koptuğunda atanan benzersiz id silinecek
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _history.Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }
        //kullanıcı mesaj yazdığında çalışır
        public async Task SendMessage(string userMessage)
        {
            await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);//receiveuserecho sayesinde yazılan mesajı ilk kendisi görür

            var history = _history[Context.ConnectionId];//ve yazılan bu mesajı hafızaya kaydeder
            history.Add(new() { ["role"] = "user", ["content"] = userMessage });

            await StreamOpenAI(history, Context.ConnectionAborted);
        }

        private async Task StreamOpenAI(List<Dictionary<string, string>> history, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("openai");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                model = modelName,
                messages = history,
                stream = true,
                temperature = 0.2
            };

            using var req = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            resp.EnsureSuccessStatusCode();

            using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            var sb = new StringBuilder();
            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();//satırı oku
                if (string.IsNullOrWhiteSpace(line)) continue;//satır boşsa geçiyoruz
                if (!line.StartsWith("data:")) continue; //openaı her satır başına data: koyar. eğer satır o ifadeyle başlamıyorsa da onu geçiyoruz

                var data = line["data:".Length..].Trim();
                if (data == "[DONE]") break;//openaı cevabı bitirdiğinde son satır olarak data:[DONE] gönderir. bunu gördüğümüz an döngüden çıkıyoruz.

                try
                {
                    var chunk = JsonSerializer.Deserialize<ChatStreamChunk>(data);
                    var delta = chunk?.Choices?.FirstOrDefault()?.Delta?.Content;//o an yazılan kelimeyi anında çeker
                    if (!string.IsNullOrEmpty(delta))//bu döngüde yazılan o kelimeyi anlık olarak kullanıcıya gösterir
                    {
                        sb.AppendLine(delta);
                        await Clients.Caller.SendAsync("ReceiveToken", delta, cancellationToken);
                    }
                }
                catch
                {
                    //hata
                }
            }

            var full = sb.ToString();//döngü boyunca tüm gelen parçalar burda birleştirildi
            history.Add(new() { ["role"] = "assistant", ["content"] = full });//ai'in verdiği tam cevabı hafızaya ekliyoruz
            await Clients.Caller.SendAsync("CompleteMessage", full, cancellationToken);//işlemin bittiğini istemciye söylüyoruz

        }

        private sealed class ChatStreamChunk
        {
            [JsonPropertyName("choices")] public List<Choice>? Choices { get; set; }
        }

        private sealed class Choice
        {
            [JsonPropertyName("delta")] public Delta? Delta { get; set; }
            [JsonPropertyName("finish_reason")] public string? FinishReason { get; set; }
        }

        private sealed class Delta
        {
            [JsonPropertyName("content")] public string? Content { get; set; }
            [JsonPropertyName("role")] public string? Role { get; set; }
        }

    }
}
