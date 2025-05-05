using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public Text Text_Letters_Sum;
    public Text Text_Noise_Sum;
    public Text Text_Score_Sum;
    int heardCharsSum = 0;
    int canceledNoiseSum = 0;
    int correctAnswers; //正答数がsavedataから見れるようになってから
    // Start is called before the first frame update
    void Start()
    {
        foreach(Manager.Savedata data in Manager.savedata)
        {
            for(int i = 0; i < data.gotStatement.Length; i++){
                if (data.gotStatement[i] != ' ') 
                {
                    heardCharsSum += 1;
                }
            }
            canceledNoiseSum += data.canceledNoise;
            //正答数がsavedataから見れるようになってから処理を追加
        }
        showHeardChars();
        showCanceledNoise();
        showCorrectAnswers();
    }

    void showHeardChars()
    {
        Text_Letters_Sum.text = heardCharsSum.ToString();
    }

    void showCanceledNoise()
    {
        Text_Noise_Sum.text = canceledNoiseSum.ToString();
    }

    void showCorrectAnswers()//正答数がsavedataから見れるようになってから
    {
        Text_Score_Sum.text = correctAnswers.ToString();
    }

    // Click to Go title Scene
    public void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
