using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
//using System.Linq;
using System;

public class Manager : MonoBehaviour
{
    public static Manager instance; //シングルトン用インスタンス

    public static List<Savedata> savedata = new(); //セーブデータのリスト
    public static Savedata newData = new();              //最新のセーブデータ格納用
    public static List<Quiz> quiz = new(); //クイズのリスト
    public static int quizNumber = 5;   //1回のゲームで5問出題
    public static int currentQuiz = 0; //現在のクイズのインデックス
    public static float interval = 0.33f;   //ノーツ生成の間隔

    [System.Serializable]
    public class Savedata
    {
        public string gotStatement = "";     //聞き取った問題文(聞き取れなかったところは空白)
        public int gotVoice = 0;            //聞き取れた文字数
        public int canceledNoise = 0;   //キャンセルしたノイズの個数
        public int generatedNoise = 0;  //発生したノイズの個数
        public int selectedAnswer;      //選ばれた選択肢
        public bool isCorrect;          //回答が正しかったか(正しかったらtrue)
    }

    [System.Serializable]
    public class Quiz
    {
        public string statement;       //問題文
        public List<string> choices;   //選択肢の文章
        public int answer;             //どの選択肢が正解か
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            DontDestroyOnLoad(gameObject);
            //完成するまでのデバッグ用データ
            if (SceneManager.GetActiveScene().name != "TitleScene")
            {
                newData.gotStatement = "聖  子が  れたのは 歴何年？";
                newData.gotVoice = 11;
                newData.canceledNoise = 3;
                newData.generatedNoise = 3;
                newData.selectedAnswer = 2;
                newData.isCorrect = true;
                Save();
                GenerateQuiz();
                newData.gotStatement = quiz[0].statement;
                newData.selectedAnswer = 2;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //ランダムに5問選び、文章の短い順に並べる
    public static void GenerateQuiz()
    {
        quiz.Clear();
        currentQuiz = 0;
        string jsonString = Resources.Load<TextAsset>("quizzes").text;
        List<Quiz> allQuiz = JsonConvert.DeserializeObject<List<Quiz>>(jsonString);
        List<int> used = new();
        while (quiz.Count < quizNumber)
        {
            int q = UnityEngine.Random.Range(0, allQuiz.Count);
            if (!used.Contains(q))
            {
                quiz.Add(allQuiz[q]);
                used.Add(q);
            }
        }
        //quiz = quiz.OrderBy(x => x.statement.Length).ToList();    短い順にすると総問題数が少ない場合毎回似た順番になってしまう
    }

    public static void Save()
    {
        savedata.Add(newData); //セーブデータをリストに追加
        newData = new();
        currentQuiz++;
    }
    public static void GameEnd()
    {
        //セーブデータを記録するならここで
        savedata.Clear();
    }
}
