using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWorldScript : MonoBehaviour
{
    Stopwatch stopwatchTimer = new Stopwatch();
    public string ElapsedTimeString { get; set; }

    //String that are used to select the player movement settings
    //Object is set to not destroy on scene reload - can be read by player scripts in other scenes
    public string TeleportType { get; set; }
    public bool HMDDependency { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        StopwatchText();
    }

    void StopwatchText()
    {
        TimeSpan elapsedTime = stopwatchTimer.Elapsed;
        ElapsedTimeString = elapsedTime.ToString(@"m\:ss\.fff");
    }

    public void StartStopwatch()
    {
        stopwatchTimer = Stopwatch.StartNew();
    }
    public void StopStopwatch()
    {
        stopwatchTimer.Stop();
    }

    public void FreezeStopwatch()
    {
        stopwatchTimer.Stop();
        StartCoroutine(ResumeStopwatch(2));
    }
    IEnumerator ResumeStopwatch(float delay)
    {
        yield return new WaitForSeconds(delay);
        stopwatchTimer.Start();
    }

    /// <summary>
    /// Global SceneManager switch - uses a string to determine what scenes to change to
    /// </summary>
    /// <param name="scene"></param>
    public void ChangeScene(string scene)
    {
        switch (scene)
        {
            case "Menu":
                SceneManager.LoadScene("MenuScene");
                break;

            case "Game":
                SceneManager.LoadScene("GameScene");
                break;

            case "Developer":
                SceneManager.LoadScene("DeveloperScene");
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
