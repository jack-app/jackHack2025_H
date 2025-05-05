using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using System.Text.Json;

public class Manager : MonoBehaviour
{
    public static Manager instance; //シングルトン用インスタンス

    public static List<Savedata> savedata = new List<Savedata>(); //セーブデータのリスト
    public static List<Quiz> quiz = new List<Quiz>(); //クイズのリスト
    public static int currentQuiz = 0; //現在のクイズのインデックス
    public static float interval = 0.33f;   //ノーツ生成の間隔

    [System.Serializable]
    public class Savedata
    {
        public string gotStatement;     //�����������蕶
        public int canceledNoise = 0;   //�L�����Z�������m�C�Y�̌�
        public int generatedNoise = 0;  //���������m�C�Y�̌�
        public int selectedAnswer;      //�I�΂ꂽ�I����
    }

    [System.Serializable]
    public class Quiz
    {
        public string statement;       //��蕶
        public List<string> choices;   //�I�����̕���
        public int answer;             //�ǂ̑I������������
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //完成するまでのデバッグ用セーブデータ
            Savedata data = new();
            data.gotStatement = "聖  子が  れたのは 歴何年？";
            data.canceledNoise = 3;
            data.generatedNoise = 3;
            data.selectedAnswer = 2;
            Save(data);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void GenerateQuiz()
    {
        string jsonString = Resources.Load<TextAsset>("quizzes").text;  //パスはビルドしたときに変わってしまう
        //quiz = JsonSerializer.Deserialize<List<Quiz>>(jsonString);
        //Debug.Log(quiz);
    }

    public static void Save(Savedata data)
    {
        savedata.Add(data); //セーブデータをリストに追加
    }
}
