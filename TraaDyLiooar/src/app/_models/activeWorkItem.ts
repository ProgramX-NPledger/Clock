/*
    Represents a work item, either currently active or historical
*/

export interface ActiveWorkItem {
    startedTimeStamp: Date; 
    timerValueSeconds: number;    
    title: string|null;
}