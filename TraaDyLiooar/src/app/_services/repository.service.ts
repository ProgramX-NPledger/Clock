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
    // get work items from local storage
    const existingWorkItems = this.getWorkItems();

    // add to work items
    existingWorkItems.push(workItem);

    // re-persist
  }

  getWorkItems () : WorkItem[] {
    return [];
  }

  private saveWorkItems(workItems : WorkItem[]) {
    const workItemsJson = JSON.stringify(workItems);
    localStorage.setItem(this.WORK_ITEMS_LOCAL_STORAGE_KEY, workItemsJson);
  }
}

// perhaps have work items (current and historical) as observable ~50+ in Udemy
