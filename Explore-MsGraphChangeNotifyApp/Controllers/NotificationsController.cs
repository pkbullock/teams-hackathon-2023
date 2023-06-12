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
using Microsoft.Kiota.Abstractions;

namespace msgraphchangeapp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly MyConfig config;
        private static Dictionary<string, Subscription> Subscriptions = new Dictionary<string, Subscription>();
        private static Timer? subscriptionTimer = null;
        private static object? DeltaLink = null;
        //private static IUserMessagesDeltaCollectionPage? lastPage = null;

        public NotificationsController(MyConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Notification Entry Point
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            // https://learn.microsoft.com/en-us/graph/api/subscription-post-subscriptions?view=graph-rest-1.0&tabs=csharp

            var graphServiceClient = GetGraphClient();

            // Including the resource data will cause the subscription to fail without a valid certificate
            // IncludeResourceData = false
            var requestBody = new Subscription
            {
                ChangeType = "updated",
                NotificationUrl = config.Ngrok + "/api/notifications",
                Resource = $"/users/{config.TestUserUPN}/chats/getAllMessages",
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(5),
                ClientState = "SecretClientState",
                LatestSupportedTlsVersion = "v1_2"
            };

            try
            {
                var newSubscription = await graphServiceClient.Subscriptions.PostAsync(requestBody);

                Subscriptions[newSubscription.Id] = newSubscription;

                if (subscriptionTimer == null)
                {
                    subscriptionTimer = new Timer(CheckSubscriptions, null, 5000, 15000);
                }

                return $"Subscribed. Id: {newSubscription.Id}, Expiration: {newSubscription.ExpirationDateTime}";
            }
            catch(Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        /// <summary>
        /// Receiving notifications from the Graph
        /// </summary>
        /// <param name="validationToken"></param>
        /// <returns></returns>
        public async Task<ActionResult<string>> Post([FromQuery] string? validationToken = null)
        {
            //https://learn.microsoft.com/en-us/training/modules/msgraph-changenotifications-trackchanges/7-exercise-track-changes


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

            // use deltaquery to query for all updates
            await CheckForUpdates();

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

        /// <summary>
        /// Renews the subscription by extending the expiration date by 5 minutes.
        /// </summary>
        /// <param name="subscription"></param>
        private async void RenewSubscription(Subscription subscription)
        {
            Console.WriteLine($"Current subscription: {subscription.Id}, Expiration: {subscription.ExpirationDateTime}");

            var graphServiceClient = GetGraphClient();

            var newSubscriptionExpiryBody = new Subscription
            {
                ExpirationDateTime = DateTime.UtcNow.AddMinutes(5)
            };

            var updatedSubscription = await graphServiceClient.Subscriptions[subscription.Id].PatchAsync(newSubscriptionExpiryBody);

            subscription.ExpirationDateTime = updatedSubscription.ExpirationDateTime;
            Console.WriteLine($"Renewed subscription: {subscription.Id}, New Expiration: {subscription.ExpirationDateTime}");
        }

        private async Task CheckForUpdates()
        {
            var graphClient = GetGraphClient();

            // get a page of users
            var userMessages = await GetUserMessages(graphClient, DeltaLink);

            OutputUsers(userMessages);

            //// go through all of the pages so that we can get the delta link on the last page.
            //while (userMessages.NextPageRequest != null)
            //{
            //    userMessages = userMessages.NextPageRequest.GetAsync().Result;
            //    OutputUsers(userMessages);
            //}

            //object? deltaLink;

            //if (userMessages.AdditionalData.TryGetValue("@odata.deltaLink", out deltaLink))
            //{
            //    DeltaLink = deltaLink;
            //}
        }

        private void OutputUsers(List<ChatMessage> messages)
        {
            //https://learn.microsoft.com/en-us/graph/api/chats-getallmessages?view=graph-rest-1.0&tabs=http

            foreach (var message in messages)
            {
                var resultMessage = $"From User: {message.Id}, {message.Subject} {message.Summary} {message.WebUrl}";
                Console.WriteLine(resultMessage);
            }
        }

        private async Task<List<ChatMessage>> GetUserMessages(GraphServiceClient graphClient, object? deltaLink)
        {

            var result = await graphClient.Users[config.TestUserID].Chats.GetAllMessages.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Top = 10;
            });

            List<ChatMessage> messages = result?.Value?.ToList() ?? new List<ChatMessage>();

            //IUserDeltaCollectionPage page;

            //if (lastPage == null || deltaLink == null)
            //{
            //    page = await graphClient.Delta()
            //                            .Request()
            //                            .GetAsync();
            //}
            //else
            //{
            //    lastPage.InitializeNextPageRequest(graphClient, deltaLink.ToString());
            //    page = await lastPage.NextPageRequest.GetAsync();
            //}

            //lastPage = page;
            return messages;
        }


        #region Code Graveyard

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


        #endregion

    }

}
