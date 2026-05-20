using System.Net;
using System.Text;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Mvc.Testing;

using Cms.Api.Authentication;
using Cms.Application.Models;


namespace Cms.Tests;


[TestClass]
public class BasicAuthenticationHandlerIntegrationTests
{
  #region Members
  private const string GET_HTTP_METHOD = "GET";
  private const string POST_HTTP_METHOD = "POST";

  private static JsonContent _defaultCmsEventsJsonContent = JsonContent.Create(new List<EventModel>
  {
    new()
    {
      Id = "1",
      Type = "event"
    }
  });

  private WebApplicationFactory<Program> _webApplicationFactory;

  private HttpClient _httpClient;

  #endregion


  #region Properties
  public TestContext TestContext { get; set; }
  #endregion


  #region Public methods
  [TestInitialize]
  public void Setup()
  {
    _webApplicationFactory = new WebApplicationFactory<Program>();

    _httpClient = _webApplicationFactory.CreateClient();
  }


  [TestMethod]
  [DataRow("user_consummer", "db041de3-afd3-4810-8a5a-d46a5fbc2d31", "/api/entities.json", "GET")]
  [DataRow("admin_consumer", "db041de3-afd3-4810-8a5a-d46a5fbc2d32", "/api/entities.json", "GET")]
  [DataRow("cms_events_processor", "db041de3-afd3-4810-8a5a-d46a5fbc2d33", "/cms/events.json", "POST")]
  public async Task BasicAuthentication_ValidUser_ReturnsSuccess(
    string userName,
    string password,
    string url,
    string httpMethod)
  {
    var httpResponseMessage = await MakeRequest(userName, password, url, httpMethod);

    Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
  }


  [TestMethod]
  [DataRow("user_consummer", "not_valid_guid", "/api/entities.json", "GET")]
  [DataRow("ser__not_valid_user_prefix", "db041de3-afd3-4810-8a5a-d46a5fbc2d31", "/api/entities.json", "GET")]
  [DataRow("admin_consumer", "not_valid_guid", "/api/entities.json", "GET")]
  [DataRow("dmin__not_valid_user_prefix", "db041de3-afd3-4810-8a5a-d46a5fbc2d32", "/api/entities.json", "GET")]
  [DataRow("cms_events_processor", "not_valid_guid", "/cms/events.json", "POST")]
  [DataRow("ms_not_valid_user_prefix", "db041de3-afd3-4810-8a5a-d46a5fbc2d33", "/cms/events.json", "POST")]
  public async Task BasicAuthentication_InvalidUser_ReturnsUnauthorized(
    string userName,
    string password,
    string url,
    string httpMethod)
  {
    var httpResponseMessage = await MakeRequest(userName, password, url, httpMethod);

    Assert.IsTrue(httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized
      || httpResponseMessage.StatusCode == HttpStatusCode.Forbidden);
  }
  #endregion


  #region Private methods
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private string EncodeBasicAuth(string userName, string password)
    => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));


  private async Task<HttpResponseMessage> MakeRequest(
    string userName,
    string password,
    string url,
    string httpMethod)
  {
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BasicAuthenticationHandler.SCHEME_NAME, EncodeBasicAuth(userName, password));

    if (httpMethod.Equals(GET_HTTP_METHOD, StringComparison.OrdinalIgnoreCase))
      return await _httpClient.GetAsync(url, TestContext.CancellationToken);
    else if (httpMethod.Equals(POST_HTTP_METHOD, StringComparison.OrdinalIgnoreCase))
      return await _httpClient.PostAsync(url, _defaultCmsEventsJsonContent, TestContext.CancellationToken);

    throw new NotSupportedException($"Http method {httpMethod} not supported in test.");
  }
  #endregion
}