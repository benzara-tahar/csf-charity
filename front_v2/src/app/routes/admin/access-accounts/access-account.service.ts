import { Injectable } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IoTAccessAccount } from './models/access-account';

@Injectable({
  providedIn: 'root',
})
export class AccessAccountService {
  private baseUrl: string = environment.urls.main + '/api/access-accounts';

  constructor(protected httpClient: _HttpClient) { }

  getAll(): Observable<IoTAccessAccount[]> {
    const url = `${this.baseUrl}`;
    return this.httpClient.get<any>(url).pipe(map((result) => result.data));
  }

  create(user: IoTAccessAccount): Observable<IoTAccessAccount> {
    const url = `${this.baseUrl}`;
    return this.httpClient.post<any>(url, user).pipe(map((result) => result.data));
  }

  update(user: IoTAccessAccount): Observable<any> {
    const url = `${this.baseUrl}`;
    return this.httpClient.patch(url, user).pipe(map((result) => result.data));
  }

  delete(userId: string): Observable<any> {
    const url = `${this.baseUrl}/${userId}`;
    return this.httpClient.delete(url).pipe(map((result) => result.data));
  }
}
