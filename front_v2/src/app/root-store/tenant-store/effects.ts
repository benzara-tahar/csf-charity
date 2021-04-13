import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { TranslateService } from '@ngx-translate/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { empty, EMPTY, Observable, of } from 'rxjs';
import { catchError, mergeMap, mergeMapTo, pluck, switchMap, tap } from 'rxjs/operators';
import { TenantService } from 'src/app/services/tenant.service';
import * as TenantActions from './actions';

@Injectable()
export class TenantEffects {
  constructor(
    private actions$: Actions,
    private tenantService: TenantService,
    private message: NzMessageService,
    private translate: TranslateService,
  ) {}
  loadTenantInfo$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TenantActions.loadTenantInfo),
      pluck('id'),
      switchMap((id) =>
        this.tenantService.get(id).pipe(
          mergeMap((tenant) => [
            TenantActions.loadTenantInfoSuccess({
              tenant,
            }),
          ]),
          catchError((error) => of(TenantActions.failure({ error }))),
        ),
      ),
    ),
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TenantActions.updateTenant),
      pluck('tenant'),
      switchMap((tenant) =>
        this.tenantService.update(tenant).pipe(
          mergeMap(() => [TenantActions.updateTenantSuccess({ tenant })]),
          catchError((error) => of(TenantActions.failure({ error }))),
        ),
      ),
    ),
  );

  failure$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TenantActions.failure),
      tap((x) => this.handleError(x)),
    ),
  );

  handleError({ error }): void {
    console.log(error);
    const localizedError = 'error.unexepctedError';
    this.message.error(this.translate.instant(localizedError));
  }
}
