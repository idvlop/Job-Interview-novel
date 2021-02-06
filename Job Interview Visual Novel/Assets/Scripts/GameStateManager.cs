using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] GameObject AnswersSpace;
    [SerializeField] GameObject QuestionSpace;
    [SerializeField] GameObject ResultsSpace;
    [SerializeField] GameObject NavigationSpace;
    [SerializeField] GameObject WelcomeSpace;

    [Tooltip("Коэффициент порога победы от 0 до 1. Например, 70% очков нужно для победы => равен 0.7")]
    [SerializeField] float SuccesTreshold = 0.7f;

    private GameObject victoryOrDefeatPanel;
    private TMP_Text resultScoreText;
    private TMP_Text mistakesText;

    public delegate void GameStateDelegate();
    public static event GameStateDelegate OnGameStarted;
    public static event GameStateDelegate OnGameEnded;

    public delegate string ResultsGettingText();
    public static event ResultsGettingText GetResultScoreTextNeed;
    public static event ResultsGettingText GetMistakesTextNeed;

    private void OnEnable()
    {
        QuestionManager.OnQuestionsOver += EndGame;
    }

    private void OnDisable()
    {
        QuestionManager.OnQuestionsOver -= EndGame;
    }

    private void EndGame()
    {
        OnGameEnded();

        AnswersSpace.SetActive(false);
        QuestionSpace.SetActive(false);
        NavigationSpace.SetActive(false);

        var maxScore = ProgressManager.GetMaxScoreOrDefault();
        var resultScore = ProgressManager.GetResultScoreOrDefault();
        if (resultScore == -0.01f || maxScore == -0.01f) Debug.LogWarning("!Ошибка доступа к EndGame()");
        var wantedPanelId = resultScore >= SuccesTreshold * maxScore ? 0 : 1;
        print("max = " + maxScore);
        print("res = " + resultScore);
        victoryOrDefeatPanel = ResultsSpace.transform.GetChild(wantedPanelId).gameObject;
        victoryOrDefeatPanel.SetActive(true);

        resultScoreText = victoryOrDefeatPanel.transform
            .GetChild(1) //==ProgressSpace
            .GetChild(0) //==FinalScoreText
            .GetComponent<TMP_Text>();

        mistakesText = victoryOrDefeatPanel.transform
            .GetChild(1) //==ProgressSpace
            .GetChild(2) //==CouldBeBetterText
            .GetComponent<TMP_Text>();

        SetResultScoreText();
        SeMistakesText();
        ResultsSpace.SetActive(true);
    }

    public void StartGame()
    {
        WelcomeSpace.SetActive(false);
        QuestionSpace.SetActive(true);
        AnswersSpace.SetActive(true);
        NavigationSpace.SetActive(true);

        OnGameStarted();
    }

    public void RestartGame()
    {
        ResultsSpace.SetActive(false);
        victoryOrDefeatPanel.SetActive(false);
        StartGame();
    }

    private void SeMistakesText()
    {
        var text = GetMistakesTextNeed();
        mistakesText.text = text;
    }

    private void SetResultScoreText()
    {
        var text = GetResultScoreTextNeed();
        resultScoreText.text = text;
    }
}
