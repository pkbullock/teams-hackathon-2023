import { useContext } from "react";
import { TeamsFxContext } from "./Context";
import config from "./lib/config";
import { useData } from "@microsoft/teamsfx-react";
import { apiClient } from './lib/apiClient';

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

  const { loading, data, error, reload } = useData(() => DisplayResponse(), {
    autoLoad: true,
  });


  return (
    <div
      className={themeString === "default" ? "light" : themeString === "dark" ? "dark" : "contrast"}
    >

    {!loading && !!data && !error && <div>
      <h4>Response Data</h4>
      <pre className="fixed">
        Data:
        {JSON.stringify(data, null, 2)}</pre>
      </div>
    }


    {!loading && !!error && <div className="error fixed">
    <h4>Testing the API Management Call isnt going so well</h4>
      {(error as any).toString()}</div>
    }
    
    </div>
  );
}
