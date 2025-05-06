using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject gameTutorial;
    [SerializeField] private GameObject quizTutorial;
    [SerializeField] private Material m;
    [SerializeField] private RectTransform taishiRect;
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject panel;
    [SerializeField] private Text explanation;
    [SerializeField] private GameObject notesParent;
    [SerializeField] private Text remainNumber;
    [SerializeField] private Text gotCharNumber;
    [SerializeField] private Text canceledNoiseNumber;
    [SerializeField] private GameObject voiceNotesPrefab;
    [SerializeField] private GameObject noiseNotesPrefab;
    [SerializeField] private CancelerTutorial cTutorial;
    private const int radius = 580;
    private const string statement = "聖徳太子はどれ？";
    private int voiceNumber = 0;
    private string gotStatement = "";
    private int gotVoice = 0;
    private int canceledNoise = 0;
    private int generatedNoise = 0;
    private bool isGame = false;
    private float intervalCount = 0f;
    [SerializeField] private GameObject finish;
    [SerializeField] private GameObject statementParent;
    [SerializeField] private GameObject charPrefab;
    private AudioSource audioSource;
    [SerializeField] private AudioSource finishAudioSource;
    [SerializeField] private AudioClip systemSE;
    [SerializeField] private AudioClip beetSE;
    private Coroutine taishiCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        audioSource = GetComponent<AudioSource>();
        quizTutorial.SetActive(false);
        panel.SetActive(false);
        finish.SetActive(false);
        remainNumber.text = "残り" + (statement.Length - voiceNumber).ToString() + "文字";
        gotCharNumber.text = gotVoice.ToString() + "/" + voiceNumber.ToString() + "文字";
        canceledNoiseNumber.text = canceledNoise.ToString() + "/" + generatedNoise.ToString() + "個";
        StartCoroutine(Commons.FadeIn(black));
        StartCoroutine(MainCoroutine());
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
                    StartCoroutine(SceneChange());
                }
            }
            else
            {
                intervalCount += Time.deltaTime;
            }
        }
    }

    private IEnumerator MainCoroutine()
    {
        yield return new WaitForSeconds(1);
        panel.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.clip = systemSE;
        audioSource.Play();
        panel.SetActive(false);
        GenerateVoiceNotes(new(radius, 0), "聖");
        yield return new WaitForSeconds(Manager.interval * 3);
        panel.SetActive(true);
        explanation.text = "今流れてきたのが周囲の人々の声です。全て集めると質問文が完成します。";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.Play();
        panel.SetActive(false);
        GenerateVoiceNotes(new(0, radius), "徳");
        yield return new WaitForSeconds(Manager.interval * 2);
        panel.SetActive(true);
        explanation.text = "声をノイズキャンセリングしてしまうと、質問に答えるときその文字が読めなくなってしまいます。";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.Play();
        panel.SetActive(false);
        GenerateNoiseNotes(new(0, radius));
        yield return new WaitForSeconds(Manager.interval * 2);
        panel.SetActive(true);
        explanation.text = "今流れてきたのがノイズです。全力でキャンセルしましょう！";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.Play();
        panel.SetActive(false);
        GenerateNoiseNotes(new(radius, 0));
        yield return new WaitForSeconds(Manager.interval * 3);
        panel.SetActive(true);
        explanation.text = "太子くんの耳にノイズが入ってしまうと、しばらく画面全体が見にくくなってしまいます。";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.Play();
        cTutorial.PermitMove();
        explanation.text = "ノイズキャンセリングはマウス(スマホならタッチ位置)の方向に追従します。ノイキャンしつつみんなの声を聞き取ろう！";
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        panel.SetActive(false);
        isGame = true;
        taishiCoroutine = StartCoroutine(TaishiMove());
        StartCoroutine(BeetMove());
    }
    private void GenerateNotes()
    {
        Vector2 pos = new(0, 0);
        int rnd = UnityEngine.Random.Range(1, 11);
        int direction = UnityEngine.Random.Range(1, 9);

        switch (direction)
        {
            case 1:
                pos = new Vector2(radius, 0);
                break;
            case 2:
                pos = new Vector2(radius * Mathf.Cos(Mathf.PI / 4), radius * Mathf.Sin(Mathf.PI / 4));
                break;
            case 3:
                pos = new Vector2(0, radius);
                break;
            case 4:
                pos = new Vector2(-radius * Mathf.Cos(Mathf.PI / 4), radius * Mathf.Sin(Mathf.PI / 4));
                break;
            case 5:
                pos = new Vector2(-radius, 0);
                break;
            case 6:
                pos = new Vector2(-radius * Mathf.Cos(Mathf.PI / 4), -radius * Mathf.Sin(Mathf.PI / 4));
                break;
            case 7:
                pos = new Vector2(0, -radius);
                break;
            case 8:
                pos = new Vector2(radius * Mathf.Cos(Mathf.PI / 4), -radius * Mathf.Sin(Mathf.PI / 4));
                break;
        }
        if (rnd <= 5)
        {
            GenerateVoiceNotes(pos, statement[voiceNumber].ToString());
        }
        else if (rnd <= 8)
        {
            GenerateNoiseNotes(pos);
        }
    }
    private void GenerateVoiceNotes(Vector2 pos, string c)
    {
        GameObject notes = Instantiate(voiceNotesPrefab, notesParent.transform);
        notes.GetComponent<RectTransform>().anchoredPosition = pos;
        notes.GetComponent<RectTransform>().localScale = new(1, 1);
        notes.transform.Find("Char").GetComponent<Text>().text = c;
        voiceNumber++;
        remainNumber.text = "残り" + (statement.Length - voiceNumber).ToString() + "文字";
        gotCharNumber.text = gotVoice.ToString() + "/" + voiceNumber.ToString() + "文字";
    }
    private void GenerateNoiseNotes(Vector2 pos)
    {
        GameObject notes = Instantiate(noiseNotesPrefab, notesParent.transform);
        notes.GetComponent<RectTransform>().anchoredPosition = pos;
        notes.GetComponent<RectTransform>().localScale = new(1, 1);
        generatedNoise++;
        canceledNoiseNumber.text = canceledNoise.ToString() + "/" + generatedNoise.ToString() + "個";
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
    private IEnumerator SceneChange()
    {
        //ほんとは最後のノーツ消滅までを待つ
        yield return new WaitForSeconds(1f);
        finish.SetActive(true);
        finishAudioSource.Play();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Commons.FadeOut(black));
        gameTutorial.SetActive(false);
        StopCoroutine(taishiCoroutine);
        quizTutorial.SetActive(true);
        GenerateStatement();
        yield return StartCoroutine(Commons.FadeIn(black));
        explanation.text = "ノイキャンで太子くんをサポートしたら、次は質問に答えることになります。";
        panel.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.clip = systemSE;
        audioSource.Play();
        explanation.text = "回答には制限時間がありますが、しっかり聞き取れていたらきっと答えられるはず！間違えると人々の罵倒を受けることも……。";
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.Play();
        explanation.text = "立派な聖徳太子を目指し、ノイキャンもクイズも頑張りましょう！";
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        audioSource.Play();
        yield return StartCoroutine(Commons.FadeOut(black));
        SceneManager.LoadScene("TitleScene");
    }
    public void Title()
    {
        audioSource.clip = systemSE;
        audioSource.Play();
        SceneManager.LoadScene("TitleScene");
    }

    //太子をノらせる
    private IEnumerator TaishiMove()
    {
        while (true)
        {
            Vector2 basePosition = taishiRect.anchoredPosition;
            taishiRect.anchoredPosition = new(basePosition.x, basePosition.y - 20);
            audioSource.clip = beetSE;
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
    void GenerateStatement()
    {
        int circleSize = 70;

        Vector2 basePosition = new(-700, 400); // 初期位置を設定
        int line = 0; // 何行目か
        int letter = 0; // line行目の何文字目か

        for (var i = 0; i < gotStatement.Length; i++)
        {
            if (letter > 20)
            {
                line++; // 20文字ごとに1行加算
                letter = 0; // 行の文字リセット
            }

            Vector2 position = basePosition + new Vector2(circleSize * letter, -circleSize * line); // 各文字の位置を計算
            char varchar = gotStatement[i];

            if (varchar == ' ')
            {
                GameObject instance = Instantiate(charPrefab, statementParent.transform);
                instance.GetComponent<RectTransform>().anchoredPosition = position; // 各文字の位置を設定
                instance.GetComponent<RectTransform>().localScale = new(1, 1); // スケールを設定
                instance.transform.Find("Char").GetComponent<Text>().text = " ";
                instance.transform.Find("Noise").GetComponent<Image>().enabled = true;

            }
            else
            {
                GameObject instance = Instantiate(charPrefab, statementParent.transform);
                instance.GetComponent<RectTransform>().anchoredPosition = position;
                instance.GetComponent<RectTransform>().localScale = new(1, 1); // スケールを設定
                instance.transform.Find("Char").GetComponent<Text>().text = varchar.ToString();
                instance.transform.Find("Noise").GetComponent<Image>().enabled = false;
            }

            letter++;
        }
    }
    private void OnDestroy()
    {
        m.SetTextureOffset("_MainTex", new(0.3f, 0));
    }
}