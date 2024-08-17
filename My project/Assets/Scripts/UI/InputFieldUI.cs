using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldUI : MonoBehaviour
{
    [SerializeField] string defaultText;
    [SerializeField] Color defaultColor;
    [SerializeField] Color inputColor;
    [SerializeField] bool isPassword;

    TMP_InputField input;
    bool isNoFirst;
    bool isSecond;

    void Awake()
    {
        input = GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener(UpdateColor);
    }

    void OnEnable()
    {
        input.textComponent.color = defaultColor;
        input.contentType = TMP_InputField.ContentType.Standard;
        isNoFirst = false;
        isSecond = true;
        input.text = defaultText;
    }

    private void OnDestroy() => input.onValueChanged.RemoveListener(UpdateColor);

    void UpdateColor(string value)
    {
        if(isNoFirst)
        {
            if (isSecond)
            {
                input.textComponent.color = inputColor;
                if (isPassword)
                {
                    input.contentType = TMP_InputField.ContentType.Password;
                }
                isSecond = false;
            }
        }
        isNoFirst = true;
    }
}
