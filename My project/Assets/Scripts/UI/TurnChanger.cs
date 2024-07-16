using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TurnChanger : MonoBehaviour
{
    TextMeshProUGUI textToChange;
    StringBuilder sb;

    void Awake()
    {
        textToChange = GetComponent<TextMeshProUGUI>();
        sb = new();
    }

    public void ChangePlayerTurn(string player)
    {
        sb.Clear();
        sb.Append(textToChange.text);
        sb.Replace("{}", player);
        textToChange.text = sb.ToString();
    }
}
