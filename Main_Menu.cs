using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void single()
    {
        SceneManager.LoadScene("Single Player");
    }
    public void coop()
    {
        SceneManager.LoadScene("Co-op");
    }
    public void exit()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }
}
