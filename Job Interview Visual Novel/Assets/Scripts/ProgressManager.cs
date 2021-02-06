using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ProgressManager : MonoBehaviour
{
    private static Stack<Question> QuestionsStack { get; } = new Stack<Question>();
    private static Stack<float> AnswersRateStack { get; } = new Stack<float>();
    private static float ResultProgressValue { get; set; } = 0;
    private static float MaxProgressValue { get; set; } = 0;

    public static bool IsResultsEvaluated { get; private set; } = false;

    private void OnEnable()
    {
        QuestionManager.OnBackToPrevQuestion += CancelLastAnswer;
        QuestionManager.OnGettingAnswer += AppendAnswer;
        GameStateManager.OnGameStarted += ResetProgress;
        GameStateManager.OnGameEnded += EvaluateProgress;
    }
    private void OnDisable()
    {
        QuestionManager.OnBackToPrevQuestion -= CancelLastAnswer;
        QuestionManager.OnGettingAnswer -= AppendAnswer;
        GameStateManager.OnGameStarted -= ResetProgress;
        GameStateManager.OnGameEnded -= EvaluateProgress;
    }

    private void AppendAnswer(Question question, Answer answer)
    {
        float rate;
        switch (answer.Rate)
        {
            case AnswerRate.Bad:
                rate = question.BadRateValue;
                break;
            case AnswerRate.Ok:
                rate = question.OkRateValue;
                break;
            case AnswerRate.Best:
                rate = question.BestRateValue;
                break;
            default:
                rate = 0;
                break;
        }
        QuestionsStack.Push(question);
        AnswersRateStack.Push(rate);
    }

    private void CancelLastAnswer()
    {
        QuestionsStack.Pop();
        AnswersRateStack.Pop();
    }

    private void EvaluateProgress()
    {
        var len = QuestionsStack.Count;
        var questionsWithMistake = new List<Question>();
        for(var i = 0; i < len; i++)
        {
            var rateValue = AnswersRateStack.Pop();
            ResultProgressValue += rateValue;

            var question = QuestionsStack.Pop();
            var bestRate = question.NeedAnswer ? question.BestRateValue : 0;
            MaxProgressValue += bestRate;
            if (bestRate > rateValue)
                questionsWithMistake.Add(question);
        }
        ResultsManager.HandleResults(ResultProgressValue, MaxProgressValue, questionsWithMistake);
        IsResultsEvaluated = true;
    }

    private void ResetProgress()
    {
        ResultProgressValue = 0;
        MaxProgressValue = 0;
        IsResultsEvaluated = false;
    }

    public static float GetMaxScoreOrDefault()
    {
        return IsResultsEvaluated ? MaxProgressValue : -0.01f;
    }

    public static float GetResultScoreOrDefault()
    {
        return IsResultsEvaluated ? ResultProgressValue : -0.01f;
    }
}
