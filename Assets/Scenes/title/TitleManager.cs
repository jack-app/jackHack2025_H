using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject black;
    [SerializeField] private RectTransform instructionImageRect;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Commons.FadeIn(black));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Tutorial()
    {
        StartCoroutine(Commons.Button(instructionImageRect));
        StartCoroutine(TutorialCoroutine());
    }
    private IEnumerator TutorialCoroutine()
    {
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("TutorialScene");
    }
}
