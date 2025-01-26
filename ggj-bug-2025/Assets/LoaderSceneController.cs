using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoaderSceneController : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("Main");
        if (_asyncOperation != null) _asyncOperation.allowSceneActivation = false;
    }

 
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }

            if (Event.current != null && Event.current.isMouse)
            {
               return;
            }
            
            if (_asyncOperation != null && _asyncOperation.progress >= 0.9f)
            {
                _asyncOperation.allowSceneActivation = true;
            }
            
        }
    }
}