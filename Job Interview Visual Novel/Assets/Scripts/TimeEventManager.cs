using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeEventManager : MonoBehaviour
{
    public GameObject TE_SpaceObject;
    public TMP_Text TE_Text;

    private static float timeLimit;
    private float timer;

    public delegate void TimeEventDelegate();
    public static event TimeEventDelegate OnTimeOver;

    private void OnEnable()
    {
        QuestionManager.OnTimeEventQuestion += StartTimeEventQuestion;
        AnswerManager.AnswerSubmitted += OnTimeEventPassed;
    }

    private void OnDisable()
    {
        QuestionManager.OnTimeEventQuestion -= StartTimeEventQuestion;
        AnswerManager.AnswerSubmitted -= OnTimeEventPassed;
    }

    private void StartTimeEventQuestion(Question question)
    {
        timeLimit = question.TimeLimit;
        timer = timeLimit;
        TE_Text.text = timeLimit.ToString("0");
    }

    private void FixedUpdate()
    {
        timer -= 1 * Time.deltaTime;
        if (timer <= 0f)
        {
            OnTimeOver();
            TE_SpaceObject.SetActive(false);
        }
        TE_Text.text = timer.ToString("0");
       
    }

    private void OnTimeEventPassed(AnswerButton answerButton)
    {
        TE_SpaceObject.SetActive(false);
    }
}
