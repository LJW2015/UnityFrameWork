public class UILoadConfig
{
    public string Id;  // UI唯一标识
    public UILayer Layer; // UI层级
    public string PrefabPath; // UI预制体路径
}

public enum UILayer{
    Background,
    Main,
    Popup,
    Tips,
    Loading,
}

/// <summary>
/// UI唯一标识管理类，用于统一管理所有UI的唯一标识
/// </summary>
public static class UIIds
{
    /// <summary>
    /// 主界面
    /// </summary>
    public readonly static UILoadConfig UIMain = new()
    {
        Id = "UIMain",
        Layer = UILayer.Main,
        PrefabPath = "UI/UIMain"
    };
}
