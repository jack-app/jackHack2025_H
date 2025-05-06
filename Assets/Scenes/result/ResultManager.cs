using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private Text text_Letters_Sum;
    [SerializeField] private Text text_Noise_Sum;
    [SerializeField] private Text text_Score_Sum;
    [SerializeField] private Text text_Comment;
    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private GameObject black;
    int charSum = 0;
    int heardCharsSum = 0;
    int canceledNoiseSum = 0;
    int generatedNoiseSum = 0;
    int correctAnswers = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioClip systemSE;
    [SerializeField] private Image actionImage;
    [SerializeField] private Sprite star;
    [SerializeField] private Sprite ase;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
        Manager.GameEnd();
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
        //5問以外の場合に対応していないことに注意
        switch (correctAnswers)
        {
            case 5:
                text_Comment.text = "かんぺき太子！";
                actionImage.sprite = star;
                break;
            case 4:
                text_Comment.text = "かなり太子！";
                actionImage.sprite = star;
                break;
            case 3:
                text_Comment.text = "そこそこ太子";
                actionImage.color = new(1, 1, 1, 0);
                break;
            case 2:
                text_Comment.text = "あかてん太子";
                actionImage.sprite = ase;
                break;
            case 1:
                text_Comment.text = "だめだめ太子";
                actionImage.sprite = ase;
                break;
            case 0:
                text_Comment.text = "ざんねん太子";
                actionImage.sprite = ase;
                break;
            default:
                break;
        }

    }

    // Click to Go title Scene
    public void GoTitle()
    {
        audioSource.clip = systemSE;
        audioSource.Play();
        StartCoroutine(Commons.Button(buttonRect));
        StartCoroutine(GoTitleCoroutine());
    }
    private IEnumerator GoTitleCoroutine()
    {
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("TitleScene");
    }
}
