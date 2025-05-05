using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField] GameObject choise1;
    [SerializeField] GameObject choise2;
    [SerializeField] GameObject choise3;
    [SerializeField] GameObject prefab;

    // 問題および選択肢の文章をUIに反映
    void Start()
    {
        string[] choises = Manager.quiz[Manager.currentQuiz].choices.ToArray(); //選択肢の取得

        GenerateStatement();

        choise1.transform.GetChild(0).GetComponent<Text>().text = choises[0];
        choise2.transform.GetChild(0).GetComponent<Text>().text = choises[1];
        choise3.transform.GetChild(0).GetComponent<Text>().text = choises[2];
    }
    
    // Startの中で呼び出され、聞き取れた問題文を表示する。問題文は一文字ずつPrefabとして生成する
    void GenerateStatement()
    {
        string statement = Manager.savedata[Manager.currentQuiz].gotStatement; //問題文の取得
        GameObject parent = GameObject.Find("Canvas"); //親オブジェクトの取得

        Vector2 basePosition = new Vector2(960-(int)(statement.Length/2)*100, 850); // 初期位置を設定
        int line = 0; // 何行目か
        int letter = 0; // line行目の何文字目か
        
        for(var i = 0; i < statement.Length; i++)
        {
            if(letter >= 20)
            {
                line++; // 20文字ごとに1行加算
                letter = 0; // 行の文字リセット
            }

            Vector3 position = basePosition + new Vector2(100 * letter, -line * 50); // 各文字の位置を計算
            char varchar = statement[i];

            if (varchar == ' ')
            {
                var instance = Instantiate(prefab, parent.transform);
                instance.GetComponent<RectTransform>().anchoredPosition = position; // 各文字の位置を設定
                instance.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f); // スケールを設定
                instance.transform.Find("char").GetComponent<Text>().text = " ";
                instance.transform.Find("noise").GetComponent<Image>().enabled = true;
                
            } else {
                var instance = Instantiate(prefab, parent.transform);
                instance.GetComponent<RectTransform>().anchoredPosition = position;
                instance.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f); // スケールを設定
                instance.transform.Find("char").GetComponent<Text>().text = varchar.ToString();
                instance.transform.Find("noise").GetComponent<Image>().enabled = false;
            }

            letter++;
        }
        

    }

    // 選択肢を決定した時に呼び出され、どの選択肢を選んだかをセーブし答え合わせ画面に遷移
    public void Answer()
    {

    }
}
