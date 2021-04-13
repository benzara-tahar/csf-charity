import { Injectable } from '@angular/core';
import { AuthService } from '@core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY, of } from 'rxjs';
import { catchError, map, mergeMapTo, switchMap, tap } from 'rxjs/operators';
import * as AuthActions from './actions';

@Injectable()
export class AuthEffects {
  constructor(private actions$: Actions, private authService: AuthService) {}

  // LOGIN
  loginRequest$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loginRequest),
      switchMap(() => {
        return this.authService.login('', '').pipe(
          map(() => AuthActions.loginRequestSuccess()),
          catchError((error) => of(AuthActions.loginRequestFail())),
        );
      }),
    ),
  );

  completeLogin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.completeLogin),
      switchMap(() => {
        return this.authService.authorizeCallback().pipe(
          map(() => AuthActions.loginSuccess()),
          catchError((error) => of(AuthActions.loginError({ error }))),
        );
      }),
    ),
  );

  // LOGOUT
  logoutRequest$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logoutRequest),
      switchMap(() => {
        return this.authService.logout().pipe(
          map(() => AuthActions.logoutRequestSuccess()),
          catchError((error) => of(AuthActions.logoutRequestFail({ error }))),
        );
      }),
    ),
  );

  failure$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginRequestFail),
        tap((x) => console.error(x)),
      ),
    // mergeMapTo(EMPTY)
  );
}
