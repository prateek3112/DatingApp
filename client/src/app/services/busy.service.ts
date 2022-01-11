import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
busyRequestCount = 0;
  constructor(private spinner : NgxSpinnerService) { }

  busy(){
    this.busyRequestCount++;
    this.spinner.show();
  }

  idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0){
      this.busyRequestCount = 0;
      this.spinner.hide();
    }
  }
}
