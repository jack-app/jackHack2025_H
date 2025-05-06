using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TaishiTutorial : MonoBehaviour
{
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private RectTransform noiseParent;
    [SerializeField] private RectTransform starsParent;
    [SerializeField] private Sprite noiseSprite;
    [SerializeField] private Sprite starsSprite;
    private const int radius = 350;
    private const int size = 700;
    private const int size2 = 225;
    AudioSource audioSource;
    [SerializeField] private AudioClip voiceSE;
    [SerializeField] private AudioClip noiseSE;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voice"))
        {
            string c = collision.transform.Find("Char").GetComponent<Text>().text;
            tutorialManager.GetVoice(c);
            if (c != " ")
            {
                audioSource.clip = voiceSE;
                audioSource.Play();
                GenerateStars();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Noise"))
        {
            GenerateNoise(collision.GetComponent<RectTransform>());
            audioSource.clip = noiseSE;
            audioSource.Play();
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
    private void GenerateStars()
    {
        GameObject newStars = new("Stars", typeof(Image));
        newStars.GetComponent<Image>().sprite = starsSprite;
        newStars.transform.SetParent(starsParent, false);
        RectTransform starsRect = newStars.GetComponent<RectTransform>();
        starsRect.anchoredPosition = new(0, 0);
        starsRect.sizeDelta = new(size2, size2);
        StartCoroutine(DestroyStars(newStars));
    }
    private IEnumerator DestroyStars(GameObject stars)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(stars);
    }
}