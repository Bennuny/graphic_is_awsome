using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;


public class MainMenu : MonoBehaviour
{
    Button btnNewGame;

    Button btnContinue;

    Button btnExit;

    PlayableDirector director;

    private void Awake()
    {
        btnNewGame = transform.GetChild(1).GetComponent<Button>();

        btnContinue = transform.GetChild(2).GetComponent<Button>();

        btnExit = transform.GetChild(3).GetComponent<Button>();


        btnExit.onClick.AddListener(QuitGame);

        btnNewGame.onClick.AddListener(PlayTimeline);

        btnContinue.onClick.AddListener(ContinueGame);

        director = FindObjectOfType<PlayableDirector>();

        director.stopped += NewGame;
    }

    void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit Game");
    }


    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }


    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        // Transition Scene

        SceneController.Instance.TransitionToFirstLevel();
    }


    void PlayTimeline()
    {
        director.Play();
    }
}
