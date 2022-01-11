import { MembersService } from './../../services/members.service';
import { Member } from './../../models/member';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members$:Observable<Member[]>;

  constructor(private memberService : MembersService) { }

  ngOnInit(): void {
    this.members$ = this.memberService.getMembers();
  }

}
