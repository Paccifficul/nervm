using Assets.Scripts.IngameScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayRoundScript : MonoBehaviour
{
    private int tScore;
    private int ctScore;
    private DateTime previousFrameTime;
    public const int TotalWinScore = 5;//Понятия не имею, до скольки раундов будет идти игра.
    public readonly TimeSpan roundTime = new TimeSpan(0, 15, 0);
    public int Round;
    public TimeSpan timeLeft = new TimeSpan();
    public bool isBombPlanted;
    public List<Vector3> tSpawnPoints = new List<Vector3>();
    public List<Vector3> ctSpawnPoints = new List<Vector3>();
    /// <summary>
    /// Количество очков команды террористов.
    /// </summary>
    public int TScore
    {
        get
        {
            return tScore;
        }
        private set
        {
            tScore = value;
            if (value >= TotalWinScore)
            {
                if (value > ctScore + 1)
                {
                    OnFirstTeamWin();
                }
            }
        }
    }
    /// <summary>
    /// Количество очков у КТ.
    /// </summary>
    public int CTScore
    {
        get
        {
            return ctScore;
        }
        private set
        {
            ctScore = value;
            if (value >= TotalWinScore)
            {
                if (value > tScore + 1)
                {
                    OnSecondTeamWin();
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        previousFrameTime = DateTime.Now;
        timeLeft = roundTime;
        //Это пока дебаг:
        IngameResources.Players.Add(GameObject.FindGameObjectsWithTag("Player")[0]);
        StartCoroutine(SetupNewRound());
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan delta = DateTime.Now - previousFrameTime;
        previousFrameTime = DateTime.Now;
        timeLeft -= delta;
        if (timeLeft <= TimeSpan.Zero)
        {
            Round++;
            timeLeft = roundTime;
            if (isBombPlanted)
            {
                TScore++;
                StartCoroutine(PlayTWinAnimation());
            }
            else
            {
                CTScore++;
                StartCoroutine(PlayCTWinAnimation());
            }
            StartCoroutine(SetupNewRound());
        }
    }
    
    public IEnumerator SetupNewRound()
    {
        yield return new WaitForEndOfFrame();
        var t = (from c in IngameResources.Players where c.GetComponent<LifeScript>().isTTeam select c).OrderBy(x => UnityEngine.Random.value);
        var ct = IngameResources.Players.Except(t).OrderBy(x => UnityEngine.Random.value);
        int i = 0;
        foreach (var player in t)
        {
            player.transform.position = tSpawnPoints[i++];
        }
        i = 0;
        foreach (var player in ct)
        {
            player.transform.position = ctSpawnPoints[i++];
        }
    }

    public IEnumerator PlayTWinAnimation()
    {
        yield return new WaitForEndOfFrame();
    }

    public IEnumerator PlayCTWinAnimation()
    {
        yield return new WaitForEndOfFrame();
    }

    public void OnFirstTeamWin()
    {
        print("ft");
    }

    public void OnSecondTeamWin()
    {
        print("st");
    }
}
