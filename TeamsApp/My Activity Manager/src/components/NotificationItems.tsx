import { Fragment, useEffect, useState } from "react";
import { List, DetailsList, IColumn, ContextualMenu, IContextualMenuItem, DetailsListLayoutMode, IButtonProps, CommandBar, ICommandBarItemProps, setVirtualParent, initializeIcons } from '@fluentui/react';
import { Activity } from "./lib/IActivity";

interface NotificationItemsProps {
    notificationData: Activity[];
    activityType?: string;
}

export default function NotificationItems({ notificationData, activityType }: NotificationItemsProps) {

    const [notifications, setNotifications] = useState<Activity[]>([]);
    const [contextMenuProps, setContextMenuProps] = useState<{
        items: IContextualMenuItem[]
        target: any;
    } | undefined>(undefined);
    const overflowProps: IButtonProps = { ariaLabel: 'More commands' };

    //Filtering the data based on activityType
    useEffect(() => {
        if (activityType) {
            const filteredData = notificationData.filter(item => item.activityType === activityType);
            setNotifications(filteredData);
        } else {
            const filteredData = notificationData;
            setNotifications(filteredData);
        }
    }, [activityType]);

    // Column Defintion
    const columns: IColumn[] = [
        { key: 'column1', name: 'Topic', fieldName: 'topic', minWidth: 150, maxWidth: 150 },
        { key: 'column2', name: 'Preview Message', fieldName: 'previewText', minWidth: 250, maxWidth: 350 },
        { key: 'column3', name: 'Importance', fieldName: 'importance', minWidth: 100, maxWidth: 200 },
        { key: 'column4', name: 'Date', fieldName: 'dateReceived', minWidth: 100, maxWidth: 200 },

    ];

    // Context menu
    const onItemContextMenu = (item?: any, index?: number, ev?: Event) => {
        if (item && ev && ev.currentTarget) {
            const mouseEvent = ev as MouseEvent;

            // Show context menu for the selected item, this can be expanded to include further tasks to perfom on the item
            setContextMenuProps({
                items: [
                    {
                        key: 'view',
                        text: 'Navigate to Item',
                        onClick: () => console.log(`View item ${item.key}`),
                    },
                    {
                        key: 'read',
                        text: 'Mark as Read',
                        onClick: () => console.log(`Delete item ${item.key}`),
                    },
                    {
                        key: 'ai-summary',
                        text: 'Summerise with AI',
                        onClick: () => console.log(`Summerise item ${item.key}`),
                    },
                    {
                        key: 'create-task',
                        text: 'Create task based on item',
                        onClick: () => console.log(`Create task on item ${item.key}`),
                    }
                ],
                target: { x: mouseEvent.clientX, y: mouseEvent.clientY },

            });
        }
    };

    const onDismissContextMenu = () => {
        // Hide the context menu
        setContextMenuProps(undefined);
    };


    //Command Bar
    initializeIcons();
    const commandBarItems: ICommandBarItemProps[] = [
        {
            key: 'read',
            text: 'Mark as Read',
            iconProps: { iconName: 'Read' },
            onClick: () => console.log('Mark as Read - Feature Not Implemented'),
        },
        {
            key: 'task',
            text: 'Bulk Create Tasks',
            iconProps: { iconName: 'TaskLogo' },
            onClick: () => console.log('Bulk Create Tasks - Feature Not Implemented'),
        },
        {
            key: 'categorise',
            text: 'Categorise',
            iconProps: { iconName: 'Source' },
            onClick: () => console.log('Categorise - Feature Not Implemented'),
        },
        {
            key: 'summarise',
            text: 'Summarise Item with AI',
            iconProps: { iconName: 'Articles' },
            onClick: () => console.log('Summarise - Feature Not Implemented'),
        },
    ];

    const _overflowItems: ICommandBarItemProps[] = [
        { 
            key: 'delete', 
            text: 'Delete', 
            onClick: () => console.log('Delete - Feature Not Implemented'), iconProps: { iconName: 'Delete' } 
        },
        {   
            key: 'share', 
            text: 'Share', 
            onClick: () => console.log('Share - Feature Not Implemented'), iconProps: { iconName: 'Share' } 
        }
    ];

    //Data Filtering
    const items = notifications.map(item => ({
        key: item.id,
        topic: item.topic.value,
        previewText: item.previewText.content,
        importance: item.importance,
        dateReceived: new Date(item.dateReceived).toLocaleDateString(),
    }));

    return (
        <Fragment>

            <CommandBar
                items={commandBarItems}
                overflowItems={_overflowItems}
                overflowButtonProps={overflowProps}
                ariaLabel="Notification Actions"
                primaryGroupAriaLabel="Perform a function on notification" farItemsGroupAriaLabel="More actions"
            />

            <DetailsList items={items}
                columns={columns}
                onItemContextMenu={onItemContextMenu}
                layoutMode={DetailsListLayoutMode.justified}
                isHeaderVisible={true} />
            {contextMenuProps && (
                <ContextualMenu items={contextMenuProps.items} onDismiss={onDismissContextMenu} target={contextMenuProps.target} />
            )}



        </Fragment>
    );
}
