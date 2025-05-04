using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
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
        string statement;       //��蕶
        List<string> choices;   //�I�����̕���
        int answer;             //�ǂ̑I������������
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
