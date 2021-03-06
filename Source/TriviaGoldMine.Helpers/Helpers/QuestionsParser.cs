﻿namespace TriviaGoldMine.Helpers.Helpers
{
    using System.Collections.Generic;
    using System.IO;

    using Excel;

    using Models;

    public static class QuestionsParser
    {
        public static List<Question> GetQuestions(string path)
        {
            var questions = new List<Question>();
            var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dataSet = excelReader.AsDataSet();
            var table = dataSet.Tables[0];

            if (table.Rows[0][2].ToString() == "3rd Answer")
            {
                for (var i = 1; i < 22; i++)
                {
                    var row = table.Rows[i];

                    var number = row[0].ToString();
                    var mainQuestion = row[1].ToString();
                    var answer = $"3rd: {row[2]}, 2nd: {row[3]}, 1st: {row[4]}";
                    var question = new Question(number, "", "", mainQuestion, answer, "");
                    questions.Add(question);
                }
            }
            else
            {
                for (var i = 1; i <= 28; i++)
                {
                    var row = table.Rows[i];
                    var number = row[0].ToString();
                    var points = row[1].ToString();
                    var mainQuestion = row[2].ToString();
                    var answer = row[3].ToString();
                    var category = row[5].ToString();
                    var alternateQuestion = row[6].ToString();
                    var question = new Question(number, points, category, mainQuestion, answer, alternateQuestion);
                    questions.Add(question);
                }
            }

            excelReader.Close();

            return questions;
        }

        public static SpreadsheetParseResult ParseSpreadsheet(string path)
        {
            var result = new SpreadsheetParseResult();
            var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var dataSet = excelReader.AsDataSet();
            var table = dataSet.Tables[0];

            if (table.Rows[0][2].ToString() == "3rd Answer")
            {
                for (var i = 1; i < 22; i++)
                {
                    var row = table.Rows[i];

                    result.Questions.Add(row[1].ToString());
                    result.Answers.Add(row[2].ToString());
                    result.Answers.Add(row[3].ToString());
                    result.Answers.Add(row[4].ToString());
                }
            }
            else
            {
                for (var i = 1; i <= 28; i++)
                {
                    var row = table.Rows[i];

                    result.Questions.Add(row[2].ToString());
                    var answer = row[3].ToString();
                    if (answer.Contains("|"))
                    {
                        var answers = answer.Split('|');
                        result.Answers.AddRange(answers);
                    }
                    else
                    {
                        result.Answers.Add(answer);
                    }

                    result.Categories.Add(row[5].ToString());
                }
            }

            excelReader.Close();

            return result;
        }
    }
}