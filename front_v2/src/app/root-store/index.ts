import { InjectionToken } from '@angular/core';
import * as RootStoreState from './root-state';
import { RootStoreModule } from './root-store.module';

// token for the state keys.
export const ROOT_STORAGE_KEYS = new InjectionToken<string[]>('Store Keys');

// token for the localStorage key.
export const ROOT_LOCAL_STORAGE_KEY = new InjectionToken<string[]>('App localStorage');

export * from './auth-store';
export * from './tenant-store';

export { RootStoreState, RootStoreModule };
