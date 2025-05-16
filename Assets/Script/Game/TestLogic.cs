using System;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TestLogic : UIBaseView
{
    // 测试用的UI组件引用
    public Button testButton;
    public Text testText;

    public Button button;
    
    public override void OnInit()
    {
        base.OnInit();
        
        // 获取UI组件
        testButton = GetUIComponent<Button>("TestButton");
        
        testButton.onClick.AddListener(OnTestButtonClick);

        testText = GetUIComponent<Text>("TestText");
        testText.text = "test";
        EventTrigger eventTrigger = testText.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => {
            Debug.Log("testText 被点击");
        });
        eventTrigger.triggers.Add(entry);
        Debug.Log("TestLogic OnInit 被调用");
    }
    
    public override void OnShow()
    {
        base.OnShow();
        Debug.Log("TestLogic OnShow 被调用");
    }
    
    public override void OnHide()
    {
        base.OnHide();
        Debug.Log("TestLogic OnHide 被调用");
    }
    
    public override void OnClose()
    {
        base.OnClose();
        Debug.Log("TestLogic OnClose 被调用");
    }
    
    // 按钮点击回调
    public void OnTestButtonClick()
    {
        Debug.Log("测试按钮被点击");
    }
} 