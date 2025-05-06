using UnityEngine;

public class NotesMover : MonoBehaviour
{
    private RectTransform rectTransform;
    private float speed;
    private const int distance = 345;         //ノーツの半径とノイキャンの位置も踏まえた、ノーツ発射から衝突までの距離
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        speed = distance / (Manager.interval * 2);          //発射からノーツ生成2回分でノイキャンに到達
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 temp = rectTransform.anchoredPosition;
        float angle = Mathf.Atan2(temp.y, temp.x);
        temp.x -= speed * Mathf.Cos(angle) * Time.deltaTime;
        temp.y -= speed * Mathf.Sin(angle) * Time.deltaTime;
        rectTransform.anchoredPosition = temp;
    }
}