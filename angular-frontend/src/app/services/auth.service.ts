import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  public register(user: User): Observable<any> {
    return this.http.post<any>(
      'https://localhost:7107/api/auth/register', 
      user
    );
  }

  public login(user: User): Observable<string> {
    return this.http.post(
      'https://localhost:7107/api/auth/login', 
      user,
      { responseType: 'text' }
    );
  }

  public getMe(): Observable<string> {
    return this.http.get(
      'https://localhost:7107/api/auth',
      { responseType: 'text' }
    );
  }
}
