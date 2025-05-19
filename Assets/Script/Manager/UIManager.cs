using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace GameFramework
{
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
            GameObject root = GameObject.Find("UICanvas");
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
                
                // 设置RectTransform
                RectTransform rectTransform = layerObj.AddComponent<RectTransform>();
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
                
                _layerDict[layer] = layerObj.transform;
            }
        }

        /// <summary>
        /// 从UIIds中获取UI配置
        /// </summary>
        /// <param name="uiId">UI唯一标识</param>
        /// <returns>预制体路径</returns>
        private static string GetUIConfig(string uiId)
        {
            return UIIds.GetUIPath(uiId);
        }

        /// <summary>
        /// 打开UI界面
        /// </summary>
        /// <param name="uiId">UI唯一标识</param>
        /// <param name="param">打开参数</param>
        public static async void OpenUI(string uiId, object param = null)
        {
            if (!_isInitialized)
            {
                Debug.LogError("UIManager 未初始化");
                return;
            }

            // 如果UI已经打开，直接返回
            if (_uiDict.ContainsKey(uiId))
            {
                Debug.LogWarning($"UI {uiId} 已经打开");
                return;
            }

            // 从UIIds中获取UI配置
            string prefabPath = GetUIConfig(uiId);
            if (string.IsNullOrEmpty(prefabPath))
            {
                Debug.LogError($"UI {uiId} 配置不存在");
                return;
            }

            // 使用UIViewFactory异步创建UI实例
            var ui = await UIViewFactory.CreateUIViewAsync(uiId, prefabPath);
            if (ui == null)
            {
                Debug.LogError($"UI {uiId} 创建失败");
                return;
            }

            // 获取UI组件并设置层级
            var baseComponent = ui.GetComponent<UIBaseComponent>();
            var baseView = ui.GetComponent<UIBaseView>();
            if (baseComponent != null)
            {
                var parent = _layerDict[baseComponent.UILayer];
                ui.transform.SetParent(parent, false);
                
                // 调用初始化方法
                baseView.OnInit();
                // 调用显示方法
                baseView.OnShow();
            }
            else
            {
                Debug.LogError($"UI {uiId} 未找到UIBaseComponent组件");
                return;
            }

            ui.name = uiId;

            // 添加到字典
            _uiDict[uiId] = ui;
        }

        /// <summary>
        /// 关闭UI界面
        /// </summary>
        /// <param name="uiId">UI唯一标识</param>
        public static void CloseUI(string uiId)
        {
            if (!_isInitialized)
            {
                Debug.LogError("UIManager 未初始化");
                return;
            }

            if (!_uiDict.ContainsKey(uiId))
            {
                Debug.LogWarning($"UI {uiId} 未打开");
                return;
            }

            // 获取UI对象
            GameObject ui = _uiDict[uiId];
            
            // 调用关闭方法
            var baseView = ui.GetComponent<UIBaseView>();
            if (baseView != null)
            {
                baseView.OnClose();
            }

            // 销毁UI对象
            GameObject.Destroy(ui);
            _uiDict.Remove(uiId);
        }

        /// <summary>
        /// 获取UI对象
        /// </summary>
        /// <param name="uiId">UI唯一标识</param>
        /// <returns>UI对象</returns>
        public static GameObject GetUI(string uiId)
        {
            if (!_isInitialized)
            {
                Debug.LogError("UIManager 未初始化");
                return null;
            }

            if (_uiDict.TryGetValue(uiId, out GameObject ui))
            {
                return ui;
            }
            return null;
        }

        /// <summary>
        /// 判断UI是否打开
        /// </summary>
        /// <param name="uiId">UI唯一标识</param>
        /// <returns>是否打开</returns>
        public static bool IsUIOpen(string uiId)
        {
            if (!_isInitialized)
            {
                Debug.LogError("UIManager 未初始化");
                return false;
            }

            return _uiDict.ContainsKey(uiId);
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
}
