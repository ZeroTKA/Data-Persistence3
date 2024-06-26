using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    public void SetDisplayName(string s)
    {
        if (s != null && !s.Trim().Equals(""))
        {
            SavedData.Instance.HasEnteredAName = true;
            SavedData.Instance.DisplayName = s;
            Debug.Log("HasName is True");
        }
        else
        {
            SavedData.Instance.HasEnteredAName = false;
            Debug.Log("HasName is False");
            //show UI that says invalid characters?
        }
    }
    public void StartGame()
    {
        if(SavedData.Instance.HasEnteredAName)
        { 
            SceneManager.LoadScene(1); 
        }
        else 
        {
            //do something visual? 
        }
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
