using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    Button btnNewGame;

    Button btnContinue;

    Button btnExit;



    private void Awake()
    {
        btnNewGame = transform.GetChild(1).GetComponent<Button>();

        btnContinue = transform.GetChild(2).GetComponent<Button>();

        btnExit = transform.GetChild(3).GetComponent<Button>();


        btnExit.onClick.AddListener(QuitGame);

        btnNewGame.onClick.AddListener(NewGame);

        btnContinue.onClick.AddListener(ContinueGame);
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


    void NewGame()
    {
        PlayerPrefs.DeleteAll();
        // Transition Scene

        SceneController.Instance.TransitionToFirstLevel();
    }
}
