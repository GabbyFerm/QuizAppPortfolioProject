using Spectre.Console;
using Figgle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace QuickPortfolioProject
{
    public class QuizApp
    {
        public List<Question> LoadQuestionsFromJson(string path)
        {
            try
            {
                var jsonData = File.ReadAllText(path);
                var quizData = JsonSerializer.Deserialize<Dictionary<string, List<Question>>>(jsonData);

                if (quizData == null || !quizData.ContainsKey("questions") || quizData["questions"] == null)
                {
                    Console.WriteLine("Error: The 'questions' data is missing or invalid.");
                    return new List<Question>();
                }

                return quizData["questions"];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading questions: {ex.Message}");
                return new List<Question>();
            }
        }

        public bool AskQuestion(Question question)
        {
            if (question == null || question.Answers == null || question.Answers.Count == 0)
            {
                Console.WriteLine("Invalid question or answers.");
                return false;
            }

            AnsiConsole.MarkupLine($"[bold mediumpurple2]{question.QuestionText}[/]");

            var prompt = new SelectionPrompt<int>()
            .Title("[bold mediumpurple2]\nChoose an answer:[/]")
            .PageSize(4)
            .HighlightStyle(new Style(foreground: Color.DarkViolet));

            for (int i = 0; i < question.Answers.Count; i++)
            {
                prompt.AddChoice(i + 1); 
            }

            for (int i = 0; i < question.Answers.Count; i++)
            {
                AnsiConsole.MarkupLine($"[]{i + 1}.[/] {question.Answers[i]}");
            }

            var answer = AnsiConsole.Prompt(prompt);

            bool isCorrect = question.Answers[answer - 1] == question.CorrectAnswer;
            if (isCorrect)
            {
                AnsiConsole.MarkupLine("[bold mediumpurple2]Correct![/]\n");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Wrong![/]\n");
            }

            return isCorrect;
        }

        public void DisplayResults(int correctAnswers, int totalQuestions)
        {
            var score = (double)correctAnswers / totalQuestions * 100;
            var result = $"You answered {correctAnswers} out of {totalQuestions} questions correctly!";
            AnsiConsole.MarkupLine(result);
            AnsiConsole.MarkupLine($"Your score: [bold mediumpurple2]{score:0.00}%[/]");
        }
    }
}
