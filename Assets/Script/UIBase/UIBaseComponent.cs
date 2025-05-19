using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

public static class TaskExtension
{
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>();
        using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
        {
            if (task != await Task.WhenAny(task, tcs.Task))
                throw new OperationCanceledException(cancellationToken);
        }
        return await task;
    }
}

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
    private CancellationTokenSource _cancellationTokenSource;

    public void Init(UIBaseView uiBaseView){
        _uiBaseView = uiBaseView;
        _cancellationTokenSource = new CancellationTokenSource();
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

        try {
            GameObject instance = await AssetManager.InstantiatePrefabAsync(path, parent, name)
                .WithCancellation(_cancellationTokenSource.Token);
                
            if (instance == null || !this || !parent) {
                return null;
            }

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
        catch (OperationCanceledException) {
            Debug.Log("界面关闭，取消加载");
            return null;
        }
        catch (Exception e) {
            Debug.LogError($"加载预制体失败: {e.Message}");
            return null;
        }
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

    public void OnDestroy() {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
    }
}
    