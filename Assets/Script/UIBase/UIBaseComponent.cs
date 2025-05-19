using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class ResourceRequestExtension
{
    public static TaskAwaiter GetAwaiter(this ResourceRequest request)
    {
        var tcs = new TaskCompletionSource<object>();
        request.completed += operation => tcs.SetResult(null);
        return ((Task)tcs.Task).GetAwaiter();
    }
}

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
    
    // 属性访问器
    public string UIName => _uiName;
    public UILayer UILayer => _uiLayer;

    private UICompoentCollection _uiCompoentCollection;
    private UIBaseView _uiBaseView;

    public void Init(UIBaseView uiBaseView){
        _uiBaseView = uiBaseView;
        _uiCompoentCollection = GetComponent<UICompoentCollection>();
        if (_uiCompoentCollection != null) {
            _uiCompoentCollection.Initialize();
            RegistComponentEvents(_uiCompoentCollection);
        }
    }

    public void RegistComponentEvents(MonoBehaviour component){
        if(component is UICompoentCollection collection){
            foreach(var item in collection.GetIterator()){
                RegistComponentEvents(item);
            }
        }
        if(component is Button button){
            button.onClick.AddListener(() => {
                OnButtonClicked(button);
            });
        }
        if(component is Slider slider){
            slider.onValueChanged.AddListener((value) => {
                OnSliderValueChanged(slider,value);
            });
        }
        if(component is InputField inputField){
            inputField.onValueChanged.AddListener((value) => {
                OnInputFieldValueChanged(inputField,value);
            });
        }
        if(component is Toggle toggle){
            toggle.onValueChanged.AddListener((value) => {
                OnToggleValueChanged(toggle,value);
            });
        }
    }

    private void OnButtonClicked(Button button) {
        _uiBaseView.OnButtonClicked(button);
    }

    private void OnSliderValueChanged(Slider slider, float value) {
        _uiBaseView.OnSliderValueChanged(slider,value);
    }

    private void OnInputFieldValueChanged(InputField inputField, string value) {
        _uiBaseView.OnInputFieldValueChanged(inputField,value);
    }

    private void OnToggleValueChanged(Toggle toggle, bool value) {
        _uiBaseView.OnToggleValueChanged(toggle,value);
    }
    
    public T Get<T>(string path) where T : MonoBehaviour
    {
        return _uiCompoentCollection.Get<T>(path);
    }

    public bool Check(MonoBehaviour component, string path){
        return _uiCompoentCollection.Check(component,path);
    }

    /// <summary>
    /// 实例化UICompoentCollection
    /// </summary>
    /// <param name="target">目标UICompoentCollection</param>
    /// <param name="parent">父级Transform</param>
    /// <param name="name">实例化名称</param>
    /// <returns>实例化后的UICompoentCollection</returns>
    public UICompoentCollection InstantiateCollection(UICompoentCollection target, Transform parent = null, string name = null){
        if(parent == null){
            parent = gameObject.transform;
        }
        if(name == null){
            name = target.name;
        }
        GameObject instance = Instantiate(target.gameObject,parent);
        instance.name = name;
        instance.transform.SetParent(parent);
        instance.transform.localPosition = Vector3.zero;
        UICompoentCollection collection = instance.GetComponent<UICompoentCollection>();
        if(collection != null){
            collection.Initialize();
        }
        if(parent.GetComponent<UICompoentCollection>() != null){
            parent.GetComponent<UICompoentCollection>().Add(instance.GetComponent<MonoBehaviour>());
        }
        RegistComponentEvents(collection);
        return collection;
    }

    /// <summary>
    /// 从Resources中异步实例化UICompoentCollection
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="parent">父级Transform</param>
    /// <param name="name">实例化名称</param>
    /// <returns>实例化后的UICompoentCollection</returns>
    public async Task<UICompoentCollection> InstantiateCollectionAsync(string path, Transform parent = null, string name = null){
        if(parent == null){
            parent = gameObject.transform;
        }
        if(name == null){
            name = path;
        }
        
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        await request;
        
        if(request.asset == null){
            Debug.LogError($"Failed to load prefab from path: {path}");
            return null;
        }
        
        GameObject instance = Instantiate(request.asset as GameObject, parent);
        instance.name = name;
        instance.transform.SetParent(parent);
        instance.transform.localPosition = Vector3.zero;
        
        UICompoentCollection collection = instance.GetComponent<UICompoentCollection>();
        if(collection != null){
            collection.Initialize();
        }
        if(parent.GetComponent<UICompoentCollection>() != null){
            parent.GetComponent<UICompoentCollection>().Add(instance.GetComponent<MonoBehaviour>());
        }
        RegistComponentEvents(collection);
        return collection;
    }

    /// <summary>
    /// 更新实例化UICompoentCollection数组
    /// </summary>
    /// <param name="target">目标UICompoentCollection</param>
    /// <param name="parent">父级UICompoentCollection</param>
    /// <param name="count">数量</param>
    /// <param name="action">回调</param>
    public void UpdateInstanceCollectionArray(UICompoentCollection target, UICompoentCollection parent, int count, Action<UICompoentCollection,int> action = null){
        int maxCount = Math.Max(parent.Count,count);
        for(int i = 0; i < maxCount; i++){
            UICompoentCollection collection = parent.Get<UICompoentCollection>(i);
            if(collection == null){
                collection = InstantiateCollection(target,parent.transform);
            }
            if(i < count){
                collection.gameObject.SetActive(true);
                action?.Invoke(collection,i);
            }else{
                collection.gameObject.SetActive(false);
            }
        }
    }
}
    