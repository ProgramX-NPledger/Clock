/*
    Represents a work item, either currently active or historical
*/

export interface WorkItem {
    startedTimeStamp: Date; 
    ended: any|null; 
    title: string|null;   
}