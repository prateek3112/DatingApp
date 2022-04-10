import { NgForm } from '@angular/forms';
import { MessageService } from './../../services/message.service';
import { Message } from './../../models/message';
import { Component, Input, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm : NgForm;
@Input() username : string;
@Input() messages : Message[];
messageContent : string;
currentUser : string
showLastSeen = false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.currentUser = JSON.parse(localStorage.getItem('user')).username;
    console.log(this.currentUser);
  }

  sendMessage(){
    this.messageService.sendMessage(this.username, this.messageContent).subscribe(message =>{
      this.messages.push(message);
      this.messageForm.reset();
    })
  }

  deleteMessage(id : number){
    this.messageService.deleteMessage(id).subscribe(()=>{
      this.messages.splice(this.messages.findIndex(m => m.id === id),1);
    })
      }

  show(){
    this.showLastSeen = !this.showLastSeen;
  }

}
