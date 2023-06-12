# teams-hackathon-2023

The purpose of this hack is to see if we can read the message notifications of the user from their activity feed or multiple chats, organise them into a friendlier more manageable view. 

## Idea 1

### What is the thinking behind this hack?

The productivity booster idea is to read the notifications and allow users to see which messages could be actions, questions or tasks, to see all the message contents in a single view from multiple chats and channels, then perform actions on them and organise them, identify the noise from the relevant messages. 

This scenario fits a problem with information overload in Teams where potentially loads of messages are recieved by the user faster than they can process them - too much noise, to see if there is a way to prioritise some messages over others, extract out key highlights from the conversation, categorise and add context to messages, filters, search, pluggable analysis and AI features for summerisation and suggestions.

Feature Ideas (not all are possible in the time)

- Read the notifications from the activity feed
- Notifications convert to tasks (manual and automated with rules)
- Highlight active Teams and Channels for quick access and notification summaries
- Provide summerisation of the activity item on specific mentions; mentions are users intentional "Hey, please focus on this"
- List out ettiquite practices
- Identify noisy sources and suggest to mute them if interaction is low from the user
- Keep a reference of teams that you have participated in, but allow the user to leave the team, but keep a link to it, to re-join in future. 


### However in researching the hack I learned a few things

- In the Graph, the activity feed is not available in the Graph API as a read operation, only a write operation is available. There is a permission in AAD but no known API.
- Getting data from across chats and channels is possible with the Graph API but alot of subscriptions to each team is required using premium APIs and doesnt quite hit the mark with the data we need for this scenario - inviting other issues over compliance and security into the app.
- Attempts to see how Teams does it failed, the calls to the service as well hidden,
- There are opportunities to improve the graph documentation for event hubs and subscriptions.
- Teams Toolkit v5 does not support Visual Studio at this time
- The MS Learn modules for the Graph Change notifications are not up to date with the latest changes to the Graph SDK C# library
- Some GitHub Copilot X usage was done to hit the ground running with some of the code
- Spent some time looking at the Graph SDK C#, there isnt specific documentation on using this library but learned how to start using it.

### Distractions and future exporations

- Dev Tunnels (very cool), some samples use ngrok - Can I convert over? How?
- Dev Home/Dev Drives - can I work faster with the drives for Teams development? Very early days, lots of potential, and setup time for test scenario
- 

## Idea 2

Instead of reading the notifications, can we see if we can display quickly the notification settings for all the teams you are part of. Can the user adjust the settings from the Team for personal settings. 

Thinking of Team Settings + When the User last interacted with the Team, to suggest to turn down the notifications for the Team.


# Resources

- https://github.com/microsoft/hack-together-teams

## Design

- https://learn.microsoft.com/en-us/microsoftteams/platform/concepts/design/design-teams-app-overview
- https://63587347138fdad13ed63ccd-omfbjvvebn.chromatic.com/
- https://github.com/OfficeDev/microsoft-teams-ui-component-library


## Teams Development Resources

- https://learn.microsoft.com/en-us/graph/toolkit/overview?context=graph%2Fapi%2F1.0&view=graph-rest-1.0
- [Graph Toolkit Playground](https://mgt.dev/)
- https://learn.microsoft.com/en-us/graph/toolkit/overview

## Azure Resources

- https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs-azure-cli%2Chttp
- https://learn.microsoft.com/en-us/azure/event-hubs/
- https://learn.microsoft.com/en-us/azure/developer/dev-tunnels/get-started?tabs=windows
- https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs--azure-portal%2Chttp