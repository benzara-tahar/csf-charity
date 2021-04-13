import { CommonModule } from '@angular/common';
import { InjectionToken, NgModule } from '@angular/core';

import { AuthStoreModule } from '@app/store/auth-store';
import * as fromAuth from '@app/store/auth-store';
import { State } from '@app/store/root-state';
import { MergedRouterStateSerializer } from '@app/store/router-store/router-state-serializer';
import { storageMetaReducer } from '@app/store/storage.metareducer';
import { LocalStorageService } from '@core';
import { environment } from '@env/environment';
import { EffectsModule } from '@ngrx/effects';
import * as fromRouter from '@ngrx/router-store';
import { RouterStateSerializer, StoreRouterConnectingModule } from '@ngrx/router-store';
import { Action, ActionReducerMap, MetaReducer, StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import * as fromTenant from './tenant-store';
import { TenantStoreModule } from './tenant-store';

// Factory for storage meta-reducer configuration
export function getStorageMetaReducers(
  saveKeys: string[],
  localStorageKey: string,
  storageService: LocalStorageService,
): MetaReducer<any>[] {
  return [storageMetaReducer(saveKeys, localStorageKey, storageService)];
}

export const ROOT_REDUCERS = new InjectionToken<ActionReducerMap<State, Action>>('Root reducers token', {
  factory: () => ({
    auth: fromAuth.authReducer,
    router: fromRouter.routerReducer,
    tenant: fromTenant.reducer,
  }),
});

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    StoreModule.forRoot(ROOT_REDUCERS, {
      runtimeChecks: {
        strictStateImmutability: true,
        strictActionImmutability: true,
      },
    }),
    EffectsModule.forRoot([]),
    AuthStoreModule,
    TenantStoreModule,
    StoreRouterConnectingModule.forRoot(),
    StoreDevtoolsModule.instrument({
      maxAge: 10,
    }),
    !environment.production ? StoreDevtoolsModule.instrument({ name: 'Charity IOT' }) : [],
    /*
    disabled for now
    { provide: ROOT_STORAGE_KEYS, useValue: [''] },
    { provide: ROOT_LOCAL_STORAGE_KEY, useValue: '__app_storage__' },
    {
      provide: USER_PROVIDED_META_REDUCERS,
      deps: [ROOT_STORAGE_KEYS, ROOT_LOCAL_STORAGE_KEY, LocalStorageService],
      useFactory: getStorageMetaReducers,
    },*/
  ],

  providers: [
    {
      provide: RouterStateSerializer,
      useClass: MergedRouterStateSerializer,
    },
  ],
})
export class RootStoreModule {}
