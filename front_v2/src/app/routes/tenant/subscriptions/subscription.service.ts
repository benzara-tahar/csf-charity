import { Injectable } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Subscription } from './models/subscription';

@Injectable({
  providedIn: 'root',
})
export class SubscriptionService {
  private baseUrl: string = environment.urls.main + '/api/webhook-subscription';

  constructor(protected httpClient: _HttpClient) { }

  getAll(): Observable<Subscription[]> {
    const url = `${this.baseUrl}`;
    return this.httpClient.get<any>(url).pipe(map((result) => result.data));
  }

  filterSubscriptions(tenantId: string): Observable<Subscription[]> {
    const url = `${this.baseUrl}/${tenantId}`;
    return this.httpClient.get<any>(url).pipe(map((result) => result.data));
  }

  getByTenant(): Observable<Subscription[]> {
    const url = `${this.baseUrl}/by-tenant`;
    return this.httpClient.get<any>(url).pipe(map((result) => result.data));
  }

  create(subscription: Subscription): Observable<Subscription> {
    const url = `${this.baseUrl}`;
    return this.httpClient.post<any>(url, subscription).pipe(map((result) => result.data));
  }

  update(subscription: Subscription): Observable<any> {
    const url = `${this.baseUrl}`;
    return this.httpClient.patch(url, subscription).pipe(map((result) => result.data));
  }

  delete(subscriptionId: string): Observable<any> {
    const url = `${this.baseUrl}/${subscriptionId}`;
    return this.httpClient.delete(url).pipe(map((result) => result.data));
  }

  getWebhookDefinitions(): Observable<any> {
    const url = `${this.baseUrl}/webhook-definitions`;
    return this.httpClient.get(url).pipe(map((result) => result.data));
  }

}
