using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject black;
    [SerializeField] private GameObject panel;
    [SerializeField] private Text explanation;
    [SerializeField] private GameObject notesParent;
    [SerializeField] private GameObject voiceNotesPrefab;
    [SerializeField] private GameObject noiseNotesPrefab;
    [SerializeField] private CancelerTutorial cTutorial;
    private const int radius = 580;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        StartCoroutine(Commons.FadeIn(black));
        StartCoroutine(MainCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MainCoroutine()
    {
        yield return new WaitForSeconds(1);
        panel.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        panel.SetActive(false);
        GenerateVoiceNotes(new(radius, 0), "聖");
        yield return new WaitForSeconds(Manager.interval * 3);
        panel.SetActive(true);
        explanation.text = "今流れてきたのが周囲の人々の声です。全て集めると質問文が完成します。";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        panel.SetActive(false);
        GenerateVoiceNotes(new(0, radius), "徳");
        yield return new WaitForSeconds(Manager.interval * 2);
        panel.SetActive(true);
        explanation.text = "声をノイズキャンセリングしてしまうと、質問に答えるときその文字が読めなくなってしまいます。";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        panel.SetActive(false);
        GenerateNoiseNotes(new(0, radius));
        yield return new WaitForSeconds(Manager.interval * 2);
        panel.SetActive(true);
        explanation.text = "今流れてきたのがノイズです。全力でキャンセルしましょう！";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        panel.SetActive(false);
        GenerateNoiseNotes(new(radius, 0));
        yield return new WaitForSeconds(Manager.interval * 3);
        panel.SetActive(true);
        explanation.text = "太子くんの耳にノイズが入ってしまうと、しばらく画面全体が見にくくなってしまいます。";
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        cTutorial.PermitMove();
        explanation.text = "ノイズキャンセリングはマウス(スマホならタッチ位置)の方向に追従します。ノイキャンしつつみんなの声を聞き取ろう！";
        yield return null;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return));
        panel.SetActive(false);
    }

    private void GenerateVoiceNotes(Vector2 pos, string c)
    {
        GameObject notes = Instantiate(voiceNotesPrefab, notesParent.transform);
        notes.GetComponent<RectTransform>().anchoredPosition = pos;
        notes.GetComponent<RectTransform>().localScale = new(1, 1);
        notes.transform.Find("Char").GetComponent<Text>().text = c;
    }
    private void GenerateNoiseNotes(Vector2 pos)
    {
        GameObject notes = Instantiate(noiseNotesPrefab, notesParent.transform);
        notes.GetComponent<RectTransform>().anchoredPosition = pos;
        notes.GetComponent<RectTransform>().localScale = new(1, 1);
    }
    public void Title()
    {
        SceneManager.LoadScene("TitleScene");
    }
}