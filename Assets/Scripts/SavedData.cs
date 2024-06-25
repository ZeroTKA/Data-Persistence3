using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedData : MonoBehaviour
{
    public static SavedData Instance;
    public string DisplayName;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);
    }

    public void SetDisplayName(string s)
    {
        DisplayName = s;
        Debug.Log(s +" is s");
        Debug.Log(DisplayName);
    }


}
