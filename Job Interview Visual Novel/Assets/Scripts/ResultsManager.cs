using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResultsManager : MonoBehaviour
{
    public static string ResultScoreText = "";
    public static string MistakesText = "";

    private void OnEnable()
    {
        GameStateManager.OnGameStarted += OnGameStarted;
        GameStateManager.GetResultScoreTextNeed += GiveResultScoreText;
        GameStateManager.GetMistakesTextNeed += GiveMistakesText;
    }

    private void OnDisable()
    {
        GameStateManager.OnGameStarted -= OnGameStarted;
        GameStateManager.GetResultScoreTextNeed -= GiveResultScoreText;
        GameStateManager.GetMistakesTextNeed -= GiveMistakesText;
    }

    public static void HandleResults(float resultScore, float maxScore, List<Question> mistakesList)
    {
        Func<float, float, string> GetResultScoreText = (result, max) =>
        {
            return "Результат: " + result + " из " + max + "возможных баллов ";
        };

        Func<List<Question>, string> GetMistakesText = (list) =>
        {
            var text = "";
            foreach (var question in list)
            {
                text += "#" + question.Id + " " + question.Name + "\n";
            }
            return text;
        };

        ResultScoreText = GetResultScoreText(resultScore, maxScore);
        MistakesText = GetMistakesText(mistakesList);
    }

    private string GiveResultScoreText()
    {
        return ResultScoreText;
    }

    private string GiveMistakesText()
    {
        return MistakesText;
    }

    private void OnGameStarted()
    {
        ResultScoreText = "";
        MistakesText = "";
    }
}
