using System;
using System.Collections.Generic;
/// <summary>
/// 事件系统类，用于统一管理所有事件
/// 
/// 设计思路：
/// 1. 采用观察者模式，实现模块间的解耦
/// 2. 支持带对象和不带对象两种注册方式
/// 3. 使用字典存储事件映射，提高查找效率
/// 4. 支持一个事件多个监听者
/// 5. 防止重复注册，避免内存泄漏
/// 
/// 使用方法：
/// 1. 注册事件：EventSysterm.Regist(EventIds.TestEvent, OnTestEvent);
/// 2. 派发事件：EventSysterm.Dispatch(EventIds.TestEvent, "Hello Event!");
/// 3. 反注册事件：EventSysterm.UnRegist(EventIds.TestEvent, OnTestEvent);
/// 
/// 注意事项：
/// 1. 注册和反注册要成对出现，避免内存泄漏
/// 2. 对象销毁时记得反注册相关事件
/// 3. 事件ID统一在EventIds中管理，避免字符串硬编码
/// 4. 回调方法参数统一为object，使用时需要类型转换
/// 5. 派发事件时注意参数类型匹配
/// </summary>
public static class EventSysterm
{
    /// <summary>
    /// 事件监听者结构，包含目标对象和回调方法
    /// </summary>
    private class EventListener
    {
        public object Target; // 可为 null，表示全局监听
        public Action<object> Callback;
    }

    /// <summary>
    /// 事件映射表，key为事件ID，value为监听者列表
    /// </summary>
    private static Dictionary<string, List<EventListener>> _eventMap = new Dictionary<string, List<EventListener>>();

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="key">事件ID</param>
    /// <param name="callback">回调方法</param>
    /// <param name="target">目标对象，可为null</param>
    public static void Regist(string key, Action<object> callback, object target = null)
    {
        if (!_eventMap.ContainsKey(key))
        {
            _eventMap[key] = new List<EventListener>();
        }
        // 防止重复注册
        if (!_eventMap[key].Exists(e => e.Target == target && e.Callback == callback))
        {
            _eventMap[key].Add(new EventListener { Target = target, Callback = callback });
        }
    }

    /// <summary>
    /// 反注册事件
    /// </summary>
    /// <param name="key">事件ID</param>
    /// <param name="callback">回调方法</param>
    /// <param name="target">目标对象，可为null</param>
    public static void UnRegist(string key, Action<object> callback, object target = null)
    {
        if (_eventMap.ContainsKey(key))
        {
            _eventMap[key].RemoveAll(e => e.Target == target && e.Callback == callback);
            if (_eventMap[key].Count == 0)
            {
                _eventMap.Remove(key);
            }
        }
    }

    /// <summary>
    /// 派发事件
    /// </summary>
    /// <param name="key">事件ID</param>
    /// <param name="param">事件参数</param>
    public static void Dispatch(string key, object param)
    {
        if (_eventMap.ContainsKey(key))
        {
            // 创建副本避免在遍历过程中修改集合
            var listeners = new List<EventListener>(_eventMap[key]);
            foreach (var listener in listeners)
            {
                listener.Callback(param);
            }
        }
    }
}
