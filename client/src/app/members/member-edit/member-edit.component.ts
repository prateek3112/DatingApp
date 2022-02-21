import { ToastrService } from 'ngx-toastr';
import { MembersService } from './../../services/members.service';
import { AccountService } from './../../services/account.service';
import { User } from './../../models/user';
import { Component, HostListener, Input, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/models/member';
import { take } from 'rxjs/operators';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  @HostListener('window:beforeunload',['$event']) unloadNotification($event : any){
    if(this.editForm.dirty){
      
      $event.returnValue = true;
    }
  }
  member: Member;
  user: User;
  constructor(
    private accountService: AccountService,
    private memberService: MembersService,
    private toastr: ToastrService
  ) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
    console.log(this.user);
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe((member) => {
      this.member = member;
    });
  }

  updateMember() {
    console.log(this.member);
    this.memberService.updateMember(this.member).subscribe(()=>{
      this.toastr.success('Profile Updated Successfully! ^_^');
      this.editForm.reset(this.member);
    })
   
  }
}
