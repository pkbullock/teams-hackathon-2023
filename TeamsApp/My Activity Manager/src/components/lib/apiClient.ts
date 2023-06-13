import { AppCredential, AppCredentialAuthConfig } from "@microsoft/teamsfx";
import axios from "axios";




export function apiClient(mockAPICall?: boolean, inSecureAPICall?: boolean) {
  let responseBody;

  if (mockAPICall) {

    if (inSecureAPICall) {
      responseBody = getNotificationsReceiver();

    } else {
      responseBody = fakeData();
    }

  } else {

    const teamsfxSdk = require("@microsoft/teamsfx");

    // Initialize a new axios instance to call kudos, store API key in request header.
    const appAuthConfig: AppCredentialAuthConfig = {
      authorityHost: process.env.AAD_APP_OAUTH_AUTHORITY_HOST || "",
      clientId: process.env.TEAMSFX_API_CLIENT_ID || "",
      tenantId: process.env.TEAMSFX_API_TENANT_ID || "",
      clientSecret: process.env.SECRET_TEAMSFX_API_CLIENT_SECRET || "",
    };
    
    const appCredential = new AppCredential(appAuthConfig);
    // Initialize a new axios instance to call your API
    const authProvider = new teamsfxSdk.BearerTokenAuthProvider(
    
      async () => {
        const tokenResult = await appCredential.getToken("NotificationActivity.Read");
        return tokenResult?.token || "";
      }
    );
    const apiClient = teamsfxSdk.createApiClient(
      process.env.TEAMSFX_API_ENDPOINT,
      authProvider
    );
    const response = apiClient.get("api/NotificationsReciever");

    responseBody = response.data;
  }

  return responseBody;
}


async function getNotificationsReceiver() {
  try {
    
    const httpClient = axios.create({

      //TODO: Fix this reference
      baseURL: "https://mockapiteamsgraphcallsapi.azure-api.net/api",
    });

    const response = await httpClient.get("/NotificationsReciever");
    console.log(response.data);
    return response.data;
  } catch (error) {
    console.error(error);
  }
}

function fakeData(){

  const data = require("./fakeData.json");

  return data;
}