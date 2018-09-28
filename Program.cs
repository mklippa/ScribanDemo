using System;
using System.Collections.Generic;
using System.IO;
using Scriban;

namespace ScribanDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var template = Template.Parse(File.ReadAllText("input.html"));
            var result = template.Render(new { TestResult = ParseCsv("ItemPool.csv") });
            File.WriteAllText("output.html", result);
        }

        private static TestResult[] ParseCsv(string filePath)
        {
            // read from file
            var lines = File.ReadAllLines(filePath);
            var keys = lines[0].Split(',');
            var items = new List<Dictionary<string,string>>();
            for (int i = 1; i < lines.Length; i++)
            {
                var item = new Dictionary<string,string>();
                var values = lines[i].Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    item.Add(keys[j],values[j]);
                }
                items.Add(item);
            }

            // prepare data
            var result = new List<TestResult>();
            result.Add(new TestResult());
            result[0].Sections = new List<Section>();
            result[0].Sections.Add(new Section());
            result[0].Sections[0].Items = new List<ItemResponse>();

            foreach (var item in items)
            {
                var question = new Question();
                question.Metadatas = new List<QuestionMetadata>();
                
                question.Metadatas.Add(Tag(item, "Curriculum"));
                question.Metadatas.Add(Tag(item, "DifficultyScale1"));
                question.Metadatas.Add(Tag(item, "DifficultyScale1DI"));
                question.Metadatas.Add(Tag(item, "DifficultyScale1Guess"));
                question.Metadatas.Add(Tag(item, "DifficultyScale2"));
                question.Metadatas.Add(Tag(item, "DifficultyScale2DI"));
                question.Metadatas.Add(Tag(item, "DifficultyScale2Guess"));
                question.Metadatas.Add(Tag(item, "DifficultyScale3"));
                question.Metadatas.Add(Tag(item, "DifficultyScale3DI"));
                question.Metadatas.Add(Tag(item, "DifficultyScale3Guess"));

                result[0].Sections[0].Items.Add(new ItemResponse
                {
                    Question = question
                });
            }

            return result.ToArray();
        }

        private static QuestionMetadata Tag(Dictionary<string, string> item, string key)
        {
            return new QuestionMetadata
            {
                Name = key,
                Value = item[key]
            };
        }
    }

    class TestResult
    {
        public List<Section> Sections { get; set; }
    }

    class Section
    {
        public List<ItemResponse> Items { get; set; }
    }

    public class ItemResponse
    {
        public Question Question { get; set; }
    }

    public class Question
    {
        public List<QuestionMetadata> Metadatas { get; set; }
    }

    public class QuestionMetadata
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
