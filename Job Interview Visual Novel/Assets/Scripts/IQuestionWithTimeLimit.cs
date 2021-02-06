using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestionWithTimeLimit
{
    float TimeLimit { get; }
    bool HasTimeLimit();
}
