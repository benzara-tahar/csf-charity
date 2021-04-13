import * as AuthActions from './actions';
import { initialState, State } from './state';
import { createReducer, on, Action } from '@ngrx/store';

const AuthReducer = createReducer(
  initialState,
  // LOGIN CASES
  on(AuthActions.loginRequest, (state) => {
    return {
      ...state,
      isLoggedIn: false,
    };
  }),
  on(AuthActions.loginRequestSuccess, (state) => {
    return {
      ...state,
    };
  }),
  on(AuthActions.completeLogin, (state) => {
    return {
      ...state,
      isLoading: true,
    };
  }),
  on(AuthActions.loginStateChanged, (state, { newStatus }) => {
    return {
      ...state,
      isLoggedIn: newStatus,
    };
  }),
  on(AuthActions.loginRequestFail, (state) => {
    return {
      ...state,
    };
  }),
  on(AuthActions.loginSuccess, (state) => {
    return {
      ...state,
      isLoggedIn: true,
      isLoading: false,
    };
  }),
  on(AuthActions.setCurrentLanguage, (state, { language }) => {
    return {
      ...state,
      currentLanguage: language,
    };
  })
);

export function authReducer(state: State | undefined, action: Action) {
  return AuthReducer(state, action);
}
