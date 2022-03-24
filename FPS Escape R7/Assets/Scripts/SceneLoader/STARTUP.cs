using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoader
{
    public class Startup : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(StartupVideo());
        }

        public void Update()
        {
            if (Input.anyKey)
            {
                SceneManager.LoadSceneAsync("NewMainMenu");
            }
        }

        private static IEnumerator StartupVideo()
        {
            yield return new WaitForSeconds(6.5f);
            SceneManager.LoadSceneAsync("NewMainMenu");
        }
    }
}
