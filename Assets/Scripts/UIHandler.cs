using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public void SetDisplayName(string s)
    {
        SavedData.Instance.DisplayName = s;
        Debug.Log(s + " is s");
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
