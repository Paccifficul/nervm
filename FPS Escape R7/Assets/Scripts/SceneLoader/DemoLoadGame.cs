using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoader
{
    public class DemoLoadGame : MonoBehaviour
    {
        [SerializeField]
        private string sceneName;

        private IEnumerator LoadSceneCor()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void CreateScene()
        {
            StartCoroutine(LoadSceneCor());
        }
    }
}
