import { createAction, props } from '@ngrx/store';

// LOGIN
export const loginRequest = createAction('[Auth] Login Request');

export const loginRequestSuccess = createAction('[Auth] Login request success');

export const loginRequestFail = createAction('[Auth] Login request Fail');

export const completeLogin = createAction('[Auth] Complete login');

export const loginStateChanged = createAction(
  '[Auth] Login status changed',
  props<{ newStatus: boolean }>()
);

export const loginSuccess = createAction('[Auth] Login Success');

export const loginError = createAction(
  '[Auth] Login Error',
  props<{ error: any }>()
);

// LOGOUT
export const logoutRequest = createAction('[Auth] logout request');

export const logoutRequestSuccess = createAction(
  '[Auth] logout request success'
);

export const logoutRequestFail = createAction(
  '[Auth] logout request fail',
  props<{ error: any }>()
);

export const completeLogout = createAction('[Auth] complete logout');

// Language and localization actions
export const setCurrentLanguage = createAction(
  '[Auth] Set Current Language',
  props<{ language: string }>()
);
