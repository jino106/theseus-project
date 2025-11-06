using Cysharp.Threading.Tasks;
using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private int bgmIndex;

    [SerializeField] private bool doFadein;

    [SerializeField] private float fadeInDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (doFadein)
        {
            soundManager.PlayBGMFadeIn(bgmIndex, fadeInDuration).Forget();
        }
        else
        {
            soundManager.PlayBGM(bgmIndex);
        }
    }
}
