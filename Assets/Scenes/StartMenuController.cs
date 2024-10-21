using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnStartClick()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnExitClick(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}