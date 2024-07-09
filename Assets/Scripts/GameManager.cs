using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    public Action<float> hideAllUI=null;
    private void Awake() {
        KeywordCofig.LoadKeywordCfgData();
        ParagraphConfig.LoadParaCfgData();
    }
    private void Start()
    {
        StartCoroutine(StartTelling());
    }
    IEnumerator StartTelling()
    {
        yield return new WaitForSeconds(0.5f);
        ParagraphSpawner.Instance.SpawnParagraph(0);
        //StartCoroutine(GameFinishCoroutine());
    }

    public void GameFinish(bool restart=false)
    {
        StartCoroutine(GameFinishCoroutine(restart));
    }
    IEnumerator GameFinishCoroutine(bool restart)
    {
        //yield return new WaitForSeconds(5f);

        // ParagraphSpawner.Instance.HideAllParagraphs(1);
        // KeywordNodeManager.Instance.
        yield return new WaitForSeconds(4f);
        hideAllUI?.Invoke(3);
        yield return new WaitForSeconds(3f);
        if (restart) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else { Application.Quit(); }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
