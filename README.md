# teams-hackathon-2023

The purpose of this hack is to see if we can read the message notifications of the user from their activity feed or multiple chats, organise them into a friendlier more manageable view. 

> Note: This is a hack and exploration of an idea, not a production ready app, there are some issues with the code, and it cannot work without Microsoft making changes to the Microsoft Graph.

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




### Distractions and future exporations

- Dev Tunnels (very cool), some samples use ngrok - Can I convert over? How?
- Dev Home/Dev Drives - can I work faster with the drives for Teams development? Very early days, lots of potential, and setup time for test scenario
- 

## Idea 2

Instead of reading the notifications, can we see if we can display quickly the notification settings for all the teams you are part of. Can the user adjust the settings from the Team for personal settings. 

Thinking of Team Settings + When the User last interacted with the Team, to suggest to turn down the notifications for the Team.


# Outcomes from the hackathon

## What I have learned

The hackathon was an opportunity to give a massive boost to all the components, tools, and services that are available to developers to build Teams apps. 
The resources listed below have been emensly helpful in getting up to speed with the latest and greatest, all the resources referenced by the hack, tools available are so much slicker and easier to use than before - which is super cool. I am rusty with deveopment so this has been a fun and positive learning experience with the intention to write blogs and share the knowledge with others, some issues discovered and ideas to share. 

This will certainly kick my learning back into a regular flow - which I also have permission slip from wife ;-)


## What I have built

The components I have built are:

- A Teams App with a Tab that provides a UI to display the notifications from a custom API (simulating the Microsoft Graph Activity API, if there was one)
- Backend API to simulate the outputs from activities and notifications (MockAPI)
- Exploration of Microsoft Graph and Event Hubs to see if getting all chats would suit the requirements
- Exploration of a ASP.NET Web Api to provide as a consumer of the Microsoft Graph Change Notifications

## What I struggled with

There are some elements about wiring all the components together specifically, the API management calls to the backend API, I made to simulate a tool reading notifications from the Microsoft Graph Activity API. There was lots of learning and decided to mock the calls instead to allow more time to use the Teams Toolkit, React, Teams Development and the Graph Toolkit. Its a choose your battle problem. 

## Use of AI Help

I have GitHub Copilot X installed and used that to help build elements quicker and more often to get past errors and challanges more so with react and the user interface. This was an opportunity to stress test the AI and see how it can help me with my development workflow. Another advantage, I was able to get a significant boost in productivity building a simple UI in a few hours, which was useful as I felt I was behind lol.

Cool stuff, love it, can see that getting used heavily.

## In researching the hack I found some issues with a few things


- In the Graph, the activity feed is not available in the Graph API as a read operation, only a write operation is available. There is a permission in AAD but no known API.
- Getting data from across chats and channels is possible with the Graph API but alot of subscriptions to each team is required using premium APIs and doesnt quite hit the mark with the data we need for this scenario - inviting other issues over compliance and security into the app.
- Attempts to see how Teams does it failed, the calls to the service as well hidden
- There are opportunities to improve the graph documentation for event hubs and subscriptions.
- Teams Toolkit v5 does not support Visual Studio at this time, shame but switched to Visual Studio Code
- The MS Learn modules for the Graph Change notifications are not up to date with the latest changes to the Graph SDK C# library, found had to start rewriting the code.
- Spent some time looking at the Graph SDK C#, there isnt specific documentation on using this library but learned how to start using it, no API reference? Or did I miss it?

# Resources

- https://github.com/microsoft/hack-together-teams

## Design

- https://learn.microsoft.com/en-us/microsoftteams/platform/concepts/design/design-teams-app-overview
- https://63587347138fdad13ed63ccd-omfbjvvebn.chromatic.com/
- https://github.com/OfficeDev/microsoft-teams-ui-component-library
- https://developer.microsoft.com/en-us/fluentui#/controls/web
- https://learn.microsoft.com/en-us/graph/toolkit/get-started/mgt-react


## Teams Development Resources

- https://learn.microsoft.com/en-us/graph/toolkit/overview?context=graph%2Fapi%2F1.0&view=graph-rest-1.0
- [Graph Toolkit Playground](https://mgt.dev/)
- https://learn.microsoft.com/en-us/graph/toolkit/overview
- https://learn.microsoft.com/en-us/microsoftteams/platform/concepts/design/design-teams-app-ui-templates
- https://axios-http.com/docs/req_config
- https://learn.microsoft.com/en-us/microsoftteams/platform/toolkit/add-api-connection
- https://learn.microsoft.com/en-us/graph/toolkit/get-started/mgt-react
- https://learn.microsoft.com/en-us/microsoftteams/platform/toolkit/add-single-sign-on


## Azure Resources

- https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs-azure-cli%2Chttp
- https://learn.microsoft.com/en-us/azure/event-hubs/
- https://learn.microsoft.com/en-us/azure/developer/dev-tunnels/get-started?tabs=windows
- https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs--azure-portal%2Chttp
- https://learn.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-access-web-apis
- https://github.com/OfficeDev/TeamsFx/wiki/Integrate-Azure-API-Management-with-your-Teams-App-and-export-the-api-to-power-app
- https://github.com/OfficeDev/TeamsFx/wiki/Integrate-API-Connection-with-your-Teams-app
