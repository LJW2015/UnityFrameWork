using System.Collections.Generic;

/// <summary>
/// UI唯一标识管理类，用于统一管理所有UI的唯一标识
/// </summary>
public static class UIIds
{
    /// <summary>
    /// UI预制体路径字典
    /// </summary>
    private static readonly Dictionary<string, string> _uiPathDict = new()
    {
        { "UIMain", "Prefab/UI/UIMain" }
    };

    /// <summary>
    /// 获取UI预制体路径
    /// </summary>
    /// <param name="uiId">UI唯一标识</param>
    /// <returns>预制体路径</returns>
    public static string GetUIPath(string uiId)
    {
        return _uiPathDict.TryGetValue(uiId, out string path) ? path : null;
    }
} 