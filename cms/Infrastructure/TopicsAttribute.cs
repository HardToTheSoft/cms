namespace Cms.Services;


public class TopicsAttribute : Attribute
{
  #region Constructor
  public TopicsAttribute(params string[] topics)
  {
    Topics = topics ?? [];
  }
  #endregion


  #region Properties
  public string[] Topics { get; }
  #endregion
}