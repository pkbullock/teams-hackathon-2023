using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MockApiTeamsGraphCalls.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsRecieverController : ControllerBase
    {
        //Note: This is a read only API as the user will take actiiion on the notification in Teams

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Get the notification messages - Mocking Only
            var messages = Utility.NotificationMessages.GetMockNotificationMessages();

            return await Task.FromResult(Ok(messages));            
        }
    }
}
