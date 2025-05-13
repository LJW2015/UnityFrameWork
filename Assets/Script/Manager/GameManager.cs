using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

/// <summary>
/// 游戏管理器，负责管理游戏生命周期
/// </summary>
public class GameManager : MonoBehaviour
{
    void Awake()
    {
        UIManager.Init();
    }
    void Start()
    {
        UIManager.OpenUI("UIMain");
    }
}
