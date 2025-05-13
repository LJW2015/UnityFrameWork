using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    [SerializeField]  // 这样可以在Inspector中设置按钮引用
    private Button testButton;
    
    void Awake()
    {
        Debug.Log("Test脚本已加载");
        if (testButton == null)
        {
            Debug.LogError("testButton引用为空！请检查Inspector面板中的引用设置");
            return;
        }
        
        // 在Awake中绑定事件，确保只绑定一次
        // testButton.onClick.RemoveAllListeners(); // 先移除所有已存在的事件
        testButton.onClick.AddListener(OnTestButtonClick);
        Debug.Log("按钮点击事件已添加");
    }

    void OnDestroy()
    {
        // 在脚本销毁时移除事件监听，防止内存泄漏
        if (testButton != null)
        {
            testButton.onClick.RemoveListener(OnTestButtonClick);
        }
    }

    void Start()
    {
        Debug.Log("Test脚本Start方法被调用");
    }

    void Update()
    {
        
    }

    private void OnTestButtonClick()
    {
        Debug.Log("TestButton被点击");
        // 在这里添加你的按钮点击逻辑
    }
}
