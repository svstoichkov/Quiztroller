namespace Quizbuilder.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Web;

    using Models;

    public static class PptxEditor
    {
        public static string Edit(SpreadsheetParseResult spreadsheet, List<string> round1Image, List<string> round2Images, string pptxPath)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "Quiztroller");
            var newFile = Path.Combine(tempFolder, Path.GetFileName(pptxPath));

            if (Directory.Exists(tempFolder))
            {
                foreach (var file in Directory.GetFiles(tempFolder))
                {
                    File.Delete(file);
                }
            }
            else
            {
                Directory.CreateDirectory(tempFolder);
            }

            File.Copy(pptxPath, newFile);

            using (var archive = ZipFile.Open(newFile, ZipArchiveMode.Update))
            {
                //ReplaceMedia(img1Path, img2Path, img3Path, img4Path, archive);
                ReplaceQuestions(spreadsheet, archive, tempFolder);
            }

            return newFile;
        }

        private static void ReplaceQuestions(SpreadsheetParseResult spreadsheet, ZipArchive archive, string tempFolder)
        {
            var slides = archive.Entries.Where(x => x.FullName.StartsWith("ppt/slides/") && x.FullName.EndsWith(".xml"));
            slides = slides.OrderBy(x => int.Parse(x.FullName.Substring(x.FullName.LastIndexOf('e') + 1, x.FullName.IndexOf('.') - x.FullName.LastIndexOf('e') - 1))).ToList();
            
            foreach (var slide in slides)
            {
                var slidePath = Path.Combine(tempFolder, Path.GetFileName(slide.FullName));
                slide.ExtractToFile(slidePath);

                var content = File.ReadAllText(slidePath);

                while (content.Contains("QSTN_"))
                {
                    var qstnIndex = content.IndexOf("QSTN_") + 5;
                    if (qstnIndex > -1)
                    {
                        var questionNumber = int.Parse(content.Substring(qstnIndex, content.IndexOf("<", qstnIndex) - qstnIndex));
                        content = content.Replace($"QSTN_{questionNumber}", HttpUtility.HtmlEncode(spreadsheet.Questions[questionNumber - 1]));
                    }
                }

                while (content.Contains("CAT_"))
                {
                    var catIndex = content.IndexOf("CAT_") + 4;
                    if (catIndex > -1)
                    {
                        var categoryNumber = int.Parse(content.Substring(catIndex, content.IndexOf("<", catIndex) - catIndex));
                        content = content.Replace($"CAT_{categoryNumber}", HttpUtility.HtmlEncode(spreadsheet.Categories[categoryNumber - 1]));
                    }
                }

                while (content.Contains("ANSWR_"))
                {
                    var catIndex = content.IndexOf("ANSWR_") + 6;
                    if (catIndex > -1)
                    {
                        var categoryNumber = int.Parse(content.Substring(catIndex, content.IndexOf("<", catIndex) - catIndex));
                        content = content.Replace($"ANSWR_{categoryNumber}", HttpUtility.HtmlEncode(spreadsheet.Answers[categoryNumber - 1]));
                    }
                }


                //content = content.Replace("%PT%", HttpUtility.HtmlEncode(questions[currentQuestionIndex].Points.ToString()));
                //content = content.Replace("%ANSWR%", HttpUtility.HtmlEncode(questions[currentQuestionIndex].Answer ?? " "));
                //
                //if (slide.FullName.Contains("27"))
                //{
                //    var answers = questions[currentQuestionIndex].Answer.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                //    for (var i = 0; i < answers.Length; i++)
                //    {
                //        content = content.Replace($"%ANSWR{i + 1}%", HttpUtility.HtmlEncode(answers[i]));
                //    }
                //    for (var i = 1; i <= 10; i++)
                //    {
                //        content = content.Replace($"%ANSWR{i}%", " ");
                //    }
                //}
                //else if (slide.FullName.Contains("28"))
                //{
                //    for (var i = 24; i < 29; i++)
                //    {
                //        content = content.Replace($"%QSTN{i - 23}%", HttpUtility.HtmlEncode(questions[i].MainQuestion ?? " "));
                //    }
                //
                //    currentQuestionIndex = 28;
                //}
                //
                File.WriteAllText(slidePath, content);
                
                slide.Delete();
                archive.CreateEntryFromFile(slidePath, slide.FullName);
            }
        }

        private static void ReplaceAnswer()
        {
            
        }

        private static void ReplaceMedia(string img1Path, string img2Path, string img3Path, string img4Path, ZipArchive archive)
        {
            var img1 = archive.Entries.FirstOrDefault(x => x.Length == 2585);
            var img2 = archive.Entries.FirstOrDefault(x => x.Length == 2583);
            var img3 = archive.Entries.FirstOrDefault(x => x.Length == 2930);
            var img4 = archive.Entries.FirstOrDefault(x => x.Length == 10242);

            img1.Delete();
            img2.Delete();
            img3.Delete();
            img4.Delete();

            archive.CreateEntryFromFile(img1Path, img1.FullName);
            archive.CreateEntryFromFile(img2Path, img2.FullName);
            archive.CreateEntryFromFile(img3Path, img3.FullName);
            archive.CreateEntryFromFile(img4Path, img4.FullName);
        }
    }
}