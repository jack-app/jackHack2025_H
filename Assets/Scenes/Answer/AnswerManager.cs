using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class AnswerManager : MonoBehaviour
{
    // 模範解答
    [SerializeField] private Text answer;
    // 出題者からのコメント
    [SerializeField] private Text comment;
    [SerializeField] private Text comment2;
    // 聖徳太子
    [SerializeField] private Image taishi;

    [SerializeField] private RectTransform buttonRect;
    [SerializeField] private Text buttonText;
    [SerializeField] private GameObject black;

    private readonly List<string> goodComments = new() { "さすが太子！！", "今日も髭がかっこいい！" };
    private readonly List<string> badComments = new() {"聞き取れてねーじゃん！！" , "なんだよ変な髭しやがって！"};

    // Start is called before the first frame update
    void Start()
    {
        black.SetActive(false);
        black.GetComponent<Image>().color = new(0, 0, 0, 0);
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        Manager.Quiz currentQuiz = Manager.quiz[Manager.currentQuiz];
        
        int playerAnswer = Manager.newData.selectedAnswer;
        int correctAnswer = currentQuiz.answer;

        var answerString = "";
        switch (correctAnswer) {
            case 1 : answerString = "①"; break;
            case 2 : answerString = "②"; break;
            case 3 : answerString = "③"; break;
        }

        answer.text = answerString + " " + currentQuiz.choices[correctAnswer-1];

        if (playerAnswer == correctAnswer)
        {
            int index1 = UnityEngine.Random.Range(0, goodComments.Count);
            comment.text = goodComments[index1];
            goodComments.RemoveAt(index1);
            int index2 = UnityEngine.Random.Range(0, goodComments.Count);
            comment2.text = goodComments[index2];
            taishi.transform.Find("Star").gameObject.SetActive(true);
            Manager.newData.isCorrect = true;
            Manager.Save();
        }
        else
        {
            int index1 = UnityEngine.Random.Range(0, badComments.Count);
            comment.text = badComments[index1];
            badComments.RemoveAt(index1);
            int index2 = UnityEngine.Random.Range(0, badComments.Count);
            comment2.text = badComments[index2];
            taishi.rectTransform.Rotate(new Vector3(0, 0, -20));
            taishi.transform.Find("ase").gameObject.SetActive(true);
            Manager.newData.isCorrect = false;
            Manager.Save();
        }
        if (Manager.currentQuiz == Manager.quizNumber)
        {
            buttonText.text = "終了";
        }
    }

    public void Next()
    {
        StartCoroutine(Commons.Button(buttonRect));
        StartCoroutine(NextCoroutine());
    }
    private IEnumerator NextCoroutine()
    {
        yield return StartCoroutine(Commons.FadeOut(black));
        if (Manager.currentQuiz != Manager.quizNumber)
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
