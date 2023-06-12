# Mock API

This API will simluate an API that returns activity data for a user in Microsoft Teams.

## Mocking Plan

There is a API call to send notifications but not recieve. If we use what is sent and recieved by the end user, we can infer a GET API call to get the notifications for the user.

Why? 

To enable development of the Teams Activity Feed as if it were available, build the UI and use the Teams Toolkit.
Building in small learning of latest ASP.NET C# features for secure APIs as well. Then get the Teams Toolkit to call the secure API. 

## API Calls - Fake the Get Operation

### Send Notification Example

Lets use this as a example of what notification data is sent to the user based on https://learn.microsoft.com/en-us/graph/api/userteamwork-sendactivitynotification?view=graph-rest-1.0&tabs=http#request-1

With modifications to estimate an example of what the Get operation would return.

```json

{
    "id": "1621973534864",
    "dateReceived": "2021-05-25T20:12:14.864Z",
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
    ],
    "importance": "normal",
    "locale": "en-us",
    "from": {
        "application": null,
        "device": null,
        "user": {
            "id": "0b4f1cf6-54c8-4820-bbb7-2a1f4257ade5",
            "displayName": "user1 a",
            "userIdentityType": "aadUser"
        }
    }
        
}

```

### How Many Examples?

Team A - 3 Users mentioning YOU. 7 Replies to the conversation thread.
Team B - 2 Users mentioning YOU. 1 Reply to the conversation thread.
Team C - 1 User mentioning YOU. 1 Reply to the conversation thread.



### Chat Message

Lets assume the output with be the same as a chat message, to fake the GET operation for the Teams App.

METHOD: Get
Content-Type: application/json

```json

{
    "@odata.context": "https://graph.microsoft.com/v1.0/$metadata#Collection(chatMessage)",
    "@odata.count": 2,
    "@odata.nextLink": "https://graph.microsoft.com/v1.0/users('0b4f1cf6-54c8-4820-bbb7-2a1f4257ade5')/chats/getallMessages?$top=2&$skiptoken=U2tpcFZhbHVlPTIjTWFpbGJveEZvbGRlcj1NYWlsRm9sZGVycy9UZWFtc01lc3NhZ2VzRGF0YQ%3d%3d",
    "value": [
        {
            "@odata.type": "#microsoft.graph.chatMessage",
            "id": "1621973534864",
            "replyToId": null,
            "etag": "1621973534864",
            "messageType": "message",
            "createdDateTime": "2021-05-25T20:12:14.864Z",
            "lastModifiedDateTime": "2021-05-25T20:12:14.864Z",
            "lastEditedDateTime": null,
            "deletedDateTime": null,
            "subject": null,
            "summary": null,
            "chatId": "19:3c9e92a344704332bbf5bda58f4d37b1@thread.v2",
            "importance": "normal",
            "locale": "en-us",
            "webUrl": null,
            "channelIdentity": null,
            "policyViolation": null,
            "eventDetail": null,
            "from": {
                "application": null,
                "device": null,
                "user": {
                    "id": "0b4f1cf6-54c8-4820-bbb7-2a1f4257ade5",
                    "displayName": "user1 a",
                    "userIdentityType": "aadUser"
                }
            },
            "body": {
                "contentType": "text",
                "content": "Hello user2, user 3"
            },
            "attachments": [],
            "mentions": [],
            "reactions": []
        }
    ]
}

```


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


## Creation

dotnet new webapi -o MockApiTeamsGraphCalls
dotnet add package Microsoft.Identity.Client
dotnet add package Microsoft.Graph
dotnet dev-certs https --trust
dotnet run


## References

- https://learn.microsoft.com/en-us/graph/api/userteamwork-sendactivitynotification?view=graph-rest-1.0&tabs=http
- https://learn.microsoft.com/en-us/graph/api/chats-getallmessages?view=graph-rest-1.0&tabs=http 



