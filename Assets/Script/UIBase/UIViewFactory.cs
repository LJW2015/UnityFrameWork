using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework;

public static class UIViewFactory 
{
    // 存储UI预制体路径和对应逻辑脚本类型的映射
    private static Dictionary<string, System.Type> _uiViewMap = new Dictionary<string, System.Type>();

    // 静态构造函数，在类第一次被访问时执行
    static UIViewFactory()
    {
        // 初始化UI映射关系
        RegisterUIView("UIMain", typeof(TestLogic));
    }

    /// <summary>
    /// 注册UI预制体和对应的逻辑脚本
    /// </summary>
    /// <param name="viewId">预制体名</param>
    /// <param name="viewType">逻辑脚本类型</param>
    public static void RegisterUIView(string viewId, System.Type viewType)
    {
        if (!_uiViewMap.ContainsKey(viewId))
        {
            _uiViewMap.Add(viewId, viewType);
        }
    }
    /// <summary>
    /// 创建UI实例并挂载对应的逻辑脚本
    /// </summary>
    /// <param name="viewId">预制体名</param>
    /// <returns>返回挂载了逻辑脚本的UI实例</returns>
    public static async Task<GameObject> CreateUIViewAsync(string viewId, string prefabPath)
    {
        if (!_uiViewMap.ContainsKey(viewId))
        {
            Debug.LogError($"UI预制体 {viewId} 未注册对应的逻辑脚本");
            return null;
        }

        // 异步加载预制体
        GameObject prefab = await AssetManager.LoadAssetAsync<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"加载预制体失败: {prefabPath}");
            return null;
        }

        // 实例化预制体
        GameObject uiInstance = GameObject.Instantiate(prefab);
        
        // 添加对应的逻辑脚本
        System.Type viewType = _uiViewMap[viewId];
        uiInstance.AddComponent(viewType);

        return uiInstance;
    }
}
