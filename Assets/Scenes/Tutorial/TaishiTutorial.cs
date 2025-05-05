using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaishiTutorial : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voice"))
        {
            string c = collision.transform.Find("Char").GetComponent<Text>().text;
            tutorialManager.GetChar(c);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Noise"))
        {

            Destroy(collision.gameObject);
        }
    }
    
}