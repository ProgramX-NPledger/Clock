import { Component, OnInit } from '@angular/core';
import { timer } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  model: any={
    timerValue: 0
  };
  timeLeft: number = 10;
  interval : any;
  subscribeTimer: any;

  constructor() {

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
      if(this.timeLeft > 0) {
        this.timeLeft--;
        this.model.timerValue++;
      } else {
        this.timeLeft = 10;
      }
    },1000)
  }

  recordTime() {
    console.log(this.model);
  }

}

/*
https://buddy.works/tutorials/building-a-desktop-app-with-electron-and-angular
51 in Udemy
*/
