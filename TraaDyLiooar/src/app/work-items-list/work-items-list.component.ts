import { Component, Input } from '@angular/core';
import { WorkItem } from '../_models/workItem';
import { GroupedWorkItems } from '../_models/groupedWorkItems';

@Component({
  selector: 'app-work-items-list',
  templateUrl: './work-items-list.component.html',
  styleUrls: ['./work-items-list.component.css']
})
export class WorkItemsListComponent {
  @Input() workItems: WorkItem[] = [];
  lastGroupedWorkItems: Array<any> = [];


  // get dates
  getWorkItemsByDay() {   
    //this updates every second, maybe ake it observeable? Use async pipe to monitor for changes
    // https://angular.io/guide/observables-in-angular
    var grouped:Array<GroupedWorkItems> = [];
    this.workItems.forEach(workItem => {
      const friendlyDate = this.getFriendlyDate(workItem.startedTimeStamp);
      let matchingGroupedWorkItem: GroupedWorkItems | undefined = grouped.find((obj)=> {
        let friendlyDate=this.getFriendlyDate(workItem.startedTimeStamp);
        return obj.key==friendlyDate;
      })
      if (!matchingGroupedWorkItem) {
        let matchingGroupedWorkItem = {
          friendlyDate: friendlyDate,
          key: friendlyDate,
          workItems:  [
            workItem
          ]
        }
        grouped.push(matchingGroupedWorkItem);
      } 
      if (matchingGroupedWorkItem) {
        matchingGroupedWorkItem.workItems.push(workItem);
      } 
    });
    
    // grouped= [
    //   {
    //     friendlyDate: '2023-12-30',
    //     key: '2023-12-30',
    //     workItems: [
    //       {
    //         ended:1,
    //         startedTimeStamp:new Date(),
    //         title:'Started'
    //       }
    //     ]
    //   }
    // ];

    return grouped;
  }

  getFriendlyDate(dateDef:any) : string {
    const date = dateDef as unknown as string;
    const dateAsString = date.split('T')[0]
    return dateAsString;
  }
}
