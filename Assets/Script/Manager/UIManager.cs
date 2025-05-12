using UnityEngine;
using System.Collections.Generic;
using System;

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
/// 2. 打开UI：UIManager.OpenUI(UIIds.Main.MainMenu);
/// 3. 关闭UI：UIManager.CloseUI(UIIds.Main.MainMenu);
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
    /// UI层级节点字典
    /// </summary>
    private static Dictionary<UILayer, Transform> _layerDict;

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
        _layerDict = new Dictionary<UILayer, Transform>();

        // 初始化UI根节点
        GameObject root = GameObject.Find("UIRoot");
        if (root == null)
        {
            root = new GameObject("UIRoot");
            GameObject.DontDestroyOnLoad(root);
        }
        _uiRoot = root.transform;

        // 初始化所有层级节点
        InitializeLayers();

        _isInitialized = true;
        Debug.Log("UIManager 初始化完成");
    }

    /// <summary>
    /// 初始化所有UI层级节点
    /// </summary>
    private static void InitializeLayers()
    {
        // 创建所有层级节点
        foreach (UILayer layer in Enum.GetValues(typeof(UILayer)))
        {
            GameObject layerObj = new GameObject(layer.ToString());
            layerObj.transform.SetParent(_uiRoot, false);
            _layerDict[layer] = layerObj.transform;
        }
    }

    /// <summary>
    /// 打开UI界面
    /// </summary>
    /// <param name="uiData">UI数据</param>
    /// <param name="param">打开参数</param>
    public static void OpenUI(UIData uiData, object param = null)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return;
        }

        // 如果UI已经打开，直接返回
        if (_uiDict.ContainsKey(uiData.Id))
        {
            Debug.LogWarning($"UI {uiData.Id} 已经打开");
            return;
        }

        // 获取UI层级
        Transform parent = _layerDict[uiData.Layer];

        // 加载UI预制体
        GameObject uiObj = Resources.Load<GameObject>(uiData.PrefabPath);
        if (uiObj == null)
        {
            Debug.LogError($"UI {uiData.Id} 预制体不存在: {uiData.PrefabPath}");
            return;
        }

        // 实例化UI
        

    }

    /// <summary>
    /// 关闭UI界面
    /// </summary>
    /// <param name="uiData">UI数据</param>
    public static void CloseUI(UIData uiData)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return;
        }

        if (!_uiDict.ContainsKey(uiData.Id))
        {
            Debug.LogWarning($"UI {uiData.Id} 未打开");
            return;
        }

        // 销毁UI对象
        GameObject ui = _uiDict[uiData.Id];
        GameObject.Destroy(ui);
        _uiDict.Remove(uiData.Id);
    }

    /// <summary>
    /// 获取UI对象
    /// </summary>
    /// <param name="uiData">UI数据</param>
    /// <returns>UI对象</returns>
    public static GameObject GetUI(UIData uiData)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return null;
        }

        if (_uiDict.TryGetValue(uiData.Id, out GameObject ui))
        {
            return ui;
        }
        return null;
    }

    /// <summary>
    /// 判断UI是否打开
    /// </summary>
    /// <param name="uiData">UI数据</param>
    /// <returns>是否打开</returns>
    public static bool IsUIOpen(UIData uiData)
    {
        if (!_isInitialized)
        {
            Debug.LogError("UIManager 未初始化");
            return false;
        }

        return _uiDict.ContainsKey(uiData.Id);
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
