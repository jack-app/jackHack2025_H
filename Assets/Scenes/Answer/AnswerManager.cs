using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    // 模範解答
    public UnityEngine.UI.Text Answer;
    // 出題者からのコメント
    public UnityEngine.UI.Text Comment;
    // 聖徳太子
    public UnityEngine.UI.Image Taishi;
    
    // Start is called before the first frame update
    void Start()
    {
        const int playerAnswer = 1;
        const int correctAnswer = 2;

        Answer.text = correctAnswer.ToString();

        if (playerAnswer == correctAnswer)
        {
            Comment.text = "さすが太子！！\n\n今日も髭がかっこいい！";
        }
        else
        {
            Comment.text = "聞き取れてねーじゃん！！\n\nなんだよ変な髭しやがって！";
            Taishi.rectTransform.Rotate(new Vector3(0, 0, -20));
        }

    }

    public void Next()
    {
        // 現在のクイズ解答回数
        const int currentPlayCount = 1;
        // ゲーム終了となるクイズ解答回数
        const int gamePlayCount = 5;
        
        if (currentPlayCount == gamePlayCount) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        } else {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ResultScene");
        }
    }
}
