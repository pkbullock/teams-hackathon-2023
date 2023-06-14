import { useContext, useState } from "react";
import { TeamsFxContext } from "./Context";
import config from "./lib/config";
import { useData } from "@microsoft/teamsfx-react";
import { apiClient } from './lib/apiClient';
import { Welcome } from "./sample/Welcome";
import './ActivityView.css';
import { CurrentUser } from "./sample/CurrentUser";
import { SelectTabData, SelectTabEvent, Tab, TabList, TabValue } from "@fluentui/react-components";
import NotificationItems from "./NotificationItems";

const showFunction = Boolean(config.apiName);


//function to display the response body
async function DisplayResponse() {

  const responseBody = apiClient(true);

  if (responseBody) {
    return responseBody;
  }
}

export default function ActivityView() {
  const { themeString } = useContext(TeamsFxContext);

  const [selectedValue, setSelectedValue] = useState<TabValue>("local");
  const onTabSelect = (event: SelectTabEvent, data: SelectTabData) => {
    setSelectedValue(data.value);
  };


  const notifications = useData(() => DisplayResponse(), {
    autoLoad: true,
  });


  const { teamsUserCredential } = useContext(TeamsFxContext);
  const userData = useData(async () => {
    if (teamsUserCredential) {
      const userInfo = await teamsUserCredential.getUserInfo();
      return userInfo;
    }
  });
  const userName = userData.loading || userData.error ? "" : userData.data!.displayName;


  return (
    <div
      className={themeString === "default" ? "light" : themeString === "dark" ? "dark" : "contrast"}
    >

      <div className="page-container">

        {/* Testing Only */}
        {!notifications.loading && !!notifications.data && !notifications.error && <div>
          {/* <h4>Response Data</h4> */}
          <pre className="fixed">
            {/* {JSON.stringify(data, null, 2)} */}
          </pre>
        </div>
        }



        <h1 className="center">My Notification Mananger {userName ? ", " + userName : ""}!</h1>

        <div className="tabList">
          <TabList selectedValue={selectedValue} onTabSelect={onTabSelect}>
            <Tab id="your-mentions" value="your-mentions">
              Your Mentions
            </Tab>
            <Tab id="team-mentions" value="team-mentions">
              Team Mentions
            </Tab>
            <Tab id="reactions" value="reactions">
              Message Reactions
            </Tab>
            <Tab id="your-prioritised-teams" value="your-prioritised-teams">
              Your prioritised Teams
            </Tab>
            <Tab id="your-unused-teams" value="your-unused-teams">
              Your Used Teams
            </Tab>
          </TabList>


          <div>
            {selectedValue === "your-mentions" && (
              <div>
                Your mentions placeholder
                <NotificationItems notificationData={notifications.data} activityType="chatMention" />
              </div>
            )}
            {selectedValue === "team-mentions" && (
              <div>
                Team mentions placeholder
                <NotificationItems notificationData={notifications.data}  activityType="channelMention"/>
              </div>
            )}
            {selectedValue === "reactions" && (
              <div>
                Your mentions placeholder
                <NotificationItems notificationData={notifications.data} activityType="channelMention" />
              </div>
            )}
          </div>

          {/* <h2>About you</h2>
        <CurrentUser userName={userName} />


        {!notifications.loading && !!notifications.error && <div className="error fixed">
          <h4>Testing the API Management Call isnt going so well</h4>
          {(notifications.error as any).toString()}</div>
        } */}

        </div>

      </div>

    </div>
  );
}
