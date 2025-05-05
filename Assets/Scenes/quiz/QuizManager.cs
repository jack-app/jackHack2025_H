using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private Text choise1;
    [SerializeField] private Text choise2;
    [SerializeField] private Text choise3;
    [SerializeField] private RectTransform choise1Rect;
    [SerializeField] private RectTransform choise2Rect;
    [SerializeField] private RectTransform choise3Rect;
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject statementParent;
    [SerializeField] Text countText;
    [SerializeField] private GameObject timeup;
    [SerializeField] private GameObject black;

    private int seconds = 10;
    private bool choised = false;

    // 問題および選択肢の文章をUIに反映
    void Start()
    {
        string[] choises = Manager.quiz[Manager.currentQuiz].choices.ToArray(); //選択肢の取得
        timeup.SetActive(false);
        GenerateStatement();
        choise1.text = choises[0];
        choise2.text = choises[1];
        choise3.text = choises[2];
        StartCoroutine(Commons.FadeIn(black));
        StartCoroutine(Timer());
    }
    
    // Startの中で呼び出され、聞き取れた問題文を表示する。問題文は一文字ずつPrefabとして生成する
    void GenerateStatement()
    {
        string statement = Manager.newData.gotStatement;
        int circleSize = 70;

        Vector2 basePosition = new(-700,400); // 初期位置を設定
        int line = 0; // 何行目か
        int letter = 0; // line行目の何文字目か
        
        for(var i = 0; i < statement.Length; i++)
        {
            if(letter >= 20)
            {
                line++; // 20文字ごとに1行加算
                letter = 0; // 行の文字リセット
            }

            Vector2 position = basePosition + new Vector2(circleSize * letter, -circleSize * line); // 各文字の位置を計算
            char varchar = statement[i];

            if (varchar == ' ')
            {
                GameObject instance = Instantiate(prefab, statementParent.transform);
                instance.GetComponent<RectTransform>().anchoredPosition = position; // 各文字の位置を設定
                instance.GetComponent<RectTransform>().localScale = new (1, 1); // スケールを設定
                instance.transform.Find("Char").GetComponent<Text>().text = " ";
                instance.transform.Find("Noise").GetComponent<Image>().enabled = true;
                
            } else {
                GameObject instance = Instantiate(prefab, statementParent.transform);
                instance.GetComponent<RectTransform>().anchoredPosition = position;
                instance.GetComponent<RectTransform>().localScale = new (1, 1); // スケールを設定
                instance.transform.Find("Char").GetComponent<Text>().text = varchar.ToString();
                instance.transform.Find("Noise").GetComponent<Image>().enabled = false;
            }

            letter++;
        }
    }

    // タイマーとボタン押せなくする
    private IEnumerator Timer()
    {
        while(seconds > 0)
        {           
            // 1秒間待つ
            yield return new WaitForSeconds(1);

            if (!choised)
            {
                seconds--;
                //　タイマー表示用UIテキストに時間を表示する
                countText.text = seconds.ToString();

                //　制限時間以下になったらコンソールに『制限時間終了』という文字列を表示する
                if (seconds == 0)
                {
                    Debug.Log("制限時間終了");
                    StartCoroutine(TimeUp());
                }
            }
        }
    }

    private IEnumerator TimeUp()
    {
        timeup.SetActive(true);
        Manager.newData.selectedAnswer = 0;
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("AnswerScene");
    }

    private IEnumerator Answer()
    {
        choised = true;
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("AnswerScene");
    }

    // 選択肢を決定した時に呼び出され、どの選択肢を選んだかをセーブし答え合わせ画面に遷移
    public void Answer1()
    {
        if (!choised)
        {
            StartCoroutine(Commons.Button(choise1Rect));
            Manager.newData.selectedAnswer = 1;
            StartCoroutine(Answer());
        }
    }
    public void Answer2()
    {
        if (!choised)
        {
            StartCoroutine(Commons.Button(choise2Rect));
            Manager.newData.selectedAnswer = 2;
            StartCoroutine(Answer());
        }
    }
    public void Answer3()
    {
        if (!choised)
        {
            StartCoroutine(Commons.Button(choise3Rect));
            Manager.newData.selectedAnswer = 3;
            StartCoroutine(Answer());
        }
    }
}
