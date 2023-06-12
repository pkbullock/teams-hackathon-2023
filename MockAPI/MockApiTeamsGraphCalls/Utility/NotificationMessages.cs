using System.Xml.Serialization;
using MockApiTeamsGraphCalls.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockApiTeamsGraphCalls.Utility
{
    /// <summary>
    /// Generates mock notification messages
    /// </summary>
    public class NotificationMessages
    {

        // Read a set of JSON files from disk and return them as a list of strongly typed objects
        public static List<UserNotificationMessage> GetMockNotificationMessages()
        {
            var notificationMessages = new List<UserNotificationMessage>();
            var files = Directory.GetFiles("Samples", "*.json");
            foreach (var file in files)
            {
                // Read the JSON file and deserialize it into a strongly typed object
                var jsonFile = File.ReadAllText(file);

                var notificationMessage = JsonSerializer.Deserialize<List<UserNotificationMessage>>(jsonFile, new JsonSerializerOptions()
                {
                    
                });

                if(notificationMessage != null)
                {
                    notificationMessages.AddRange(notificationMessage);
                }
            }
            return notificationMessages;
        }

    }
}
