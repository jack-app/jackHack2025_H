using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material m;
    [SerializeField] private RectTransform taishiRect;
    [SerializeField] private GameObject clickToStart;
    private string statement;
    private int voiceNumber = 0;
    private string gotStatement = "";
    private int gotVoice = 0;
    private int canceledNoise = 0;
    private int generatedNoise = 0;
    [SerializeField] private Text quizCharNumber;
    [SerializeField] private Text restCharNumber;
    [SerializeField] private Text gotCharNumber;
    [SerializeField] private Text canceledNoiseNumber;
    private bool isGame = false;    //ゲーム中にtrueになる変数
    private float intervalCount = 0f;
    [SerializeField] private GameObject notesParent;
    [SerializeField] private GameObject voiceNotesPrefab;
    [SerializeField] private GameObject noiseNotesPrefab;
    [SerializeField] private GameObject finish;
    [SerializeField] private GameObject black;
    private const int radius = 580;
    private AudioSource audioSource;
    [SerializeField] private AudioSource finishAudioSource;


    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        audioSource = GetComponent<AudioSource>();
        finish.SetActive(false);
        statement = Manager.quiz[Manager.currentQuiz].statement;
        quizCharNumber.text = (Manager.currentQuiz + 1).ToString() + "/" + Manager.quizNumber.ToString() + "問目";
        restCharNumber.text = "残り" + (statement.Length - voiceNumber).ToString() + "文字";
        gotCharNumber.text = gotVoice.ToString() + "/" + voiceNumber.ToString() + "文字";
        canceledNoiseNumber.text = canceledNoise.ToString() + "/" + generatedNoise.ToString() + "個";
        StartCoroutine(ToStart());

        //StartCoroutine(QuizCharNumber());
        //StartCoroutine(RestCharNumber());
        //StartCoroutine(GotCharNumber());
        //StartCoroutine(CanceledNoiseNumber());
    }

    private IEnumerator ToStart()
    {
        yield return StartCoroutine(Commons.FadeIn(black));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        clickToStart.SetActive(false);
        StartCoroutine(TaishiMove());
        StartCoroutine(BeetMove());
        yield return new WaitForSeconds(Manager.interval * 5);
        isGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGame)
        {
            if (intervalCount > Manager.interval)
            {
                GenerateNotes();
                intervalCount = 0;
                //GenerateNotesで声ノーツが生成されたとき、voiceNumberは次に生成する文字のインデックスとなる
                if (voiceNumber == statement.Length)
                {
                    isGame = false;
                    StartCoroutine(GameFinish());
                }
            }
            else
            {
                intervalCount += Time.deltaTime;
            }
        }
    }

    private IEnumerator GameFinish()
    {
        //ほんとは正確に最後のノーツの消滅を待ちたい
        yield return new WaitForSeconds(1);
        finish.SetActive(true);
        finishAudioSource.Play();
        yield return new WaitForSeconds(1);
        Manager.newData.gotStatement = gotStatement;
        Manager.newData.gotVoice = gotVoice;
        Manager.newData.canceledNoise = canceledNoise;
        Manager.newData.generatedNoise = generatedNoise;        //データをセーブ
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("QuizScene");
    }

    //太子をノらせる
    private IEnumerator TaishiMove()
    {
        while (true)
        {
            Vector2 basePosition = taishiRect.anchoredPosition;
            taishiRect.anchoredPosition = new(basePosition.x, basePosition.y - 20);
            audioSource.Play();
            while (taishiRect.anchoredPosition.y < basePosition.y)
            {
                yield return null;
                Vector2 temp = taishiRect.anchoredPosition;
                temp.y += 20 * Time.deltaTime / (Manager.interval * 2);
                taishiRect.anchoredPosition = temp;
                if (temp.y >= basePosition.y)
                {
                    taishiRect.anchoredPosition = basePosition;
                    yield return null;
                    break;
                }
            }
        }
    }

    //背景を動かす
    private IEnumerator BeetMove()
    {
        while (true)
        {
            while (true)
            {
                float x = m.GetTextureOffset("_MainTex").x;
                x -= Time.deltaTime * 1.2f / (Manager.interval * 8);
                m.SetTextureOffset("_MainTex", new(x, 0));
                yield return null;
                if (x < -0.9f)
                {
                    m.SetTextureOffset("_MainTex", new(0.3f, 0));
                    break;
                }
            }
        }
    }

    ////何問目中何問目かを表示
    //private IEnumerator QuizCharNumber()
    //{
    //    while(true)
    //    {
    //        yield return new WaitUntil(() => isGame);
    //        while (true)
    //        {                
    //            int totalQuizNumber=Manager.quiz.Count;
    //            quizCharNumber.text=(Manager.currentQuiz+1)+"/"+totalQuizNumber.ToString()+"問目";
    //            yield return null;
    //        }
    //    }
        
    //}
    
    ////残りの文字数を表示
    //private IEnumerator RestCharNumber()
    //{
    //    while(true)
    //    {
    //        yield return new WaitUntil(() => isGame);
    //        while (true)
    //        {                
    //            string statement=Manager.quiz[Manager.currentQuiz].statement;
    //            int restNumber=statement.Length-voiceNumber;
    //            restCharNumber.text="残り"+restNumber.ToString()+"文字";
    //            yield return null;
    //        }
    //    }
        
    //}

    ////聞き取った文字数を表示
    //private IEnumerator GotCharNumber()
    //{
    //    while(true)
    //    {
    //        yield return new WaitUntil(() => isGame);
    //        while (true)
    //        {
    //            int charnumber=statement.Length;
    //            gotCharNumber.text = gotVoice.ToString()+"/"+charnumber.ToString()+"文字";
    //            //gotcharnumber.text="/文字だよ";
    //            yield return null;
    //        }
    //    }
        
    //}

    ////キャンセルしたノイズ数を表示
    //private IEnumerator CanceledNoiseNumber()
    //{
    //    while(true)
    //    {
    //        yield return new WaitUntil(() => isGame);
    //        while (true)
    //        {
    //            canceledNoiseNumber.text = canceledNoise.ToString()+"/"+ generatedNoise.ToString()+"個";
    //            //gotcharnumber.text="/文字だよ";
    //            yield return null;
    //        }
    //    }
        
    //}

    private void GenerateNotes()
    {
        Vector2 pos = new (0, 0);
        int rnd = UnityEngine.Random.Range(1, 11);
        int direction = UnityEngine.Random.Range(1, 9);

        switch (direction)
        {
            case 1:
                pos = new(radius, 0);
                break;
            case 2:
                pos = new(radius * Mathf.Cos(Mathf.PI / 4), radius * Mathf.Sin(Mathf.PI / 4));
                break;
            case 3:
                pos = new(0, radius);
                break;
            case 4:
                pos = new(-radius * Mathf.Cos(Mathf.PI / 4), radius * Mathf.Sin(Mathf.PI / 4));
                break;
            case 5:
                pos = new(-radius, 0);
                break;
            case 6:
                pos = new(-radius * Mathf.Cos(Mathf.PI / 4), -radius * Mathf.Sin(Mathf.PI / 4));
                break;
            case 7:
                pos = new(0, -radius);
                break;
            case 8:
                pos = new(radius * Mathf.Cos(Mathf.PI / 4), -radius * Mathf.Sin(Mathf.PI / 4));
                break;
        }
        if(rnd <= 5)
        {
            GameObject notes = Instantiate(voiceNotesPrefab, notesParent.transform);
            notes.GetComponent<RectTransform>().anchoredPosition = pos;
            notes.GetComponent<RectTransform>().localScale = new(1, 1);
            notes.transform.Find("Char").GetComponent<Text>().text = statement[voiceNumber].ToString();
            voiceNumber++;
            restCharNumber.text = "残り" + (statement.Length - voiceNumber).ToString() + "文字";
            gotCharNumber.text = gotVoice.ToString() + "/" + voiceNumber.ToString() + "文字";
        }
        else if(rnd <= 8)
        {
            GameObject notes = Instantiate(noiseNotesPrefab, notesParent.transform);
            notes.GetComponent<RectTransform>().anchoredPosition = pos;
            notes.GetComponent<RectTransform>().localScale = new(1, 1);
            generatedNoise++;
            canceledNoiseNumber.text = canceledNoise.ToString() + "/" + generatedNoise.ToString() + "個";
        }
    }
    public void GetVoice(string c)
    {
        gotStatement += c;
        if (c != " ")
        {
            gotVoice++;
            gotCharNumber.text = gotVoice.ToString() + "/" + voiceNumber.ToString() + "文字";
        }
    }
    public void CancelNoise()
    {
        canceledNoise++;
        canceledNoiseNumber.text = canceledNoise.ToString() + "/" + generatedNoise.ToString() + "個";
    }
    private void OnDestroy()
    {
        m.SetTextureOffset("_MainTex", new(0.3f, 0));
    }
}