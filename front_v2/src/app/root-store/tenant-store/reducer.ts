import { initialState, State } from './state';
import { createReducer, on } from '@ngrx/store';
import { loadTenantInfoSuccess, updateTenantSuccess } from './actions';

export const reducer = createReducer<State>(
  initialState,
  on(loadTenantInfoSuccess, (state, { tenant }) => ({
    ...state,
    isLoading: true,
    currentTenant: tenant,
  })),
  on(updateTenantSuccess, (state, { tenant }) => ({
    ...state,
    currentTenant: tenant,
  })),
);
