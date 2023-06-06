# teams-hackathon-2023

The purpose of this hack is to see if we can read the message notifications of the user from their activity feed or multiple chats. 

## What is the thinking behind this hack?

The productivity booster idea is to read the notifications and allow users to see which messages could be actions, questions or tasks, to see all the message contents in a single view from multiple chats and channels, then perform actions on them. 

This scenario fits a problem with information overload in Teams where potentially loads of messages are recieved by the user faster than they can process them, to see if there is a way to prioritise some messages over others.

## Known Challanges

THe graph API has no read operation on activity so looking for alternative way to get this data feed in order for processing to happen.

# Resources

- [https://github.com/microsoft/hack-together-teams](https://github.com/microsoft/hack-together-teams)

## Design

- [https://learn.microsoft.com/en-us/microsoftteams/platform/concepts/design/design-teams-app-overview](https://learn.microsoft.com/en-us/microsoftteams/platform/concepts/design/design-teams-app-overview)
- [https://63587347138fdad13ed63ccd-omfbjvvebn.chromatic.com/](https://63587347138fdad13ed63ccd-omfbjvvebn.chromatic.com/)
- [https://github.com/OfficeDev/microsoft-teams-ui-component-library](https://github.com/OfficeDev/microsoft-teams-ui-component-library)


## Teams Development Resources

- [https://learn.microsoft.com/en-us/graph/toolkit/overview?context=graph%2Fapi%2F1.0&view=graph-rest-1.0](https://learn.microsoft.com/en-us/graph/toolkit/overview?context=graph%2Fapi%2F1.0&view=graph-rest-1.0)
- [Graph Toolkit Playground](https://mgt.dev/)

## Azure Resources

- [https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs-azure-cli%2Chttp](https://learn.microsoft.com/en-us/graph/change-notifications-delivery-event-hubs?view=graph-rest-1.0&tabs=change-notifications-eventhubs-azure-cli%2Chttp)
- [https://learn.microsoft.com/en-us/azure/event-hubs/](https://learn.microsoft.com/en-us/azure/event-hubs/)


