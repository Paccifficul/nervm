using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DEMO_loadgamer : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadSceneCor()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void CreateScene()
    {
        StartCoroutine(LoadSceneCor());
    }
}
