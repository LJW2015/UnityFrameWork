using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public enum CollectionType {
    Map = 0,
    Array = 1,
}

public class UICompoentCollection : MonoBehaviour
{
    public List<MonoBehaviour> components;
    public CollectionType collectionType;

    private Dictionary<string, MonoBehaviour> _componentMap = null;
    private List<MonoBehaviour> _componentArray = null;
    private bool _isInitialized = false;

    /// <summary>
    /// 初始化组件集合
    /// </summary>
    public void Initialize() {
        if (_isInitialized) return;
        
        _componentMap = new Dictionary<string, MonoBehaviour>();
        _componentArray = new List<MonoBehaviour>();

        foreach (var component in components) {
            if (component is UICompoentCollection collection) {
                collection.Initialize();
            }
            AddToCollection(component);
        }
        _isInitialized = true;
    }

    /// <summary>
    /// 添加组件到集合
    /// </summary>
    /// <param name="component">组件</param>
    private void AddToCollection(MonoBehaviour component) {
        if (collectionType == CollectionType.Map) {
            _componentMap[component.name] = component;
        } else {
            _componentArray.Add(component);
        }
    }

    //公用部分 start
    /// <summary>
    /// 获取组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <param name="index">组件名称或索引</param>
    /// <returns>组件</returns>
    public T Get<T>(string index) where T : MonoBehaviour {
        if (collectionType == CollectionType.Map && _componentMap != null) {
            if (_componentMap.TryGetValue(index, out MonoBehaviour comp)) {
                return comp as T;
            }
            return null;
        }
        return null;
    }

    public T Get<T>(int index) where T : MonoBehaviour {
        if (collectionType == CollectionType.Array && _componentArray != null) {
            if (index < _componentArray.Count) {
                return _componentArray[index] as T;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取Map迭代器
    /// </summary>
    /// <returns>Map迭代器</returns>
    public IEnumerable<KeyValuePair<string, MonoBehaviour>> GetMapIterator() {
        return _componentMap;
    }

    /// <summary>
    /// 获取数组迭代器
    /// </summary>
    /// <returns>数组迭代器</returns>
    public IEnumerable<MonoBehaviour> GetArrayIterator() {
        return _componentArray;
    }

    /// <summary>
    /// 获取迭代器
    /// </summary>
    /// <returns>迭代器</returns>
    public IEnumerable<MonoBehaviour> GetIterator() {
        if (collectionType == CollectionType.Map) {
            return _componentMap?.Values ?? Enumerable.Empty<MonoBehaviour>();
        } else {
            return _componentArray ?? Enumerable.Empty<MonoBehaviour>();
        }
    }
    /// <summary>
    /// 获取组件数量
    /// </summary>
    public int Count {
        get {
            if (collectionType == CollectionType.Map) {
                return _componentMap.Count;
            } else {
                return _componentArray.Count;
            }
        }
    }

    /// <summary>
    /// 检查组件是否存在
    /// </summary>
    /// <param name="component">组件</param>
    /// <param name="index">组件名称或索引</param>
    /// <returns>是否存在</returns>
    public bool Check(MonoBehaviour component, string index) {
        return Get<MonoBehaviour>(index) == component;
    }

    public bool Check(MonoBehaviour component, int index) {
        return Get<MonoBehaviour>(index) == component;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="component">组件</param>
    public void Add(MonoBehaviour component) {
        AddToCollection(component);
        components.Add(component);
    }
    //公用部分 end

}
