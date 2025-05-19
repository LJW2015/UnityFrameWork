using System;
using UnityEngine;
using GameFramework;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class TestLogic : UIBaseView
{
    public override void OnInit()
    {
        base.OnInit();
        if (UIComponent == null) {
            Debug.LogError("UIComponent is null in TestLogic");
            return;
        }
        
        Text text = UIComponent.Get<Text>("TestText");
        if (text == null) {
            Debug.LogError("TestText component not found");
            return;
        }
        
        text.text = "test";

        UIComponent.UpdateInstanceCollectionArray(UIComponent.Get<UICompoentCollection>("TestUccClone"),UIComponent.Get<UICompoentCollection>("TestUccUpdate"),5, (collection,index) => {
            collection.name = "TestUccUpdateClone" + index;
        });

        StartCoroutine(LoadTemplateAsync());
        Debug.Log("TestLogic OnInit 被调用");
    }

    private IEnumerator LoadTemplateAsync()
    {
        var task = UIComponent.InstantiateCollectionAsync("Prefab/Template/TestTemplate", this.transform);
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.Result != null)
        {
            task.Result.gameObject.SetActive(false);
        }
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
    
    public override void OnButtonClicked(Button button)
    {
        Debug.Log(button.name + "被调用");
    }
} 