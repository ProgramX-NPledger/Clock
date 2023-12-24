/*
  Representing a repository for work items for addition/display to the UI
  Services are application-lifetime so will persist as components are built/destroyed

  Use localStorage to access browser storage, key-vaue pairs so use JSON to dehydrate
*/

import { Injectable } from '@angular/core';
import { WorkItem } from '../_models/workItem';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {

  private WORK_ITEMS_LOCAL_STORAGE_KEY = 'workItems';

  constructor() { }

  addWorkItem (workItem: WorkItem) {
    console.log('Repostiroy save');
    console.log(workItem);
    
    // get work items from local storage
    const existingWorkItems = this.getWorkItems();

    // add to work items
    existingWorkItems.push(workItem);

    // re-persist
    this.saveWorkItems(existingWorkItems);
  }

  getWorkItems () : WorkItem[] {
    const workItemsJson = localStorage.getItem(this.WORK_ITEMS_LOCAL_STORAGE_KEY);
    if (workItemsJson) {
      const workItems = JSON.parse(workItemsJson);
      // sort?
      return workItems;
    }
    return [];
  }

  private saveWorkItems(workItems : WorkItem[]) {
    const workItemsJson = JSON.stringify(workItems);
    localStorage.setItem(this.WORK_ITEMS_LOCAL_STORAGE_KEY, workItemsJson);
  }
}

// perhaps have work items (current and historical) as observable ~50+ in Udemy

// https://chat.openai.com/share/0c4254bd-80ba-464a-8171-a9138512963b

/*
Access fs object (et al) from Node, but will only work in main process NOT rendered components.
main.ts should be main process. components are render processes.
Use a service to route between component and main process
*/
