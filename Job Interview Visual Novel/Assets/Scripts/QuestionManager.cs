using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public static Question[] Questions;
    public static Question CurrentQuestion;

    [SerializeField] TMP_Text NameTextBox;
    [SerializeField] TMP_Text TextBox;
    [SerializeField] GameObject TimeEventBox;

    public delegate void QuestionDelegate(Question question);
    public static event QuestionDelegate QuestionChangedTo;
    public static event QuestionDelegate OnTimeEventQuestion;

    public delegate void ResponseAnswerDelegate(Question question, Answer answer);
    public static event ResponseAnswerDelegate OnGettingAnswer;

    public delegate void ResponseProgressDelegate();
    public static event ResponseProgressDelegate OnBackToPrevQuestion;
    public static event ResponseProgressDelegate OnQuestionsOver;

    private void OnEnable()
    {
        AnswerManager.AnswerSubmitted += AnswerSubmitted;
        TimeEventManager.OnTimeOver += OnTimeQuestionOvered;
        
    }

    private void OnDisable()
    {
        AnswerManager.AnswerSubmitted -= AnswerSubmitted;
        TimeEventManager.OnTimeOver -= OnTimeQuestionOvered;
    }

    void Start()
    {
        Questions = InitializeQuestions().ToArray();
        GameStateManager.OnGameStarted += OnGameStarted;
        
    }

    private void OnGameStarted()
    {
        ChangeCurrentQuestionToInitial();
    }


    private IEnumerable<Question> InitializeQuestions()
    {
        return QuestionStorage.GetQuestions();
    }

    private void SetCurrentQuestion(int id)
    {
        CurrentQuestion = Questions[id];
        NameTextBox.text = "#" + id + " " + CurrentQuestion.Name;
        TextBox.text = CurrentQuestion.Text;
        QuestionChangedTo(CurrentQuestion);
        if (CurrentQuestion.HasTimeLimit())
        {
            TimeEventBox.SetActive(true);
            OnTimeEventQuestion(CurrentQuestion);
        }
    }

    private void ChangeCurrentQuestionToNext()
    {
        var id = CurrentQuestion.Id + 1;
        if (id >= Questions.Length)
        {
            OnQuestionsOver();
            return;
        }
        SetCurrentQuestion(id);
    }

    private void ChangeCurrentQuestionToPrevious()
    {
        SetCurrentQuestion(CurrentQuestion.Id - 1);
    }

    private void ChangeCurrentQuestionToInitial()
    {
        SetCurrentQuestion(0);
    }

    private void AnswerSubmitted(AnswerButton answerButton)
    {
        OnGettingAnswer(CurrentQuestion, answerButton.Answer);
        //print(answerButton.ButtonLocalID + " = " + answerButton.Answer.Text); //Закоментировать(!)
        ChangeCurrentQuestionToNext();
    }

    private void OnTimeQuestionOvered()
    {
        OnGettingAnswer(CurrentQuestion, Answer.NoneAnswer);
        ChangeCurrentQuestionToNext();
    }

    public void OnBackButton()
    {
        if (CurrentQuestion.Id > 0)
        {
            if (CurrentQuestion.HasTimeLimit())
                TimeEventBox.SetActive(false);
            ChangeCurrentQuestionToPrevious();
            OnBackToPrevQuestion();
        }
    }
}
//var len = 3;
//string[] qTexts = new string[] {
//            "Добрый день, меня зовут Татьяна. Я - тимлид команды разработки.",
//            "Сейчас мы с вами проведем небольшое собеседование, чтобы понять уровень Ваших навыков",
//            "У вас есть опыт разработки на Unity?"
//        };
//Answer[] answersForN2 = new Answer[] {
//            new Answer(0, AnswerRate.Bad, "К сожалению, я еще не освоил Unity"),
//            new Answer(1, AnswerRate.Ok, "Я сделал несколько игр для учебных проектов"),
//            new Answer(3, AnswerRate.Best, "Да, я работал Unity разработчиком в нескольких компаниях")
//        };
//for (var i = 0; i < len; i++)
//{
//    if (i == 2)
//    {
//        list.Add(new Question(i, "question #" + i, qTexts[i], 10, 0, -10, 15f, answersForN2));
//    }
//    else
//    {
//        list.Add(new Question(i, "question #" + i, qTexts[i]));
//    }

//}