namespace Pushification.Services
{
    public class WorkerService
    {

        public async void Run()
        {
            while (true)
            {
                NotificationService notificationService = new NotificationService();
                await notificationService.Run();


                //SubscribeService subscribeService = new SubscribeService();
                //await subscribeService.Run();   
            }         
        }
    }

}
