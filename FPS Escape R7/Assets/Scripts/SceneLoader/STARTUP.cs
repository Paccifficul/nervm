using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class STARTUP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartupVideo());
    }

    IEnumerator StartupVideo()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
