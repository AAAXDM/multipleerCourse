using TMPro;
using UnityEngine;

public class UserUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] GameObject online;
    [SerializeField] GameObject offline;

    public TextMeshProUGUI Username => username;
    public TextMeshProUGUI Score => score;


    public void SetOnlineStatus(bool isOnline)
    {
        online.SetActive(isOnline);
        offline.SetActive(!isOnline);
    }
}
