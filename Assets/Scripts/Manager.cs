using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager instance; //シングルトン用インスタンス

    public static List<Savedata> savedata = new List<Savedata>(); //セーブデータのリスト
    public static Savedata newData = new Savedata();              //最新のセーブデータ格納用
    public static List<Quiz> quiz = new List<Quiz>(); //クイズのリスト
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
            DontDestroyOnLoad(gameObject);
            //完成するまでのデバッグ用セーブデータ
            if (SceneManager.GetActiveScene().name != "TitleScene")
            {
                newData.gotStatement = "聖  子が  れたのは 歴何年？";
                newData.gotVoice = 11;
                newData.canceledNoise = 3;
                newData.generatedNoise = 3;
                newData.selectedAnswer = 2;
                newData.isCorrect = true;
                Save();
                if(Manager.quiz.Count != 0){
                    GenerateQuiz();  // クイズ読み込み
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void GenerateQuiz()
    {
        string jsonString = Resources.Load<TextAsset>("quizzes").text;  //パスはビルドしたときに変わってしまう
        quiz = JsonConvert.DeserializeObject<List<Quiz>>(jsonString);
        Debug.Log(quiz);
    }

    public static void Save()
    {
        savedata.Add(newData); //セーブデータをリストに追加
        newData = new();
    }
}
