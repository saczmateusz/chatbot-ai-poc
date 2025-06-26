using ChatbotAI.BL.ExternalAPIs.Interfaces;

namespace ChatbotAI.BL.ExternalAPIs
{
    public class AIServiceMock : IAIServiceMock
    {
        public async Task<string> SimulateAIResponse(string message)
        {
            // Simulate AI processing time
            await Task.Delay(50);

            var loremWords = new[]
    {
        "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit",
        "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore",
        "magna", "aliqua", "enim", "ad", "minim", "veniam", "quis", "nostrud",
        "exercitation", "ullamco", "laboris", "nisi", "aliquip", "ex", "ea", "commodo",
        "consequat", "duis", "aute", "irure", "in", "reprehenderit", "voluptate",
        "velit", "esse", "cillum", "fugiat", "nulla", "pariatur", "excepteur", "sint",
        "occaecat", "cupidatat", "non", "proident", "sunt", "culpa", "qui", "officia",
        "deserunt", "mollit", "anim", "id", "est", "laborum"
    };

            var random = new Random();
            var wordCount = random.Next(10, 300); // Random length between 20-300 words
            var selectedWords = new List<string>();

            for (int i = 0; i < wordCount; i++)
            {
                selectedWords.Add(loremWords[random.Next(loremWords.Length)]);
            }

            return string.Join(" ", selectedWords) + ".";
        }
    }
}
