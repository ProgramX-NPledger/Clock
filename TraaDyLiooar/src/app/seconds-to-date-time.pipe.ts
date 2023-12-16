/*

 https://betterprogramming.pub/why-and-how-to-create-an-impure-filter-pipe-in-angular-a3916de5841f

*/

import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'secondsToDateTime'
})

export class SecondsToDateTimePipe implements PipeTransform {

  transform(seconds: any, ...args: any[]): any {
    var d = new Date(0,0,0,0,0,0,0);
    d.setSeconds(seconds);
    return d;
  }

}
