public class UIData
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
    public readonly static UIData UIMain = new()
    {
        Id = "UIMain",
        Layer = UILayer.Main,
        PrefabPath = "UI/Main/MainPanel"
    };

    /// <summary>
    /// 登录界面
    /// </summary>
    public readonly static UIData UILogin = new()
    {
        Id = "UILogin",
        Layer = UILayer.Main,
        PrefabPath = "UI/Login/LoginPanel"
    };

    /// <summary>
    /// 设置界面
    /// </summary>
    public readonly static UIData UISetting = new()
    {
        Id = "UISetting",
        Layer = UILayer.Popup,
        PrefabPath = "UI/Setting/SettingPanel"
    };

    /// <summary>
    /// 提示界面
    /// </summary>
    public readonly static UIData UITips = new()
    {
        Id = "UITips",
        Layer = UILayer.Tips,
        PrefabPath = "UI/Tips/TipsPanel"
    };

    /// <summary>
    /// 加载界面
    /// </summary>
    public readonly static UIData UILoading = new()
    {
        Id = "UILoading",
        Layer = UILayer.Loading,
        PrefabPath = "UI/Loading/LoadingPanel"
    };
}
