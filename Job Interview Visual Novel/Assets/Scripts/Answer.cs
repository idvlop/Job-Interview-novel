using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer //: MonoBehaviour
{
    public readonly int Id;
    public readonly string Text;
    public readonly AnswerRate Rate;

    public Answer(int id, AnswerRate rate, string text)
    {
        Id = id;
        Text = text;
        Rate = rate;
    }

    public static Answer JustNext { get; } = new Answer(0, AnswerRate.NotNeedRate, "Далее");
    public static Answer NoneAnswer { get; } = new Answer(-1, AnswerRate.Bad, "");
}

public enum AnswerRate
{
    NotNeedRate,
    Bad,
    Ok,
    Best
};