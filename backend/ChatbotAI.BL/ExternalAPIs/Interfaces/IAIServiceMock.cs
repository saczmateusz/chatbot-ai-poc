namespace ChatbotAI.BL.ExternalAPIs.Interfaces
{
    public interface IAIServiceMock
    {
        public Task<string> SimulateAIResponse(string message);
    }
}
