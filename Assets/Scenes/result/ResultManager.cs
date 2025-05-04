using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Click to Go title Scene
    public void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
