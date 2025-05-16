using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 首先创建一个UI生命周期接口
public interface IUIView
{
    void OnInit();      // 初始化
    void OnShow();      // 显示
    void OnHide();      // 隐藏
    void OnClose();   // 销毁
}


public class UIBaseView : MonoBehaviour, IUIView
{
    // UI组件引用
    protected UIBaseComponent _uiComponent;
    
    // UI状态
    protected bool _isInitialized = false;  // 是否初始化
    protected bool _isVisible = false;      // 是否可见

    public UIBaseComponent UIComponent => _uiComponent;
    
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
       
    }
    
    // 获取UI组件方法
    protected T GetUIComponent<T>(string path = "") where T : Component
    {
        return _uiComponent?.GetComponent<T>(path);
    }

    public virtual void OnButtonClicked(Button button)
    {
        // 子类实现具体的事件处理
    }

    public virtual void OnSliderValueChanged(Slider slider, float value)
    {
        // 子类实现具体的事件处理
    }

    public virtual void OnInputFieldValueChanged(InputField inputField, string value)
    {
        // 子类实现具体的事件处理
    }

    public virtual void OnToggleValueChanged(Toggle toggle, bool value)
    {
        // 子类实现具体的事件处理
    }
}
