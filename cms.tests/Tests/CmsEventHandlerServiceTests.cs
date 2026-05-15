using Cms.Models;
using Cms.Services;
using Cms.Tests.Infrastructure;


namespace Cms.Tests;


[TestClass]
public class CmsEventHandlerServiceTests
{
  #region Members
  private TestLogger<CmsEventHandlerService> _logger;

  private CmsEventHandlerService _cmsEventHandler;
  #endregion


  #region Properties
  public TestContext TestContext { get; set; }
  #endregion


  #region Public methods
  [TestInitialize]
  public void Setup()
  {
    _logger = new TestLogger<CmsEventHandlerService>(TestContext);

    _cmsEventHandler = new CmsEventHandlerService(_logger);
  }


  [TestMethod]
  public async Task Process_WithMockDto_LogsAndReturnsExpected()
  {
    await _cmsEventHandler.HandleEventAsync(new CmsEventModel
    {
      Topic = "events/process"
    });

    // bool foundLog = _logger.Messages.TryGetValue("SUCCESS", out _);

    // Assert.IsTrue(foundLog, "Expected log message was not found.");
  }
  #endregion
}