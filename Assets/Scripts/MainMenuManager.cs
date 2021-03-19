using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private string startSceneName;

    void Start()
    {
        startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(startSceneName, LoadSceneMode.Single);
        asyncLoad.completed += OnStartGame;
    }

    private void OnStartGame(AsyncOperation loadOperation)
    {
        Debug.Log("Loaded start scene.");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
