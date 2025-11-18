using FluentAssertions;
using MediaAccess.Application.Ports.Inbound.Contracts;
using MediaAccess.Application.UseCases;
using MediaAccess.UnitTests.Common.Doubles;

namespace MediaAccess.UnitTests.Application.UseCases;

public sealed class RequestImageAccessTests
{
  // Happy path
  [Fact]
  public async Task ExecuteAsync_ValidKey_CallsStorage_AndReturnsSuccess()
  {
    const string url = "https://storage.example.com/presigned";
    var expiresAt = DateTimeOffset.Parse("2030-01-01T00:00:00Z");

    var storage = new FakeStoragePresignClient
    {
      UrlToReturn = url,
      ExpiresAtToReturn = expiresAt
    };

    var sut = new RequestImageAccess(storage);

    var objectKey = "images/2030/01/01/sample.jpg";
    var cmd = new RequestImageAccessCommand(objectKey);

    var result = await sut.ExecuteAsync(cmd);

    var success = result as RequestImageAccessResult.Success;
    success.Should().NotBeNull();
    success!.Url.Should().Be(url);
    success.ExpiresAt.Should().Be(expiresAt);

    storage.LastGetRequest.Should().NotBeNull();
    storage.LastGetRequest!.Key.Should().Be(objectKey);
    storage.LastGetRequest!.TtlSec.Should().Be(300);
  }

  // Validation: invalid key (empty or whitespace)
  [Theory]
  [InlineData("")]
  [InlineData("    ")]
  public async Task ExecuteAsync_InvalidObjectKey_ReturnsInvalid_AndDoesNotCallStorage(string input)
  {
    var storage = new FakeStoragePresignClient();
    var sut = new RequestImageAccess(storage);

    var cmd = new RequestImageAccessCommand(input);

    var result = await sut.ExecuteAsync(cmd);

    var invalid = result as RequestImageAccessResult.Invalid;
    invalid.Should().NotBeNull();
    invalid!.Errors.Should().ContainKey("objectKey");

    storage.LastGetRequest.Should().BeNull();
  }


  // Note:
  // Cancellation and exceptional paths are not covered for the semester scope.
}
