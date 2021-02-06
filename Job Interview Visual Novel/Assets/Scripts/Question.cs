using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : IQuestionWithTimeLimit //MonoBehaviour
{
    public readonly int Id;
    public readonly string Name;
    public readonly string Text;
    public readonly float BestRateValue;
    public readonly float OkRateValue;
    public readonly float BadRateValue;
    public readonly Answer[] AnswersArray;
    //public Answer[] AnswersArray { get; private set; }

    public bool NeedAnswer { get; }
    public Question(int id, string name, string text)
    {
        Id = id;
        Name = name;
        Text = text;
        NeedAnswer = false;
        AnswersArray = new Answer[] {Answer.JustNext};
    }

    //public Question(int id, string text, float bestRate, float okRate, float badRate)
    //{
    //    Id = id;
    //    Text = text;
    //    BestRateValue = bestRate;
    //    OkRateValue = okRate;
    //    BadRateValue = badRate;
    //    NeedAnswer = true;
    //}

    public Question(int id, string name, string text, float bestRate, float okRate, float badRate, Answer[] answersArray)
    {
        Id = id;
        Name = name;
        Text = text;
        BestRateValue = bestRate;
        OkRateValue = okRate;
        BadRateValue = badRate;
        NeedAnswer = true;
        AnswersArray = answersArray;
    }

    public float TimeLimit { get; } = 0;
    public bool HasTimeLimit() // true - time limit есть; false - не имеет time limit
    {
        return TimeLimit > 0;
    }

    //public Question(int id, string text, float bestRate, float okRate, float badRate, float timeLimit)
    //{
    //    Id = id;
    //    Text = text;
    //    BestRateValue = bestRate;
    //    OkRateValue = okRate;
    //    BadRateValue = badRate;
    //    TimeLimit = timeLimit;
    //    NeedAnswer = true;
    //}

    public Question(int id, string name, string text, float bestRate, float okRate, float badRate, float timeLimit, Answer[] answersArray)
    {
        Id = id;
        Name = name;
        Text = text;
        BestRateValue = bestRate;
        OkRateValue = okRate;
        BadRateValue = badRate;
        TimeLimit = timeLimit;
        NeedAnswer = true;
        AnswersArray = answersArray;
    }

    //public void SetOrCheckAnswers(Answer[] answers)
    //{
    //    if(AnswersArray.Length == 0)
    //    {
    //        AnswersArray = answers;
    //    }
    //    else
    //    {
    //        Debug.Log("Question #" + Id + "has answers already");
    //    }
    //}
}
