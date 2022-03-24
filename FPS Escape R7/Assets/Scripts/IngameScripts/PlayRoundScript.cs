using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InGameScripts;
using UnityEngine;

namespace InGameScripts
{
    public class PlayRoundScript : MonoBehaviour
    {
        private int _tScore;
        private int _ctScore;
        private DateTime _previousFrameTime;
        private const int TotalWinScore = 5;//Понятия не имею, до скольки раундов будет идти игра.
        private readonly TimeSpan _roundTime = new TimeSpan(0, 15, 0);
        public int Round;
        private TimeSpan _timeLeft = new TimeSpan();
        public bool isBombPlanted;
        public List<Vector3> tSpawnPoints = new List<Vector3>();
        public List<Vector3> ctSpawnPoints = new List<Vector3>();
        /// <summary>
        /// Количество очков команды террористов.
        /// </summary>
        public int TScore
        {
            get => _tScore;
            private set
            {
                _tScore = value;
                if (value < TotalWinScore) return;
                if (value > _ctScore + 1)
                {
                    OnFirstTeamWin();
                }
            }
        }
        /// <summary>
        /// Количество очков у КТ.
        /// </summary>
        public int CTScore
        {
            get => _ctScore;
            private set
            {
                _ctScore = value;
                if (value < TotalWinScore) return;
                if (value > _tScore + 1)
                {
                    OnSecondTeamWin();
                }
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            _previousFrameTime = DateTime.Now;
            _timeLeft = _roundTime;
            //Это пока дебаг:
            InGameResources.Players.Add(GameObject.FindGameObjectsWithTag("Player")[0]);
            StartCoroutine(SetupNewRound());
        }

        // Update is called once per frame
        private void Update()
        {
            var delta = DateTime.Now - _previousFrameTime;
            _previousFrameTime = DateTime.Now;
            _timeLeft -= delta;
            if (_timeLeft > TimeSpan.Zero) return;
            Round++;
            _timeLeft = _roundTime;
            if (isBombPlanted)
            {
                TScore++;
                StartCoroutine(PlayTWinAnimation());
            }
            else
            {
                CTScore++;
                StartCoroutine(PlayCtWinAnimation());
            }
            StartCoroutine(SetupNewRound());
        }

        private IEnumerator SetupNewRound()
        {
            yield return new WaitForEndOfFrame();
            var t = (from c in InGameResources.Players where c.GetComponent<LifeScript>().isTTeam select c).OrderBy(x => UnityEngine.Random.value);
            var ct = InGameResources.Players.Except(t).OrderBy(x => UnityEngine.Random.value);
            var i = 0;
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

        private static IEnumerator PlayTWinAnimation()
        {
            yield return new WaitForEndOfFrame();
        }

        private static IEnumerator PlayCtWinAnimation()
        {
            yield return new WaitForEndOfFrame();
        }

        private static void OnFirstTeamWin()
        {
            print("ft");
        }

        private static void OnSecondTeamWin()
        {
            print("st");
        }
    }
}
