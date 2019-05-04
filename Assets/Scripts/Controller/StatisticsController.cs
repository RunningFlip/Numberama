﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StatisticsController : MonoBehaviour
{
    [Header("Button")]
    public Button loadMainMenuButton;


    private void Start()
    {
        //Button
        loadMainMenuButton.onClick.AddListener(delegate { SceneManager.LoadScene("MainMenuScene"); });
    }
}
