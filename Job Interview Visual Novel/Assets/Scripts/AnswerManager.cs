using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerManager : MonoBehaviour
{
    [SerializeField] GameObject AnswerSpace;

    private static Question currentQuestion;

    public delegate void AnswerDelegate(AnswerButton answerButton);
    public static event AnswerDelegate AnswerSubmitted;

    private void OnEnable()
    {
        QuestionManager.QuestionChangedTo += QuestionChangedTo;
    }

    private void OnDisable()
    {
        QuestionManager.QuestionChangedTo -= QuestionChangedTo;
    }

    private void QuestionChangedTo(Question question)
    {
        currentQuestion = question;
        var answersCount = question.AnswersArray.Length;
        SetActiveWantedAnswerPanel(answersCount);
        var currentPanel = AnswerSpace.transform.GetChild(answersCount - 1);
        SetAnswersToButtons(currentPanel, answersCount);
    }

    private void SetActiveWantedAnswerPanel(int answersCount)
    {
        for(var i=0; i < 3; i++)
        {
            AnswerSpace.transform.GetChild(i).gameObject.SetActive(i == answersCount - 1);
        }
    }

    private void SetAnswersToButtons(Transform answersPanel, int answersCount)
    {
        for (var i = 0; i < answersCount; i++)
        {
            var currentButton = answersPanel.GetChild(i);
            var s = currentButton.GetComponent<AnswerButton>();
            s.Answer = currentQuestion.AnswersArray[i];
            var textOnButton = currentButton.GetChild(0).GetComponent<TMP_Text>();
            textOnButton.text = s.Answer.Text;
        }
    }

    public void OnSubmitAnswer(AnswerButton answerButton)
    {
        AnswerSubmitted(answerButton);
    }
}
