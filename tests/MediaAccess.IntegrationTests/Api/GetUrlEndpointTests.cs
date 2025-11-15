using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MediaAccess.Api.Contracts;
using MediaAccess.IntegrationTests.Common;
using Microsoft.AspNetCore.Http;

namespace MediaAccess.IntegrationTests.Api;

public sealed class GetUrlEndpointTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;

  public GetUrlEndpointTests(CustomWebApplicationFactory factory)
  {
      _client = factory.CreateClient();
  }

  // Happy path
  [Fact]
  public async Task GetUrl_ValidRequest_Returns200_WithUrlAndExpiry()
  {
    var request = new GetUrlRequest("images/2030/01/01/sample.jpg");

    var response = await _client.PostAsJsonAsync("/internal/v0/media/get-url", request);

    response.StatusCode.Should().Be(HttpStatusCode.OK);

    var body = await response.Content.ReadFromJsonAsync<GetUrlResponse>();
    body.Should().NotBeNull();
    body!.Url.Should().Be("https://storage.example.com/presigned");
    body.ExpiresAt.Should().Be(DateTimeOffset.Parse("2030-01-01T00:00:00Z"));
  }


  // Validation: empty/whitespace object key (422)
  [Theory]
  [InlineData("")]
  [InlineData("   ")]
  public async Task GetUrl_InvalidObjectKey_Returns422(string input)
  {
    var request = new GetUrlRequest(input);

    var response = await _client.PostAsJsonAsync("/internal/v0/media/get-url", request);

    response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
  }

}
