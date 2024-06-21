using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonText : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI text;

    public string Text => text.text;

    public Button Button => button;


    public void SetText(string text) => this.text.text = text;
}
