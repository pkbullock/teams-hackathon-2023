namespace MockApiTeamsGraphCalls.Models
{
    public class UserNotificationMessage
    {
        public string id { get; set; }
        public DateTime dateReceived { get; set; }
        public Topic topic { get; set; }
        public string activityType { get; set; }
        public Previewtext previewText { get; set; }
        public Templateparameter[] templateParameters { get; set; }
        public string importance { get; set; }
        public string locale { get; set; }
        public From from { get; set; }
        
    }

    public class Topic
    {
        public string source { get; set; }
        public string value { get; set; }
        public string webUrl { get; set; }
    }

    public class Previewtext
    {
        public string content { get; set; }
    }

    public class From
    {
        public object application { get; set; }
        public object device { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string userIdentityType { get; set; }
    }

    public class Templateparameter
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
