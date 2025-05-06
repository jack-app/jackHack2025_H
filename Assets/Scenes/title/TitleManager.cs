using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject black;
    [SerializeField] private RectTransform startImageRect;
    [SerializeField] private RectTransform instructionImageRect;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Commons.FadeIn(black));
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        audioSource.Play();
        StartCoroutine(GameStartCoroutine());
    }
    private IEnumerator GameStartCoroutine()
    {
        yield return StartCoroutine(Commons.Button(startImageRect));
        Manager.GenerateQuiz();  // クイズ読み込み
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("GameScene");
    }
    public void Tutorial()
    {
        audioSource.Play();
        StartCoroutine(Commons.Button(instructionImageRect));
        StartCoroutine(TutorialCoroutine());
    }
    private IEnumerator TutorialCoroutine()
    {
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("TutorialScene");
    }
}
