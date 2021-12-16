import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  providers: [{ provide: BsDropdownConfig, useValue: { isAnimated: true, autoClose: false } }]
})
export class NavComponent implements OnInit {
model:any = {}
// currentUser$ : Observable<User>;
  constructor(public accountService : AccountService, private router : Router, private toastr : ToastrService) { }

  ngOnInit(): void {
  //  this.currentUser$ = this.accountService.currentUser$;
  }

  async login(){
    console.log(this.model);
    await (await this.accountService.login(this.model)).subscribe(response =>{
      this.router.navigateByUrl('/members');
     this.toastr.success('Welcome!','Login Successfull :)');
    },
   )
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
    
  }

  

}
