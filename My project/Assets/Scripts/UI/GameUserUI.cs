using TMPro;
using UnityEngine;

public class GameUserUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userName;
    [SerializeField] TextMeshProUGUI userScore;

    public void SetUsername(string name) => userName.text = name;

    public void SetScore(int score) => userScore.text = score.ToString();
}
