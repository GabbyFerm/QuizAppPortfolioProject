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

            ClearAndDisplayTitle("Music Quiz");

            AnsiConsole.Status()
            .Spinner(Spinner.Known.Arc)
            .SpinnerStyle(Style.Parse("magenta"))
            .Start("[bold magenta]Loading question...[/]", ctx =>
            {
                Thread.Sleep(1000); 
            });

            AnsiConsole.MarkupLine($"[bold plum3]{question.QuestionText}[/]");

            var prompt = new SelectionPrompt<string>()
            .Title("[bold mediumpurple2]\nChoose an answer (use arrow keys):[/]")
            .PageSize(4)  
            .HighlightStyle(new Style(foreground: Color.MediumPurple2))
            .AddChoices(question.Answers); 

            var selectedAnswer = AnsiConsole.Prompt(prompt);

            bool isCorrect = selectedAnswer == question.CorrectAnswer;
            if (isCorrect)
            {
                AnsiConsole.MarkupLine("[bold mediumpurple2]Correct![/]\n");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Wrong![/]\n");
            }

            Thread.Sleep(2000);
            return isCorrect;
        }

        public void DisplayResults(int correctAnswers, int totalQuestions)
        {
            var score = (double)correctAnswers / totalQuestions * 100;
            AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("magenta"))
            .Start("[bold magenta]Calculating your score...[/]", ctx =>
            {
            
                Thread.Sleep(2000);
            });

            var table = new Table();
            table.Border = TableBorder.Rounded; 
            table.BorderStyle = Style.Parse("magenta");
            table.AddColumn("[bold mediumpurple2]Quiz Results[/]");
            table.AddRow($"You answered [bold mediumpurple2]{correctAnswers}[/] out of [bold mediumpurple2]{totalQuestions}[/] questions correctly!");
            table.AddRow($"Your score: [bold mediumpurple2]{score:0.00}%[/]");

            AnsiConsole.Write(table);
        }
        public void ClearAndDisplayTitle(string title)
        {
            AnsiConsole.Clear();
            var figgleTitle = FiggleFonts.Doom.Render(title);
            AnsiConsole.Markup($"[bold magenta]{figgleTitle}[/]");
        }
    }
}
