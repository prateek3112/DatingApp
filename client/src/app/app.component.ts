import { AccountService } from './services/account.service';
import { KeyValue } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';
import { User } from './models/user';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users:any;

  constructor(private http : HttpClient, private accountService : AccountService ){

  }
  ngOnInit(){

  this.setCurrentUser();
  }

  setCurrentUser(){
    const user : User = JSON.parse(localStorage.getItem('user'));
this.accountService.setCurrentUser(user);
  }
 
  // getUsers(){
  //   this.http.get('https://localhost:5001/api/Users').subscribe(response =>{
  //     this.users=response;
  //     console.log(this.users);
      
  //    },error =>{
  //      console.log(error)
  //    });
     
  // }

  originalOrder = (
    a:KeyValue<number,string>,
    b:KeyValue<number,string>
      
  ) : number => {
    return 0
  };

}
