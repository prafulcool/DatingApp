//import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
//import { AccountService } from './account.service';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private https = inject(HttpClient);
  //private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;

  // getMembers(){
  //   return this.https.get<Member[]>(this.baseUrl + 'users', this.getHttpOptions());
  // }

  // getMember(username: string){
  //   return this.https.get<Member>(this.baseUrl + 'users/' + username, this.getHttpOptions());
  // }

  // getHttpOptions(){
  //   return{
  //     headers: new HttpHeaders({
  //       Authorization: `Bearer ${this.accountService.currentUser()?.token}` //backtick used for template literal
  //     })
  //   }
  // }

  getMembers(){
    return this.https.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username: string){
    console.log("I called: " + username);
    return this.https.get<Member>(this.baseUrl + 'users/' + username);
  }
}
