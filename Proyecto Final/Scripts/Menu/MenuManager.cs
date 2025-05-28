using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("SelectedProfileIndex");
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
        PlayerPrefs.DeleteKey("SelectedProfileIndex");
        PlayerPrefs.Save();
        Application.Quit();
    }
}
