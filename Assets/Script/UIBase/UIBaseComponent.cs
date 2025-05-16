using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 设置UI状态
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
