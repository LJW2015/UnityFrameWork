using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        UIManager.Init();
    }
    void Start()
    {
        UIManager.OpenUI(UIIds.UIMain.Id);
    }
}
