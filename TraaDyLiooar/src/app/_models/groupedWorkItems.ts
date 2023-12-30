/*
    Represents a work items, grouped by date
*/

import { WorkItem } from "./workItem";

export interface GroupedWorkItems {
    friendlyDate: string; 
    key: string;
    workItems: WorkItem[]
}