using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Text text_Letters_Sum;
    [SerializeField] private Text text_Noise_Sum;
    [SerializeField] private Text text_Score_Sum;
    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private GameObject black;
    int charSum = 0;
    int heardCharsSum = 0;
    int canceledNoiseSum = 0;
    int generatedNoiseSum = 0;
    int correctAnswers = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Commons.FadeIn(black));
        foreach(Manager.Savedata data in Manager.savedata)
        {
            charSum += data.gotStatement.Length;
            heardCharsSum += data.gotVoice;
            canceledNoiseSum += data.canceledNoise;
            generatedNoiseSum += data.generatedNoise;
            if (data.isCorrect)
            {
                correctAnswers++;
            }
        }
        ShowHeardChars();
        ShowCanceledNoise();
        ShowCorrectAnswers();
    }

    void ShowHeardChars()
    {
        text_Letters_Sum.text = heardCharsSum.ToString() + " / " + charSum.ToString() + " 文字";
    }

    void ShowCanceledNoise()
    {
        text_Noise_Sum.text = canceledNoiseSum.ToString() + " / " + generatedNoiseSum.ToString() + " 個";
    }

    void ShowCorrectAnswers()//正答数がsavedataから見れるようになってから
    {
        text_Score_Sum.text = correctAnswers.ToString() + " / " + Manager.quizNumber.ToString() + " 問";
    }

    // Click to Go title Scene
    public void GoTitle()
    {
        StartCoroutine(Commons.Button(buttonRect));
        StartCoroutine(GoTitleCoroutine());
    }
    private IEnumerator GoTitleCoroutine()
    {
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("TitleScene");
    }
}
