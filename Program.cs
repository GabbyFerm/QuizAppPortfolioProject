using Spectre.Console;
using Figgle;
using System;
using System.Collections.Generic;
using System.IO;
using QuickPortfolioProject;

namespace QuickPortfolioProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string jsonFilePath = "questions.json";
            QuizApp quizApp = new QuizApp();
            List<Question> questions = quizApp.LoadQuestionsFromJson(jsonFilePath);

            if (questions == null || questions.Count == 0)
            {
                Console.WriteLine("No questions found.");
                return;
            }

            int correctAnswers = 0;
            int totalQuestions = questions.Count;

            string title = "Music Quiz";
            var figgleTitle = FiggleFonts.Doom.Render(title);
            AnsiConsole.Markup($"[bold magenta]{figgleTitle}[/]");

            foreach (var question in questions)
            {
                bool isCorrect = quizApp.AskQuestion(question);
                if (isCorrect)
                {
                    correctAnswers++;
                }
            }

            quizApp.DisplayResults(correctAnswers, totalQuestions);
        }
    }
}
