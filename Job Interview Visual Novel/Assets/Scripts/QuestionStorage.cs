using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestionStorage : MonoBehaviour
{
    public static float DefaultBadRate { get; } = -10;
    public static float DefaultOkRate { get; } = 0;
    public static float DefaultBestRate { get; } = 10;

    public static AnswerRate GetAnswerRateByIndex(int i)
    {
        return i == 1 ? AnswerRate.Bad : i == 2 ? AnswerRate.Ok : i == 3 ? AnswerRate.Best : AnswerRate.NotNeedRate;
    }

    public static List<Question> GetQuestions() //todo возможно стоит сделать чтение из файла
    {
        var list = new List<Question>();
        var fullText = "0;Вступление;Добрый день! Меня зовут Татьяна, я - тимлид команды разработки. Сейчас мы проведем небольшое собеседование, чтобы понять уровень ваших навыков.;0\n1;Вступление;По итогам собеседования мы обсудим возможность Вашего дальнейшего трудоустройства к нам в компанию. Давайте начинать.;0\n2;Опыт с C#;У вас есть опыт разработки на C#?;1\tДа, я применял C# реальных проектах.^3;Я проходил онлайн-курсы по C# и разрабатывал на нем в учебных проектах^2;Пока нет, зато я неплохо программирую на Python!^1\n3;Опыт с Unity;Вы уже работали на Unity?;1\tДа, я разрабатывал несколько игр для учебных проектов и парочку для себя^3;Совсем небольшой, я только учусь, но разрабатывал на Construct 2^2;Я видел серию про Юнити, но мне больше понравился последний сезон, хотя концовку я так и не понял^1\n4;Отношение к делу;Так, хорошо... Скажите, Вы сталкивались с проблемой угасания энтузиазма в ходе долгой работы над одним проектом?;1;20\tВы что, конечно нет!^1;Такое случается иногда случается, но стараюсь держаться до конца^2;Бывает, но меня воодушевляет то, что я делаю, поэтому всегда довожу работу до конца^3\n5;Чужой код;Вам приходилось разбираться в чужом коде?;1\tИ разбираться, и вносить изменения часто приходится, когда работаю над проектом в команде^3;Если долго покапаюсь, возможно разберусь^2\n6;Опыт с Git;У Вас есть опыт использования систем контроля версий?;1\tДа, использовал Git и Subversion^3;Немного знаком с Git, при работе на Unity я обычно использую Unity Collaborate^2;К сожалению, нет, но могу быстро освоить^1\n7;Отношение к обучению;Для работы в нашей компании, вам придется многое освоить. Вы любите учиться?;1;20\tЯ очень люблю разрабатывать, и хочу уметь больше, так что готов впитывать как губка^3;Не очень, но если нужно, то окей^1;Конечно, я не против научиться новому^3\n8;Передышка;Хорошо, у нас дружелюбный коллектив, так что обучение не будет напряженным. Вы еще не устали?;1\tНет, все в порядке^3;Может самую малость, можем продолжать^3\n9;Win и Android;Осталось всего несколько вопросов. Вы имеете опыт создания приложений на Unity под Windows и Android?;1;15\tРазрабатывал игры как на Windows, так и на Android^3;Только под Android^3;Нет, пока работал только с HTML5^2\n10;Адаптивный UI;Работали с настройкой адаптивного UI?;1;5\tДа^3;К сожалению, нет^2\n11;English;Есть ли у Вас проблемы при чтении технической литературы на английском языке?;1\tНет, это необходимый навык для работы с Unity^3;Нет, таких проблем не возникает, тем более есть гугл переводчик^3;Иногда бывают...^2\n12;О компании;Мы почти закончили. Вас не смутило, что перед вами только голова с плечами?;1;10\tО чем вы?^3;Это немного странно, но я уже привык^3;Я готов смириться с этим, если вы меня возьмете в компанию^3";
        var a = fullText.Split('\n')//==lines array
            .Select(x => x.Split('\t'));//==questions and answers together arrays
        var quests = a.SelectMany(x => x.Where((y, i) => i % 2 == 0)).ToArray();//Массив вопросов в виде строки
        var answ = a.SelectMany(x => x.Where((y, i) => i % 2 == 1)).ToArray();//Массив слипшихся ответов к каждому вопросу 
        var answIndex = 0;
        for (var i = 0; i < quests.Count(); i++)
        {
            var curQuest = quests[i].Split(';');
            if (curQuest.Length == 4)
            {
                if (curQuest[curQuest.Length - 1] == "0")
                {
                    list.Add(new Question(int.Parse(curQuest[0]), curQuest[1], curQuest[2]));
                }
                else if (curQuest[curQuest.Length - 1] == "1")
                {
                    var curAnswers = answ[answIndex].Split(';');
                    answIndex++;
                    var answersArray = new Answer[curAnswers.Length];
                    for (var j = 0; j < curAnswers.Length; j++)
                    {
                        var answerAndRate = curAnswers[j].Split('^');
                        answersArray[j] = new Answer(j, GetAnswerRateByIndex(int.Parse(answerAndRate[1])), answerAndRate[0]);
                    }
                    list.Add(new Question(int.Parse(curQuest[0]), curQuest[1], curQuest[2], DefaultBestRate, DefaultOkRate, DefaultBadRate, answersArray));
                }
            }
            else if (curQuest.Length == 5)
            {
                var curAnswers = answ[answIndex].Split(';');
                answIndex++;
                var answersArray = new Answer[curAnswers.Length];
                for (var j = 0; j < curAnswers.Length; j++)
                {
                    var answerAndRate = curAnswers[j].Split('^');
                    answersArray[j] = new Answer(j, GetAnswerRateByIndex(int.Parse(answerAndRate[1])), answerAndRate[0]);
                }
                list.Add(new Question(int.Parse(curQuest[0]), curQuest[1], curQuest[2], DefaultBestRate, DefaultOkRate, DefaultBadRate, int.Parse(curQuest[curQuest.Length - 1]), answersArray));
            }
        }
        return list;
    }
}
