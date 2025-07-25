import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { User } from '../_models/User';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;//'https://localhost:5001/api/';
  currentUser = signal<User | null>(null);
  //constructor() { }
  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => { console.log(user.photoUrl);
        if(user){
          this.setCurrentUser(user);
          //console.log(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    );
  }

  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user)
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
