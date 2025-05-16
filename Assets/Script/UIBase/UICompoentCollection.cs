using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CollectionType {
    Map = 0,
    Array = 1,
}

public class UICompoentCollection : MonoBehaviour
{
    public List<Component> components;
    public CollectionType collectionType;

    private Dictionary<string, Component> _componentMap = null;
    private List<Component> _componentArray = null;
    private bool _isInitialized = false;

    private void Start() {
        if (!_isInitialized) {
            Initialize();
        }
    }

    /// <summary>
    /// 初始化组件集合
    /// </summary>
    public void Initialize() {
        if (_isInitialized) return;
        
        _componentMap = new Dictionary<string, Component>();
        _componentArray = new List<Component>();

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
    private void AddToCollection(Component component) {
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
    public T Get<T>(string index) where T : Component {
        if (collectionType == CollectionType.Map && _componentMap != null) {
            if (_componentMap.TryGetValue(index, out Component comp)) {
                return comp as T;
            }
            return null;
        }
        return null;
    }

    public T Get<T>(int index) where T : Component {
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
    public IEnumerable<KeyValuePair<string, Component>> GetMapIterator() {
        return _componentMap;
    }

    /// <summary>
    /// 获取数组迭代器
    /// </summary>
    /// <returns>数组迭代器</returns>
    public IEnumerable<Component> GetArrayIterator() {
        return _componentArray;
    }

    /// <summary>
    /// 获取迭代器
    /// </summary>
    /// <returns>迭代器</returns>
    public IEnumerable<Component> GetIterator() {
        if (collectionType == CollectionType.Map) {
            return _componentMap.Values;
        } else {
            return _componentArray;
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
    public bool Check(Component component, string index) {
        return Get<Component>(index) == component;
    }

    public bool Check(Component component, int index) {
        return Get<Component>(index) == component;
    }

    /// <summary>
    /// 添加组件
    /// </summary>
    /// <param name="component">组件</param>
    public void Add(Component component) {
        AddToCollection(component);
        components.Add(component);
    }
    //公用部分 end

}
