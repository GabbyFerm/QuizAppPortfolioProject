using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPortfolioProject
{
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> Answers { get; set; } = new List<string>();
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
