using System;
using System.Threading;
using Project.Enums;
using Project.Helpers;
using Project.Services;

namespace Project.Base {

    abstract class InteractiveCommand : Command {

        // Prints the question and blocks until we're got an answer
        protected string AskQuestion(string questionText, QuestionValidator validator = null, string defaultValue = null) {
            Question question = new Question(questionText, validator, defaultValue);

            // Print question
            ConsoleHelper.Print(PrintType.Default, question.GetMessage());

            // Get answer and return
            return WaitForAnswer(question);
        }

        private string WaitForAnswer(Question question) {
            Program app = Program.GetInstance();
            ConsoleService consoleService = app.GetService<ConsoleService>("console");

            // Try to register the question
            if (!consoleService.SetQuestion(question)) {
                throw new Exception("Failed to register question");
            }

            string answer = question.GetAnswer();

            // Return default if answer is empty
            if (question.GetDefaultValue() != null && answer.Length == 0) {
                return question.GetDefaultValue();
            }

            // Validate value
            QuestionValidator validator = question.GetValidator();

            if (validator != null && !validator.Validate(answer)) {
                ConsoleHelper.Print(PrintType.Error, "Invalid answer: " + answer);
                question.SetAnswer(null);
                return WaitForAnswer(question);
            }

            // Return answer
            return answer;
        }

    }

}
