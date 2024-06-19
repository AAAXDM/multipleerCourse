using UnityEngine;

public class LoadingTweener : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float fadeTime;
    [SerializeField] Vector3 smallSize;

    void OnEnable()
    {
        LeanTween.scale(gameObject, Vector3.one, 0);
        LeanTween.scale(gameObject, smallSize, fadeTime).setDelay(delay)
            .setEase(LeanTweenType.easeInOutCirc)
            .setLoopPingPong();
    }
}
