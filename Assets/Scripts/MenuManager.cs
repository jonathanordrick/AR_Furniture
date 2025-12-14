using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartAR()
    {
        SceneManager.LoadScene("SampleScene"); // atau nama scene AR kamu
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}