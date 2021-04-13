import {
  createSelector,
  MemoizedSelector,
  createFeatureSelector,
} from '@ngrx/store';

import { State as AuthState } from './state';

export const getIsLoggedIn = (state: AuthState): any => state.isLoggedIn;
export const getIsLoading = (state: AuthState): any => state.isLoading;
export const getCurrentLanguage = (state: AuthState): any =>
  state.currentLanguage;

export const selectAuthState: MemoizedSelector<
  object,
  AuthState
> = createFeatureSelector<AuthState>('auth');

export const selectAuthIsLoggedIn: MemoizedSelector<
  object,
  boolean
> = createSelector(selectAuthState, getIsLoggedIn);

export const selectAuthIsLoading: MemoizedSelector<
  object,
  boolean
> = createSelector(selectAuthState, getIsLoading);

export const selectCurrentLanguage: MemoizedSelector<
  object,
  string
> = createSelector(selectAuthState, getCurrentLanguage);
