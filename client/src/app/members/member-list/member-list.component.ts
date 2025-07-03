import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { Member } from '../../_models/member';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {

  //private memberService = inject(MembersService);
  memberService = inject(MembersService);
  //members: Member[] = [];

  ngOnInit(): void {
    if(this.memberService.members.length == 0)
      this.loadMembers();
  }

  // loadMembers(){
  //   this.memberService.getMembers().subscribe({
  //     next: members => this.members = members
  //   })
  // }

  loadMembers(){
    this.memberService.getMembers();
  }

}
