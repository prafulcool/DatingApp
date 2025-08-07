import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { Member } from '../../_models/member';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_services/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {

  //private memberService = inject(MembersService);
  memberService = inject(MembersService);
  //members: Member[] = [];
  // pageNumber = 1;
  // pageSize = 5;
  //private accountService = inject(AccountService);
  //userParams = new UserParams(this.accountService.currentUser());
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display:'Females'}];

  ngOnInit(): void {
    //if(this.memberService.members.length == 0)
    if(!this.memberService.paginatedResult())
      this.loadMembers();
  }

  // loadMembers(){
  //   this.memberService.getMembers().subscribe({
  //     next: members => this.members = members
  //   })
  // }

  // loadMembers(){
  //   this.memberService.getMembers();
  // }

  loadMembers(){
    // this.memberService.getMembers(this.pageNumber, this.pageSize);
    this.memberService.getMembers();
  }

  resetFilters(){
    //this.userParams = new UserParams(this.accountService.currentUser());
    this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    // if(this.pageNumber !== event.page)
    // {
    //   this.pageNumber = event.page;
    //   this.loadMembers();
    // }
    if(this.memberService.userParams().pageNumber !== event.page)
    {
      this.memberService.userParams().pageNumber = event.page;
      this.loadMembers();
    }
  }

}
