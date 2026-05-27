using System.Text.Json.Serialization;

namespace Hydra.Infrastructure.Api;

public record HydrationEntryDto
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("userId")]
    public int UserId { get; init; }

    [JsonPropertyName("amountMl")]
    public int AmountMl { get; init; }

    [JsonPropertyName("intakeTime")]
    public DateTime IntakeTime { get; init; }

    [JsonPropertyName("lastModifiedUtc")]
    public DateTime LastModifiedUtc { get; init; }
}
