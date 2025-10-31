using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class FadeController : MonoBehaviour
{
    // 透明度を操作する真っ黒な画像
    private Image fadeImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // コンポーネントを取得
        fadeImage = GetComponent<Image>();

        // コンポーネントがアタッチされているかチェック
        if (fadeImage == null)
        {
            Debug.LogError("FadeController: fadeImage cannot found.");
        }
        // アタッチされている場合
        else
        {
            /*
            // fadeImageの色を黒にする
            fadeImage.color = Color.black;
            // フェードイン開始
            FadeIn(4.0f).Forget();
            */
        }
    }

    // フェードインを実行する関数
    public async UniTask FadeIn(float duration)
    {
        if (fadeImage != null)
        {
            float startTime = Time.time;
            float alpha = 0.0f;
            while (Time.time - startTime < duration)
            {
                alpha += Time.deltaTime / duration;
                fadeImage.color = new Color(0, 0, 0, 1 - alpha);
                await UniTask.Yield(PlayerLoopTiming.Update); // 毎フレーム待機
            }
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    // フェードアウトを実行する関数
    public async UniTask FadeOut(float duration)
    {
        if (fadeImage != null)
        {
            float startTime = Time.time;
            float alpha = 0.0f;
            while (Time.time - startTime < duration)
            {
                alpha += Time.deltaTime / duration;
                fadeImage.color = new Color(0, 0, 0, alpha);
                await UniTask.Yield(PlayerLoopTiming.Update); // 毎フレーム待機
            }
            fadeImage.color = new Color(0, 0, 0, 1);
        }
    }

}
