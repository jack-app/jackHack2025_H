using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnswerManager : MonoBehaviour
{
    /*みっきーのコメント
      通常の変数名は小文字で始めることが多い
      TaishiはImageでなく直接RectTransformで受け取っても良い(Image.recttransformでアクセスできるのは自分も知らなかった！)
      今回は問題ないが、オブジェクトをアタッチしたいときはpublicより[SerializeField] privateの方が安全(他のプログラムがアクセスできないため)
    */

    // 模範解答
    public Text Answer;
    // 出題者からのコメント
    public Text Comment;
    // 聖徳太子
    public Image Taishi;
    
    // Start is called before the first frame update
    void Start()
    {
        int playerAnswer = 1;
        int correctAnswer = 2;

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
        int currentPlayCount = 1;
        // ゲーム終了となるクイズ解答回数
        int gamePlayCount = 5;
        
        if (currentPlayCount == gamePlayCount) {
            SceneManager.LoadScene("GameScene");
        } else {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
