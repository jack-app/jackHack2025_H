using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [System.Serializable]
    public class Savedata
    {
        public string gotStatement;     //聞き取った問題文
        public int canceledNoise = 0;   //キャンセルしたノイズの個数
        public int generatedNoise = 0;  //発生したノイズの個数
        public int selectedAnswer;      //選ばれた選択肢
    }

    [System.Serializable]
    public class Quiz
    {
        string statement;       //問題文
        List<string> choices;   //選択肢の文章
        int answer;             //どの選択肢が正解か
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
