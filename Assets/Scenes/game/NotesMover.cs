using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesMover : MonoBehaviour
{
    private RectTransform rectTransform;
    private const int speed = 1160;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //0.5秒で中心に到達する(本当はノーツ生成の間隔を使って一律制御したい)
        Vector2 temp = rectTransform.anchoredPosition;
        float angle = Mathf.Atan2(temp.y, temp.x);
        temp.x -= speed * Mathf.Cos(angle) * Time.deltaTime;
        temp.y -= speed * Mathf.Sin(angle) * Time.deltaTime;
        rectTransform.anchoredPosition = temp;
    }
}