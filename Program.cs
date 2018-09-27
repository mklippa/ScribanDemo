using System;
using System.Collections.Generic;
using System.IO;
using Scriban;

namespace ScribanDemo
{
    class Program
    {
        private static readonly TestResult TestResult = new TestResult
        {
            Sections = new[]
            {
                new Section
                {
                    Items = new []
                    {
                        new ItemResponse
                        {
                            Question = new Question
                            {
                                Metadatas = new []
                                {
                                    new QuestionMetadata
                                    {
                                        Name = "Foo",
                                        Value = "Bar"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        static void Main(string[] args)
        {
            var template = Template.Parse(File.ReadAllText("input.html"));
            var result = template.Render(new { TestResult = TestResult.ToString() });
            File.WriteAllText("output.html", result);
        }
    }

    class TestResult
    {
        public IEnumerable<Section> Sections { get; set; }
    }

    class Section
    {
        public IEnumerable<ItemResponse> Items { get; set; }
    }

    public class ItemResponse
    {
        public Question Question { get; set; }
    }

    public class Question
    {
        public IEnumerable<QuestionMetadata> Metadatas { get; set; }
    }

    public class QuestionMetadata
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
