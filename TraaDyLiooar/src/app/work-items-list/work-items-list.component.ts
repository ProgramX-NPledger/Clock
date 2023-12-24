import { Component, Input } from '@angular/core';
import { WorkItem } from '../_models/workItem';

@Component({
  selector: 'app-work-items-list',
  templateUrl: './work-items-list.component.html',
  styleUrls: ['./work-items-list.component.css']
})
export class WorkItemsListComponent {
  @Input() workItems: WorkItem[] = [];

}
