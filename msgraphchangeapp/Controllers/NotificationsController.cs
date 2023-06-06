using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using msgraphchangeapp.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Http.Headers;
using Azure.Identity;

namespace msgraphchangeapp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly MyConfig config;
        private static Dictionary<string, Subscription> Subscriptions = new Dictionary<string, Subscription>();
        private static Timer? subscriptionTimer = null;

        public NotificationsController(MyConfig config)
        {
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            // https://learn.microsoft.com/en-us/graph/api/subscription-post-subscriptions?view=graph-rest-1.0&tabs=csharp


            var graphServiceClient = GetGraphClient();

            var requestBody = new Subscription
            {
                ChangeType = "updated",
                NotificationUrl = config.Ngrok + "/api/notifications",
                Resource = $"/users/{config.TestUserUPN}/chats/getAllMessages",
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(5),
                ClientState = "SecretClientState",
                LatestSupportedTlsVersion = "v1_2"
            };

            //var sub = new Microsoft.Graph.Subscription();
            //sub.ChangeType = "updated";
            //sub.NotificationUrl = config.Ngrok + "/api/notifications";
            //sub.Resource = "/users";
            //sub.ExpirationDateTime = DateTime.UtcNow.AddMinutes(5);
            //sub.ClientState = "SecretClientState";

            //var newSubscription = await graphServiceClient
            //    .Subscriptions
            //    .Request()
            //    .AddAsync(sub);

            var newSubscription = await graphServiceClient.Subscriptions.PostAsync(requestBody);

            Subscriptions[newSubscription.Id] = newSubscription;

            if (subscriptionTimer == null)
            {
                subscriptionTimer = new Timer(CheckSubscriptions, null, 5000, 15000);
            }

            return $"Subscribed. Id: {newSubscription.Id}, Expiration: {newSubscription.ExpirationDateTime}";
        }

        public async Task<ActionResult<string>> Post([FromQuery] string? validationToken = null)
        {
            // handle validation
            if (!string.IsNullOrEmpty(validationToken))
            {
                Console.WriteLine($"Received Token: '{validationToken}'");
                return Ok(validationToken);
            }

            // handle notifications
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                string content = await reader.ReadToEndAsync();

                Console.WriteLine(content);

                var notifications = JsonSerializer.Deserialize<ChangeNotificationCollection>(content);

                if (notifications != null)
                {
                    foreach (var notification in notifications.Value)
                    {
                        Console.WriteLine($"Received notification: '{notification.Resource}', {notification.ResourceData.AdditionalData["id"]}");
                    }
                }
            }

            return Ok();
        }

        private GraphServiceClient GetGraphClient()
        {

            //https://learn.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS

            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

             var clientSecretCredential = new ClientSecretCredential(
                config.TenantId,
                config.AppId,
                config.AppSecret, 
                options);


            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            return graphClient;
        }

        //private async Task<string> GetAccessToken()
        //{
        //    IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(config.AppId)
        //        .WithClientSecret(config.AppSecret)
        //        .WithAuthority($"https://login.microsoftonline.com/{config.TenantId}")
        //        .WithRedirectUri("https://daemon")
        //        .Build();

        //    string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

        //    var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();

        //    return result.AccessToken;
        //}


        private void CheckSubscriptions(Object? stateInfo)
        {
            AutoResetEvent? autoEvent = stateInfo as AutoResetEvent;

            Console.WriteLine($"Checking subscriptions {DateTime.Now.ToString("h:mm:ss.fff")}");
            Console.WriteLine($"Current subscription count {Subscriptions.Count()}");

            foreach (var subscription in Subscriptions)
            {
                // if the subscription expires in the next 2 min, renew it
                if (subscription.Value.ExpirationDateTime < DateTime.UtcNow.AddMinutes(2))
                {
                    RenewSubscription(subscription.Value);
                }
            }
        }

        private async void RenewSubscription(Subscription subscription)
        {
            Console.WriteLine($"Current subscription: {subscription.Id}, Expiration: {subscription.ExpirationDateTime}");

            var graphServiceClient = GetGraphClient();

            var newSubscription = new Subscription
            {
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(5)
            };

            // TEMP

            //await graphServiceClient
            //  .Subscriptions[subscription.Id]
            //  .Request()
            //  .UpdateAsync(newSubscription);

            //subscription.ExpirationDateTime = newSubscription.ExpirationDateTime;
            //Console.WriteLine($"Renewed subscription: {subscription.Id}, New Expiration: {subscription.ExpirationDateTime}");

            Console.WriteLine("Called but not used");
        }

    }

}
