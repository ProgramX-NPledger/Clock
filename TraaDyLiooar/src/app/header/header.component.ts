import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { timer } from 'rxjs';
import { RepositoryService } from '../_services/repository.service';
import { WorkItem } from '../_models/workItem';
import { ActiveWorkItem } from '../_models/activeWorkItem';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  @Output() submitWorkItem = new EventEmitter();

  model: ActiveWorkItem={
    started: 0,
    timerValue:0,
    title: null
  };
  timeLeft: number = 10;
  interval : any;
  subscribeTimer: any;

  constructor(public repositoryService : RepositoryService) {

  }

  ngOnInit(): void {
    this.startTimer();

  }


  
  // https://stackoverflow.com/questions/50455347/how-to-do-a-timer-in-angular-5
  
  observableTimer() {
    const source = timer(1000, 2000);
    const abc = source.subscribe(val => {
      console.log(val, '-');
      alert(this.timeLeft);
      this.subscribeTimer = this.timeLeft - val;
    });
  }
  
  startTimer() {
    this.interval = setInterval(() => {
        this.model.timerValue++;
    },1000)
  }

  recordTime() {
    const workItem = <WorkItem>({
      ended: this.model.timerValue,
      started: this.model.started,
      title: this.model.title
    });
  
    this.submitWorkItem.emit(this.model);
    // reset the clock
    this.model.started=this.model.timerValue;
    this.model.timerValue = 0;
    this.model.title='';

    console.log(this.model);
  }

}

/*
https://buddy.works/tutorials/building-a-desktop-app-with-electron-and-angular
51 in Udemy
*/
