using System;
using UnityEngine;

public class EventSystermTest : MonoBehaviour
{
    private void Start()
    {
        // 注册事件（不带对象）
        EventSysterm.Regist(EventIds.TestEvent, OnTestEvent);
        Debug.Log("注册TestEvent（不带对象）");

        // 派发事件
        EventSysterm.Dispatch(EventIds.TestEvent, "Hello Event!");

        // 反注册事件（不带对象）
        EventSysterm.UnRegist(EventIds.TestEvent, OnTestEvent);
        Debug.Log("反注册TestEvent（不带对象）");

        // 再次派发，应该没有回调输出
        EventSysterm.Dispatch(EventIds.TestEvent, "This should not be received.");

        // 注册事件（带对象）
        EventSysterm.Regist(EventIds.TestEvent, OnTestEvent, this);
        Debug.Log("注册TestEvent（带对象）");

        // 派发事件
        EventSysterm.Dispatch(EventIds.TestEvent, "Hello Event with target!");

        // 反注册事件（带对象）
        EventSysterm.UnRegist(EventIds.TestEvent, OnTestEvent, this);
        Debug.Log("反注册TestEvent（带对象）");

        // 再次派发，应该没有回调输出
        EventSysterm.Dispatch(EventIds.TestEvent, "This should not be received (target).");
    }

    private void OnTestEvent(object param)
    {
        Debug.Log($"收到TestEvent，参数：{param}");
    }
} 