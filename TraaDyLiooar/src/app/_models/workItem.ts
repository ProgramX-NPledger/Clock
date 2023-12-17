/*
    Represents a work item, either currently active or historical
*/

export interface WorkItem {
    started: any;
    ended: any|null;
    title: string;
    
}