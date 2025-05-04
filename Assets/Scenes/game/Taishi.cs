using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taishi : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voice"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Noise"))
        {
            //画面上にノイズを発生させる(Resources.Load<Sprite>("noise");を使用)

            Destroy(collision.gameObject);
        }
    }
}