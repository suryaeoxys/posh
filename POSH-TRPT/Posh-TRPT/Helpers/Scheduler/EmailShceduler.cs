//using Quartz;

//namespace Posh_TRPT.Helpers.Scheduler
//{
//    public class EmailShceduler : IJob
//    {
//        public Task Execute(IJobExecutionContext context)
//        {
//            // Code to hit the REST API
//            // Example using HttpClient
//            using (var client = new HttpClient())
//            {
//                var response = client.GetAsync("http://example.com/api").Result;
//                var data = response.Content.ReadAsStringAsync().Result;
//                // Process the response data
//            }
//            return Task.CompletedTask;
//        }
//    }
//}
