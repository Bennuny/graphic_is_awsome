using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>, IEndGameObserver
{
    private GameObject player;

    private NavMeshAgent agent;

    public GameObject PlayerPrefab;

    public SceneFader sceneFaderPrefab;

    bool fadeFinish;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.AddObserver(this);

        fadeFinish = true;
    }

    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                {
                    StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                }
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                {
                    StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                }
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SaveManager.Instance.SavePlayerData();


        var activeSceneName = SceneManager.GetActiveScene().name;
        if (activeSceneName == sceneName)
        {
            player = GameManager.Instance.playerStats.gameObject;

            agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false;

            var destNode = GetDestination(destinationTag);
            player.transform.SetPositionAndRotation(destNode.transform.position, destNode.transform.rotation);

            agent.enabled = true;

            yield return null;
        }
        else
        {
            Debug.Log("Transition Scene Name " + sceneName);

            yield return SceneManager.LoadSceneAsync(sceneName);

            var destNode = GetDestination(destinationTag);
            yield return Instantiate(PlayerPrefab, destNode.transform.position, destNode.transform.rotation);

            SaveManager.Instance.LoadPlayerData();

            yield break;
        }

    }

    TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].transtionTag == destinationTag)
            {
                return entrances[i];
            }
        }

        return null;
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.Instance.SceneName));
    }


    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("MaoScene"));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);



        if (sceneName != "")
        {
            yield return fade.StartCoroutine(fade.FadeOut(2.0f));

            yield return SceneManager.LoadSceneAsync(sceneName);

            yield return player = Instantiate(PlayerPrefab, GameManager.Instance.GetEntrance().position, GameManager.Instance.GetEntrance().rotation);

            //
            SaveManager.Instance.SavePlayerData();

            yield return fade.StartCoroutine(fade.FadeIn(2.0f));

            yield break;
        }

        yield break;
    }


    public void TransitionToMain()
    {
        StartCoroutine(LoadMain());
    }


    IEnumerator LoadMain()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);

        yield return fade.StartCoroutine(fade.FadeOut(2.0f));

        yield return SceneManager.LoadSceneAsync("MainScene");

        yield return fade.StartCoroutine(fade.FadeIn(1.0f));

        yield break;
    }

    void IEndGameObserver.EndNotifiy()
    {
        if (fadeFinish)
        {
            fadeFinish = false;
        }

        TransitionToMain();
    }
}
