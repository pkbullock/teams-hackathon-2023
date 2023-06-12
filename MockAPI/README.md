# Mock API

This API will simluate an API that returns activity data for a user in Microsoft Teams.

## Mocking Plan

There is a API call to send notifications but not recieve. If we use what is sent and recieved by the end user, we can infer a GET API call to get the notifications for the user.

Why? 

To enable development of the Teams Activity Feed as if it were available, build the UI and use the Teams Toolkit.
Building in small learning of latest ASP.NET C# features for secure APIs as well. Then get the Teams Toolkit to call the secure API. 

### Example CALL TO Send

POST CALL: /users/{userId | user-principal-name}/teamwork/sendActivityNotification

```json

POST https://graph.microsoft.com/v1.0/users/{userId}/teamwork/sendActivityNotification
Content-Type: application/json

{
    "topic": {
        "source": "text",
        "value": "Deployment Approvals Channel",
        "webUrl": "https://teams.microsoft.com/l/message/19:448cfd2ac2a7490a9084a9ed14cttr78c@thread.skype/1605223780000?tenantId=c8b1bf45-3834-4ecf-971a-b4c755ee677d&groupId=d4c2a937-f097-435a-bc91-5c1683ca7245&parentMessageId=1605223771864&teamName=Approvals&channelName=Azure%20DevOps&createdTime=1605223780000"
    },
    "activityType": "deploymentApprovalRequired",
    "previewText": {
        "content": "New deployment requires your approval"
    },
    "templateParameters": [
        {
            "name": "deploymentId",
            "value": "6788662"
        }
    ]
}

```

```csharp
// Code snippets are only available for the latest version. Current version is 5.x
var graphClient = new GraphServiceClient(requestAdapter);

var requestBody = new Microsoft.Graph.Users.Item.Teamwork.SendActivityNotification.SendActivityNotificationPostRequestBody
{
	Topic = new TeamworkActivityTopic
	{
		Source = TeamworkActivityTopicSource.Text,
		Value = "Deployment Approvals Channel",
		WebUrl = "https://teams.microsoft.com/l/message/19:448cfd2ac2a7490a9084a9ed14cttr78c@thread.skype/1605223780000?tenantId=c8b1bf45-3834-4ecf-971a-b4c755ee677d&groupId=d4c2a937-f097-435a-bc91-5c1683ca7245&parentMessageId=1605223771864&teamName=Approvals&channelName=Azure%20DevOps&createdTime=1605223780000",
	},
	ActivityType = "deploymentApprovalRequired",
	PreviewText = new ItemBody
	{
		Content = "New deployment requires your approval",
	},
	TemplateParameters = new List<KeyValuePair>
	{
		new KeyValuePair
		{
			Name = "deploymentId",
			Value = "6788662",
		},
	},
};
await graphClient.Users["{user-id}"].Teamwork.SendActivityNotification.PostAsync(requestBody);

```



## References

- https://learn.microsoft.com/en-us/graph/api/userteamwork-sendactivitynotification?view=graph-rest-1.0&tabs=http




