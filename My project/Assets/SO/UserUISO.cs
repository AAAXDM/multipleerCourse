using UnityEngine;

[CreateAssetMenu(fileName = "UserUISO", menuName = "UserUISO", order = 52)]
public class UserUISO : ScriptableObject
{
    [SerializeField] UserUI userUI;

    public UserUI UserUI => userUI;
}
