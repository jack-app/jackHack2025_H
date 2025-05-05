using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject black;
    [SerializeField] private RectTransform startImageRect;
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
        Manager.GenerateQuiz();  // クイズ読み込み
        StartCoroutine(Commons.Button(startImageRect));
        StartCoroutine(GameStartCoroutine());
    }
    private IEnumerator GameStartCoroutine()
    {
        yield return StartCoroutine(Commons.FadeOut(black));
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
