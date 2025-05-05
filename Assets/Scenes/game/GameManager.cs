using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material m;
    [SerializeField] private RectTransform taishiRect;
    private bool isGame = false;    //ゲーム中にtrueになる変数
    private float intervalCount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        isGame = true;  //本来はクリックなどでプレイヤーが開始させてからtrueになる
        StartCoroutine(TaishiMove());
        StartCoroutine(BeetMove());
    }

    // Update is called once per frame
    void Update()
    {
        if (isGame)
        {
            if (intervalCount > Manager.interval)
            {
                intervalCount = 0;
            }
            else
            {
                intervalCount += Time.deltaTime;
            }
        }
    }




    //太子をノらせる
    private IEnumerator TaishiMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => isGame);
            Vector2 basePosition = taishiRect.anchoredPosition;
            taishiRect.anchoredPosition = new(basePosition.x, basePosition.y - 20);
            while (taishiRect.anchoredPosition.y < basePosition.y)
            {
                yield return null;
                Vector2 temp = taishiRect.anchoredPosition;
                temp.y += 20 * Time.deltaTime / (Manager.interval * 2);
                taishiRect.anchoredPosition = temp;
                if (temp.y >= basePosition.y)
                {
                    taishiRect.anchoredPosition = basePosition;
                    yield return null;
                    break;
                }
            }
        }
    }

    //背景を動かす
    private IEnumerator BeetMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => isGame);
            while (true)
            {
                float x = m.GetTextureOffset("_MainTex").x;
                x -= Time.deltaTime * 1.2f / (Manager.interval * 8);
                m.SetTextureOffset("_MainTex", new(x, 0));
                yield return null;
                if (x < -0.9f)
                {
                    m.SetTextureOffset("_MainTex", new(0.3f, 0));
                    break;
                }
            }
        }
    }
    private void OnDestroy()
    {
        m.SetTextureOffset("_MainTex", new(0.3f, 0));
    }
}