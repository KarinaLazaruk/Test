using Web.Managers;

namespace Web.Helpers
{
    public class ApiClient
    {
        public ProjectsManager Projects { get; private set; }
        public PullRequestsManager PullRequests { get; private set; }

        private readonly HttpCommunicationWorker _httpWorker;
        
        public ApiClient()
        {
            _httpWorker = new HttpCommunicationWorker("http://sh31.corteos.ru:7990/", "jenkins", "q123456w");
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            Projects = new ProjectsManager(_httpWorker);
            PullRequests = new PullRequestsManager(_httpWorker);
        }
    }
}