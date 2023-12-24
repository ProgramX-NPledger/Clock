/*
    Represents a work item, either currently active or historical
*/

export interface ActiveWorkItem {
    started: any;
    timerValue: number;    
    title: string|null;
}