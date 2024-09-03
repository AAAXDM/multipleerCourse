using UnityEngine;

public class LoadingTweener : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] float fadeTime;
    [SerializeField] Vector3 smallSize;

    int id;
    void OnEnable()
    {
        LeanTween.scale(gameObject, Vector3.one, 0);
        id  = LeanTween.scale(gameObject, smallSize, fadeTime).setDelay(delay)
            .setEase(LeanTweenType.easeInOutCirc)
            .setLoopPingPong().id;
    }

    void OnDisable()
    {
        LeanTween.cancel(id);
    }
}
