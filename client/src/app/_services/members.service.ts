//import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
//import { AccountService } from './account.service';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  //private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  members = signal<Member[]>([]);

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
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
      next: members => this.members.set(members)
    });
  }

  getMember(username: string){
    const member = this.members().find(x => x.userName == username);
    console.log("I called: " + username);
    if(member !== undefined) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  //

  // updatemember(member: Member)
  // {
  //   return this.http.put(this.baseUrl + 'users', member).pipe(
  //     tap(() => {
  //       this.members.update(members => members.map(m => m.userName === member.userName ? member : m))
  //   })
  //   );
  // }

  updatemember(member: Member)
  {
    return this.http.put(this.baseUrl + 'users', member)
  }

  setMainPhoto(photo: Photo){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe(
      tap(() => {
        this.members.update(members => members.map(m => {
          if(m.photos.includes(photo))
          {
            m.photoUrl = photo.url;
          }
          return m;
        }))
      })
    )
  }

  deletePhoto(photo: Photo){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
      tap(() => {
      this.members.update(members => members.map(m => {
        if(m.photos.includes(photo)){
          m.photos = m.photos.filter(x => x.id !== photo.id)
        }
        return m;
      }))
      })
    )
  }
}
