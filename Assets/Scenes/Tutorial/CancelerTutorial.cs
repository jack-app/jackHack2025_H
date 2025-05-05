using UnityEngine;
using UnityEngine.UI;

public class CancelerTutorial : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;
    private RectTransform cancelerRect;
    private Vector2 mousePosition;
    private const int radius = 150;
    private int screenWidth;
    private int screenHeight;
    private bool canMove = false;
    // Start is called before the first frame update
    void Start()
    {
        cancelerRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            mousePosition = Input.mousePosition;    //マウスの位置(画面左下が(0,0))
            screenWidth = Screen.width;   // 画面の横幅
            screenHeight = Screen.height; // 画面の縦幅
            if (mousePosition != null)
            {
                mousePosition.x -= screenWidth / 2;
                mousePosition.y -= screenHeight / 2;        //画面中心が(0,0)になるよう補正
                float angle = Mathf.Atan2(mousePosition.y, mousePosition.x);
                cancelerRect.anchoredPosition = new(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                cancelerRect.localEulerAngles = new(0, 0, angle * Mathf.Rad2Deg - 90);
            }
        }
    }
    public void PermitMove()
    {
        canMove = true;
    }
    public void ProhibitMove()
    {
        canMove = false;
        cancelerRect.anchoredPosition = new(0, radius);
        cancelerRect.localEulerAngles = new(0, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voice"))
        {
            //声は順番が前後してほしくない、ここでは透明にするだけ
            collision.gameObject.GetComponent<Image>().color = new(1, 1, 1, 0);
            collision.transform.Find("Char").GetComponent<Text>().text = " ";
        }
        else if (collision.CompareTag("Noise"))
        {
            tutorialManager.CancelNoise();
            Destroy(collision.gameObject);
        }
    }
}