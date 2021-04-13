import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<boolean> {
    return this.http
      .post<{ token: string }>('/api/auth', { username, password })
      .pipe(
        map((result) => {
          localStorage.setItem('access_token', result.token);
          return true;
        }),
      );
  }

  logout(): Observable<any> {
    localStorage.removeItem('access_token');
    return of(true);
  }

  authorizeCallback(): Observable<any> {
    return of(true);
  }
  public get loggedIn(): boolean {
    return localStorage.getItem('access_token') !== null;
  }
}
