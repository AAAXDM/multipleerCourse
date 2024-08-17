using NetworkShared;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class EndRoundUi : MonoBehaviour
{
    [Inject] NetworkingClient server;

    [SerializeField] List<RoundState> states;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject waitingText;
    [SerializeField] GameObject playAgainText;
    [SerializeField] GameObject opponentLeftText;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image background;
    [SerializeField] Button playAgainBtn;
    [SerializeField] Button quitBtn;
    [SerializeField] Button acceptButton;

    Vector3 startScale;
    int lobbySceneNumber = 1;

    void Awake()
    {
        startScale = transform.localScale;
        playAgainBtn.onClick.AddListener(PlayAgain);
        quitBtn.onClick.AddListener(Quit);
        acceptButton.onClick.AddListener(Accept);
        FinishGameHandler.OnPlayAgain += PlayAgainCallback;
        FinishGameHandler.OnFinishGame += FinishGameCallback;
    }

    void OnEnable() => LeanTween.scale(panel, Vector2.one, 1f).setEase(LeanTweenType.easeOutBounce);

    void OnDisable()
    {
        panel.transform.localScale = startScale;
        acceptButton.interactable = true;
        playAgainBtn.gameObject.SetActive(true);
        playAgainText.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        waitingText.SetActive(false);
    }

    void OnDestroy()
    {
        playAgainBtn.onClick.RemoveListener(PlayAgain);
        quitBtn.onClick.RemoveListener(Quit);
        acceptButton.onClick.RemoveListener(Accept);
        FinishGameHandler.OnPlayAgain -= PlayAgainCallback;
        FinishGameHandler.OnFinishGame -= FinishGameCallback;
    }

    public void Init(StateType type)
    {
        RoundState state = states.Where(x => x.Type == type).FirstOrDefault();
        text.color = state.TextColor;
        text.text = state.Text;
        background.color = state.BackColor;
    }

    void FinishGameCallback()
    {
        opponentLeftText.SetActive(true);
        waitingText.SetActive(false);
        playAgainText.SetActive(false);
        playAgainBtn.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
    }

    void PlayAgainCallback()
    {
        playAgainBtn.gameObject.SetActive(false);
        playAgainText.SetActive(true);
        acceptButton.gameObject.SetActive(true);
    }

    void PlayAgain()
    {
        playAgainBtn.gameObject.SetActive(false);
        waitingText.SetActive(true);
        SendFinishGameInfoOnServer(false);
    }

    void Accept()
    {
        acceptButton.interactable = false;
        SendFinishGameInfoOnServer(false);
    }

    void Quit()
    {
        quitBtn.interactable = false;
        SceneManager.LoadScene(lobbySceneNumber);
        SendFinishGameInfoOnServer(true);
    }

    void SendFinishGameInfoOnServer(bool isFinished)
    {
        FinishGameRequest response = new()
        {
            IsFinished = isFinished
        };
        server.SendOnServer(response);
    }
}

[Serializable]
public class RoundState
{
    [SerializeField] StateType type;
    [SerializeField] Color backColor;
    [SerializeField] Color textColor;
    [SerializeField] string text;

    public StateType Type => type;
    public Color BackColor => backColor;
    public Color TextColor => textColor;
    public string Text => text;
}
