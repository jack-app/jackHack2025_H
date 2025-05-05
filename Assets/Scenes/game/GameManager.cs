using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material m;
    [SerializeField] private RectTransform taishiRect;
    [SerializeField] private Text gotcharnumber;
    private bool isGame = false;    //ゲーム中にtrueになる変数
    private float intervalCount = 0f;
    [SerializeField] private GameObject notesParent;
    [SerializeField] private GameObject voiceNotesPrefab;
    [SerializeField] private GameObject noiseNotesPrefab;
    private const int radius = 580;
    private int voiceNumber = 0;
    private int generatedNoise = 0;
    // Start is called before the first frame update
    void Start()
    {
        isGame = true;  //本来はクリックなどでプレイヤーが開始させてからtrueになる
        StartCoroutine(TaishiMove());
        StartCoroutine(BeetMove());
        //StartCoroutine(Gotcharnumber());
    }

    // Update is called once per frame
    void Update()
    {
        if (isGame)
        {
            if (intervalCount > Manager.interval)
            {
                intervalCount = 0;
            }
            else
            {
                intervalCount += Time.deltaTime;
            }
        }
    }




    //太子をノらせる
    private IEnumerator TaishiMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => isGame);
            Vector2 basePosition = taishiRect.anchoredPosition;
            taishiRect.anchoredPosition = new(basePosition.x, basePosition.y - 20);
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
            yield return new WaitUntil(() => isGame);
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
    
    //聞き取った文字数を表示
    private IEnumerator Gotcharnumber()
    {
        while(true)
        {
            yield return new WaitUntil(() => isGame);
            while (true)
            {

                //Manager.Savedata savedata = new Manager.Savedata();
                //Manager.Quiz quiz = new Manager.Quiz();
                //Manager.Savedata savedata=Manager.savedata[0];
                //Manager.Quiz quiz = Manager.quiz[0];

                List<Manager.Savedata> savedata = Manager.savedata; //セーブデータのリスト
                List<Manager.Quiz> quiz = Manager.quiz; //クイズのリスト
                string gotstatement=savedata[0].gotStatement;
                string statement=quiz[0].statement;
                int gotnumber=gotstatement.Length;
                int charnumber=statement.Length;
                gotcharnumber.text=gotnumber.ToString()+"/"+charnumber.ToString()+"/文字だよ";
                //gotcharnumber.text="/文字だよ";
                yield return null;
            }
        }
        
    }
    private void OnDestroy()
    {
        m.SetTextureOffset("_MainTex", new(0.3f, 0));
    }

    private void GenerateNotes()
    {
        Vector2 pos = new Vector2(0, 0);
        int rnd = Random.Range(1, 10);
        int direction = Random.Range(1, 8);

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
        if(rnd <= 5)
        {
            GameObject notes = Instantiate(voiceNotesPrefab, notesParent.transform);
            notes.GetComponent<RectTransform>().anchoredPosition = pos;
            notes.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            notes.transform.Find("Char").GetComponent<Text>().text = "t";
            voiceNumber++;
        }
        else if(rnd <= 8)
        {
            GameObject notes = Instantiate(noiseNotesPrefab, notesParent.transform);
            notes.GetComponent<RectTransform>().anchoredPosition = pos;
            notes.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            generatedNoise++;
        }
    }
}