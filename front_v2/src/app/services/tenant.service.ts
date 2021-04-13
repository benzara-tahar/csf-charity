import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Tenant } from '../shared/model/tenant';

@Injectable({
  providedIn: 'root',
})
export class TenantService {
  private baseUrl: string = environment.urls.main + '/api/tenant';

  constructor(protected httpClient: _HttpClient) {}

  get(id: string): Observable<Tenant> {
    const url = `${this.baseUrl}`;
    return this.httpClient.get<any>(`url/${id}`).pipe(map((result) => result.data));
  }
  getAll(): Observable<Tenant[]> {
    const url = `${this.baseUrl}`;
    return this.httpClient.get<any>(url).pipe(map((result) => result.data));
  }

  create(tenant: Tenant): Observable<Tenant> {
    const url = `${this.baseUrl}`;
    return this.httpClient.post<any>(url, tenant).pipe(map((result) => result.data));
  }

  update(tenant: Tenant): Observable<any> {
    const url = `${this.baseUrl}`;
    return this.httpClient.patch(url, tenant).pipe(map((result) => result.data));
  }

  delete(tenantId: string): Observable<any> {
    const url = `${this.baseUrl}/${tenantId}`;
    return this.httpClient.delete(url).pipe(map((result) => result.data));
  }
}
