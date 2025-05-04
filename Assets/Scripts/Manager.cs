using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager : MonoBehaviour
{
    public static Manager instance; //シングルトン用インスタンス

    public List<Savedata> savedata = new List<Savedata>(); //セーブデータのリスト
    public List<Quiz> quiz = new List<Quiz>(); //クイズのリスト
    public int currentQuiz = 0; //現在のクイズのインデックス

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

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void GenerateQuiz()
    {
        string filepath;
        string fileName = "Data.json";
        filepath = Application.dataPath + "/" + fileName;  

    }

    public void Save(Savedata data)
    {
        savedata.Add(data); //セーブデータをリストに追加
    }
}
