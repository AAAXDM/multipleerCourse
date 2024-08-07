using System.Text;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TurnChanger : MonoBehaviour
{
    TextMeshProUGUI textToChange;
    StringBuilder sb;
    string replace = "{}";

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
    }
}
