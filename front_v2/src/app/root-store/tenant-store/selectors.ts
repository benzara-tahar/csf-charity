import { createFeatureSelector, createSelector, MemoizedSelector } from '@ngrx/store';

import { Tenant } from '@shared';
import { State as TenantState } from './state';

export const getIsLoading = (state: TenantState) => state.isLoading;
export const getCurrentTenant = (state: TenantState) => state.currentTenant;

export const selectTenantState: MemoizedSelector<object, TenantState> = createFeatureSelector<TenantState>('tenant');

export const selectIsLoading: MemoizedSelector<object, boolean> = createSelector(selectTenantState, getIsLoading);

export const selectCurrentTenant: MemoizedSelector<object, Tenant> = createSelector(selectTenantState, getCurrentTenant);
