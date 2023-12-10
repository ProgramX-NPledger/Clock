import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  model: any={}

  constructor() {

  }

  recordTime() {
    console.log(this.model);
  }

}

/*
https://buddy.works/tutorials/building-a-desktop-app-with-electron-and-angular
51 in Udemy
*/
