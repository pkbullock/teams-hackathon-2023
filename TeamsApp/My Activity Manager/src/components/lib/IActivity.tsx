export interface Activity {
    id: string;
    dateReceived: string;
    topic: {
      source: string;
      value: string;
      webUrl: string;
    };
    activityType: string;
    previewText: {
      content: string;
    };
    importance: string;
    locale: string;
    from: {
      application: null;
      device: null;
      user: {
        id: string;
        displayName: string;
        userIdentityType: string;
      };
    };
  }