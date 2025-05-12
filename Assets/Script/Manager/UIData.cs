using UnityEngine;

/// <summary>
/// UI数据类，存储UI的完整信息
/// </summary>
[System.Serializable]
public class UIData
{
    /// <summary>
    /// UI唯一标识
    /// </summary>
    public string Id;

    /// <summary>
    /// UI层级
    /// </summary>
    public UILayer Layer;

    /// <summary>
    /// UI预制体路径
    /// </summary>
    public string PrefabPath;

    /// <summary>
    /// UI是否常驻内存
    /// </summary>
    public bool IsPersistent;

    /// <summary>
    /// UI是否缓存
    /// </summary>
    public bool IsCache;

    /// <summary>
    /// UI是否全屏
    /// </summary>
    public bool IsFullScreen;

    /// <summary>
    /// UI是否模态
    /// </summary>
    public bool IsModal;

    /// <summary>
    /// UI是否显示背景
    /// </summary>
    public bool ShowBackground;

    /// <summary>
    /// UI背景透明度
    /// </summary>
    public float BackgroundAlpha;

    /// <summary>
    /// UI打开动画
    /// </summary>
    public string OpenAnimation;

    /// <summary>
    /// UI关闭动画
    /// </summary>
    public string CloseAnimation;

    /// <summary>
    /// UI打开音效
    /// </summary>
    public string OpenSound;

    /// <summary>
    /// UI关闭音效
    /// </summary>
    public string CloseSound;
} 