import { KeyValue } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { error } from '@angular/compiler/src/util';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'client';
  users:any;

  constructor(private http : HttpClient){

  }
  ngOnInit(){
  this.getUsers();
  }

 
  getUsers(){
    this.http.get('https://localhost:5001/api/Users').subscribe(response =>{
      this.users=response;
      console.log(this.users);
      
     },error =>{
       console.log(error)
     });
     
  }

  originalOrder = (
    a:KeyValue<number,string>,
    b:KeyValue<number,string>
      
  ) : number => {
    return 0
  };

}
