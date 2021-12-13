import { ToastrService } from 'ngx-toastr';
import { AccountService } from './../services/account.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model:any = {}
@Output() cancelRegister = new EventEmitter();
  constructor(private accountService : AccountService, private toastr : ToastrService) { }

  ngOnInit(): void {
  }

  async register(){
    
    await (await this.accountService.login(this.model)).subscribe(response =>{
      console.log(response);
      this.cancel();
    },
    error=>{
      console.log(error);
      this.toastr.error(error.error);
    })
  }
cancel(){
  this.cancelRegister.emit(false);
}

}
