using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UILayer
{
    Background,
    Main,
    Popup,
    Tips,
}

public class UIBaseComponent : MonoBehaviour
{
    // UI预制体的基本信息
    [SerializeField] private string _uiName;        // UI名称
    [SerializeField] public UILayer _uiLayer;          // UI层级
    // 组件缓存字典
    private Dictionary<string, Component> _componentCache = new Dictionary<string, Component>();
    
    // 属性访问器
    public string UIName => _uiName;
    public UILayer UILayer => _uiLayer;
    private void Awake()
    {
       
    }
    
    // 获取组件方法
    public T GetComponent<T>(string path = "") where T : Component
    {
        string key = typeof(T).Name + path;
        if (_componentCache.TryGetValue(key, out Component component))
        {
            return component as T;
        }
        
        T targetComponent;
        if (string.IsNullOrEmpty(path))
        {
            targetComponent = GetComponent<T>();
        }
        else
        {
            Transform target = transform.Find(path);
            targetComponent = target?.GetComponent<T>();
        }
        
        if (targetComponent != null)
        {
            _componentCache[key] = targetComponent;
        }
        
        return targetComponent;
    }
    
    // 设置UI状态
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
