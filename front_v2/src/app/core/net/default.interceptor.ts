import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponseBase } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';
import { _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { BehaviorSubject, Observable, of, throwError } from 'rxjs';
import { catchError, filter, mergeMap, switchMap, take, tap } from 'rxjs/operators';

const CODEMESSAGE: { [key: number]: string } = {
  200: 'The server successfully returned the requested data. ',
  201: 'Create or modify data successfully. ',
  202: 'A request has entered the background queue (asynchronous task). ',
  204: 'Delete data successfully. ',
  400: 'There was an error in the request sent, and the server did not create or modify data. ',
  401: 'The user does not have permission (the token, username, password are wrong). ',
  403: 'The user is authorized, but access is forbidden. ',
  404: 'The request sent is for a record that does not exist, and the server is not operating. ',
  406: 'The requested format is not available. ',
  410: 'The requested resource has been permanently deleted and will no longer be available. ',
  422: 'When creating an object, a validation error occurred. ',
  500: 'An error occurred in the server, please check the server. ',
  502: 'Gateway error. ',
  503: 'The service is unavailable, the server is temporarily overloaded or maintained. ',
  504: 'The gateway has timed out. ',
};

/**
 * The default HTTP interceptor, please refer to `app.module.ts` for registration details
 */
@Injectable()
export class DefaultInterceptor implements HttpInterceptor {
  private refreshTokenType: 're-request' | 'auth-refresh' = 'auth-refresh';
  private refreshToking = false;
  private refreshToken$: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private injector: Injector) {
    if (this.refreshTokenType === 'auth-refresh') {
      this.buildAuthRefresh();
    }
  }

  private get notification(): NzNotificationService {
    return this.injector.get(NzNotificationService);
  }

  private get tokenSrv(): ITokenService {
    return this.injector.get(DA_SERVICE_TOKEN);
  }

  private get http(): _HttpClient {
    return this.injector.get(_HttpClient);
  }

  private goTo(url: string): void {
    setTimeout(() => this.injector.get(Router).navigateByUrl(url));
  }

  private checkStatus(ev: HttpResponseBase): void {
    if ((ev.status >= 200 && ev.status < 300) || ev.status === 401) {
      return;
    }

    const errortext = CODEMESSAGE[ev.status] || ev.statusText;
    this.notification.error(`Error ${ev.status}: ${ev.url}`, errortext);
  }

  /**
   * refresh Token Request
   */
  private refreshTokenRequest(): Observable<any> {
    const model = this.tokenSrv.get();
    return this.http.post(`/api/auth/refresh`, null, null, { headers: { refresh_token: model?.refresh_token || '' } });
  }

  // #region Refresh Token Method 1: Use 401 to refresh Token

  private tryRefreshToken(ev: HttpResponseBase, req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    // 1 If the request is a refresh token request, it means that the refresh token can directly jump to the login page
    if ([`/api/auth/refresh`].some((url) => req.url.includes(url))) {
      this.toLogin();
      return throwError(ev);
    }
    // 2 If `refreshToking` is `true`, it means that it has been requested to refresh the token, all subsequent requests
    //  will be transferred to the waiting state, and the request will be re-initiated after the result is returned
    if (this.refreshToking) {
      return this.refreshToken$.pipe(
        filter((v) => !!v),
        take(1),
        switchMap(() => next.handle(this.reAttachToken(req))),
      );
    }
    // 3, Try to call refresh token
    this.refreshToking = true;
    this.refreshToken$.next(null);

    return this.refreshTokenRequest().pipe(
      switchMap((res) => {
        // Notify subsequent requests to continue execution
        this.refreshToking = false;
        this.refreshToken$.next(res);
        // Resave new token
        this.tokenSrv.set(res);
        // Reinitiate request
        return next.handle(this.reAttachToken(req));
      }),
      catchError((err) => {
        this.refreshToking = false;
        this.toLogin();
        return throwError(err);
      }),
    );
  }

  /**
   * Re-attach new Token information
   *
   * > Because of the request that has already been initiated, we will not go through `@delon/auth` again,
   *  so we need to reattach a new Token according to the business situation
   */
  private reAttachToken(req: HttpRequest<any>): HttpRequest<any> {
    // The following example is based on NG-ALAIN using `SimpleInterceptor` by default
    const token = this.tokenSrv.get()?.token;
    return req.clone({
      setHeaders: {
        token: `Bearer ${token}`,
      },
    });
  }

  // #endregion

  // #region Token refresh method 2: Use the `refresh` interface of `@delon/auth`

  private buildAuthRefresh(): void {
    this.tokenSrv.refresh
      .pipe(
        filter(() => !this.refreshToking),
        switchMap(() => {
          this.refreshToking = true;
          return this.refreshTokenRequest();
        }),
      )
      .subscribe(
        (res) => {
          // TODO: Mock expired value
          res.expired = +new Date() + 1000 * 60 * 5;
          this.refreshToking = false;
          this.tokenSrv.set(res);
        },
        () => this.toLogin(),
      );
  }

  // #endregion

  private toLogin(): void {
    this.notification.error(`Not logged in or login has expired, please log in again.`, ``);
    this.goTo('/passport/login');
  }

  private handleData(ev: HttpResponseBase, req: HttpRequest<any>, next: HttpHandler): Observable<any> {
    // The `end()` operation of `_HttpClient` may not be executed because of `throw` export
    if (ev.status > 0) {
      this.http.end();
    }
    this.checkStatus(ev);
    // Business processing: some common operations
    switch (ev.status) {
      case 200:
        // Business-level error handling, the following is to assume that Restful has a unified output format (meaning that there is a corresponding data format regardless of success or failure).
        // For example, response content:
        // Error content: {status: 1, msg:'Illegal parameter'}
        // Correct content: {status: 0, response: {}}
        // Then the following code snippets can be directly applied
        // if (ev instanceof HttpResponse) {
        // const body = ev.body;
        // if (body && body.status !== 0) {
        // this.injector.get(NzMessageService).error(body.msg);
        // // Continue to throw errors to interrupt all subsequent Pipe and subscribe operations, therefore:
        // // this.http.get('/').subscribe() will not trigger
        // return throwError({});
        // } else {
        // // Re-modify the content of `body` to the content of `response`. For most scenarios, you no longer need to care about business status codes
        // return of(new HttpResponse(Object.assign(ev, {body: body.response })));
        // // or still maintain the complete format
        // return of(ev);
        // }
        // }
        break;
      case 401:
        if (this.refreshTokenType === 're-request') {
          return this.tryRefreshToken(ev, req, next);
        }
        this.toLogin();
        break;
      case 403:
      case 404:
      case 500:
        this.goTo(`/exception/${ev.status}`);
        break;
      default:
        if (ev instanceof HttpErrorResponse) {
          console.warn(
            'Unknown errors, mostly caused by the backend does not support cross-domain CORS or invalid configuration, please refer to https://ng-alain.com/docs/server to solve cross-domain issues',
            ev,
          );
        }
        break;
    }
    if (ev instanceof HttpErrorResponse) {
      return throwError(ev);
    } else {
      return of(ev);
    }
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Unified server prefix
    let url = req.url;
    if (!url.startsWith('https://') && !url.startsWith('http://')) {
      url = environment.SERVER_URL + url;
    }

    const newReq = req.clone({ url });
    return next.handle(newReq).pipe(
      mergeMap((ev) => {
        // Allow unified handling of request errors
        if (ev instanceof HttpResponseBase) {
          return this.handleData(ev, newReq, next);
        }
        // If everything is normal, follow up operations
        return of(ev);
      }),
      catchError((err: HttpErrorResponse) => this.handleData(err, newReq, next)),
    );
  }
}
