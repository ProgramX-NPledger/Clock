import { Component } from '@angular/core';
import { RepositoryService } from './_services/repository.service';
import { WorkItem } from './_models/workItem';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'TraaDyLiooar';

  constructor (private repositoryService : RepositoryService) {

  }

  submitWorkItem(event: WorkItem) {
    // received from the recordTime method in the header component
    // submits a work item model for storage
    this.repositoryService.addWorkItem(event);
  }
}
