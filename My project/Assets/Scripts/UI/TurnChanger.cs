using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TurnChanger : MonoBehaviour
{
    TextMeshProUGUI textToChange;
    StringBuilder sb;
    Vector3 startScale = new Vector3 (0.7f, 0.7f, 0.7f);
    string replace = "{}";
    float tweenTime = 0.3f;

    void Awake()
    {
        textToChange = GetComponent<TextMeshProUGUI>();
        sb = new();
    }

    public void ChangePlayerTurn(string player)
    {
        sb.Clear();
        sb.Append(textToChange.text);
        sb.Replace(replace, player);
        replace = player;
        textToChange.text = sb.ToString();
        Animate();
    }

    void Animate()
    {
        LeanTween.scale(textToChange.gameObject, startScale, tweenTime).setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(x => LeanTween.scale(textToChange.gameObject, Vector3.one, tweenTime).
                                                        setEase(LeanTweenType.easeInOutBack));
    }
}
