namespace Pushification.Services
{
    public class WorkerService
    {

        public async void Run()
        {
            
                NotificationService notificationService = new NotificationService();
                await notificationService.Run();

                //SubscribeService subscribeService = new SubscribeService();
                //await subscribeService.Run();   
                  
        }
    }

}
