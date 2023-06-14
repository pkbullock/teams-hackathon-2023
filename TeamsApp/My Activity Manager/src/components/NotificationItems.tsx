import { Fragment, useEffect, useState } from "react";
import { List } from '@fluentui/react';
import { Activity } from "./lib/IActivity";

interface NotificationItemsProps {
    notificationData: Activity[];
    activityType?:string;
  }

export default function NotificationItems({ notificationData, activityType }: NotificationItemsProps) {


    const [notifications, setNotifications] = useState<Activity[]>([]);

    useEffect(() => {
    
      // Filter the data based on activityType
      if(activityType){
        const filteredData = notificationData.filter(item => item.activityType === activityType);
        setNotifications(filteredData);
      }else{
        const filteredData = notificationData;
        setNotifications(filteredData);
      }
    }, [activityType]);


    return (
        <List items={notifications} onRenderCell={onRenderItem} />
    );
}


function onRenderItem(item?:Activity) {
    return (
      <div>
        <div>{item?.topic.value}</div>
        <div>{item?.previewText.content}</div>
      </div>
    );
  }