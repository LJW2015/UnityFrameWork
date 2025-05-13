using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 首先创建一个UI生命周期接口
public interface IUIView
{
    void OnInit();      // 初始化
    void OnShow();      // 显示
    void OnHide();      // 隐藏
    void OnClose();   // 销毁
}

// 创建一个UI事件接口
public interface IUIEvent
{
    void RegisterEvents();    // 注册事件
    void UnregisterEvents();  // 注销事件
} 

public class UIBaseView : MonoBehaviour, IUIView, IUIEvent
{
    // UI组件引用
    protected UIBaseComponent _uiComponent;
    
    // UI状态
    protected bool _isInitialized = false;  // 是否初始化
    protected bool _isVisible = false;      // 是否可见
    
    // 生命周期方法
    public virtual void OnInit()
    {
        if (_isInitialized) return;
        
        // 获取UI组件
        _uiComponent = GetComponent<UIBaseComponent>();
        if (_uiComponent == null)
        {
            Debug.LogError($"UIBaseComponent not found on {gameObject.name}");
            return;
        }
        
        _isInitialized = true;
        RegisterEvents();
    }

    public virtual void OnShow()
    {
        if (!_isInitialized)
        {
            OnInit();
        }
        _isVisible = true;
        _uiComponent?.SetVisible(true);
    }

    public virtual void OnHide()
    {
        _isVisible = false;
        _uiComponent?.SetVisible(false);
    }

    public virtual void OnClose()
    {
        UnregisterEvents();
    }

    // 事件系统
    public virtual void RegisterEvents()
    {
        // 子类实现具体的事件注册
    }

    public virtual void UnregisterEvents()
    {
        // 子类实现具体的事件注销
    }
    
    // 获取UI组件方法
    protected T GetUIComponent<T>(string path = "") where T : Component
    {
        return _uiComponent?.GetComponent<T>(path);
    }
}
