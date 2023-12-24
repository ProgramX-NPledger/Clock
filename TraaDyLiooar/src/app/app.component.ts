import { Component, OnInit } from '@angular/core';
import { RepositoryService } from './_services/repository.service';
import { WorkItem } from './_models/workItem';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'TraaDyLiooar';
  workItems : WorkItem[] = [];

  constructor (private repositoryService : RepositoryService) {

  }

  ngOnInit(): void {
    // load work items and display at outset
    this.loadAndRefreshWorkItems();
  }

  submitWorkItem(event: WorkItem) {
    // received from the recordTime method in the header component
    // submits a work item model for storage
    this.repositoryService.addWorkItem(event);
      // load and refresh work items
    this.loadAndRefreshWorkItems(); 
  }

  loadAndRefreshWorkItems() {
    const workItems = this.repositoryService.getWorkItems();
    // work items need to be passed to the work Items child for display
    this.workItems=workItems;
  }
}
