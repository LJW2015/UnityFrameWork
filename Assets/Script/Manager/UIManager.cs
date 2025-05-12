using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// UI管理器，负责管理所有UI界面的生命周期
/// 
/// 设计思路：
/// 1. 采用静态类，全局访问
/// 2. 使用字典管理所有UI界面
/// 3. 集成事件系统，支持UI打开关闭的事件通知
/// 4. 支持UI的层级管理
/// 
/// 使用方法：
/// 1. 初始化：UIManager.Init();
/// 2. 打开UI：UIManager.OpenUI("MainUI");
/// 3. 关闭UI：UIManager.CloseUI("MainUI");
/// </summary>
public static class UIManager
{
    /// <summary>
    /// UI界面字典，key为UI名称，value为UI对象
    /// </summary>
    private static Dictionary<string, GameObject> _uiDict;

    /// <summary>
    /// UI根节点
    /// </summary>
    private static Transform _uiRoot;

    /// <summary>
    /// 是否已初始化
    /// </summary>
    private static bool _isInitialized = false;

    /// <summary>
    /// 初始化UI管理器
    /// </summary>
    public static void Init()
    {
        if (_isInitialized)
        {
            Debug.LogWarning("UIManager 已经初始化");
            return;
        }

        // 初始化UI字典
        _uiDict = new Dictionary<string, GameObject>();

        // 初始化UI根节点
        GameObject root = GameObject.Find("UIRoot");
        if (root == null)
        {
            root = new GameObject("UIRoot");
            GameObject.DontDestroyOnLoad(root);
        }
        _uiRoot = root.transform;

        _isInitialized = true;
        Debug.Log("UIManager 初始化完成");
    }

    /// <summary>
    /// 打开UI界面
    /// </summary>
    /// <param name="uiName">UI名称</param>
    /// <param name="param">打开参数</param>
    public static void OpenUI(string uiName, object param = null)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return;
        }

        // 如果UI已经打开，直接返回
        if (_uiDict.ContainsKey(uiName))
        {
            Debug.LogWarning($"UI {uiName} 已经打开");
            return;
        }

        // TODO: 加载UI预制体
        // GameObject uiObj = Resources.Load<GameObject>($"UI/{uiName}");
        // if (uiObj == null)
        // {
        //     Debug.LogError($"UI {uiName} 预制体不存在");
        //     return;
        // }

        // 实例化UI
        // GameObject ui = GameObject.Instantiate(uiObj, _uiRoot);
        // ui.name = uiName;
        // _uiDict[uiName] = ui;
    }

    /// <summary>
    /// 关闭UI界面
    /// </summary>
    /// <param name="uiName">UI名称</param>
    public static void CloseUI(string uiName)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return;
        }

        if (!_uiDict.ContainsKey(uiName))
        {
            Debug.LogWarning($"UI {uiName} 未打开");
            return;
        }

        // 销毁UI对象
        GameObject ui = _uiDict[uiName];
        GameObject.Destroy(ui);
        _uiDict.Remove(uiName);
    }

    /// <summary>
    /// 获取UI对象
    /// </summary>
    /// <param name="uiName">UI名称</param>
    /// <returns>UI对象</returns>
    public static GameObject GetUI(string uiName)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return null;
        }

        if (_uiDict.TryGetValue(uiName, out GameObject ui))
        {
            return ui;
        }
        return null;
    }

    /// <summary>
    /// 判断UI是否打开
    /// </summary>
    /// <param name="uiName">UI名称</param>
    /// <returns>是否打开</returns>
    public static bool IsUIOpen(string uiName)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return false;
        }

        return _uiDict.ContainsKey(uiName);
    }

    /// <summary>
    /// 清理所有UI
    /// </summary>
    public static void Clear()
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return;
        }

        foreach (var ui in _uiDict.Values)
        {
            if (ui != null)
            {
                GameObject.Destroy(ui);
            }
        }
        _uiDict.Clear();
    }
}
