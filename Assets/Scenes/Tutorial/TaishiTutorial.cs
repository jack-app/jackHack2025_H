using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaishiTutorial : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private RectTransform noiseParent;
    [SerializeField] private Sprite noiseSprite;
    private const int radius = 350;
    private const int size = 700;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voice"))
        {
            string c = collision.transform.Find("Char").GetComponent<Text>().text;
            tutorialManager.GetVoice(c);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Noise"))
        {
            GenerateNoise(collision.GetComponent<RectTransform>());
            Destroy(collision.gameObject);
        }
    }
    private void GenerateNoise(RectTransform rect)
    {
        GameObject newNoise = new("Noise", typeof(Image));
        newNoise.GetComponent<Image>().sprite = noiseSprite;
        newNoise.transform.SetParent(noiseParent, false);
        RectTransform noiseRect = newNoise.GetComponent<RectTransform>();
        float angle = Mathf.Atan2(rect.anchoredPosition.y, rect.anchoredPosition.x);
        noiseRect.anchoredPosition = new(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        noiseRect.sizeDelta = new(size, size);
        StartCoroutine(DestroyNoise(newNoise));
    }
    private IEnumerator DestroyNoise(GameObject noise)
    {
        yield return new WaitForSeconds(2);
        Image image = noise.GetComponent<Image>();
        while (image.color.a > 0)
        {
            Color temp = image.color;
            temp.a -= Time.deltaTime * 2;
            image.color = temp;
            yield return null;
        }
        Destroy(noise);
    }
}