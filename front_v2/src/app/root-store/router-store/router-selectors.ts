import { createFeatureSelector, createSelector, MemoizedSelector } from '@ngrx/store';
import { RouteReducerState  } from '@app/store/router-store/state';
import { Params } from '@angular/router';

export const selectRouterState: MemoizedSelector<object,
  RouteReducerState> = createFeatureSelector<RouteReducerState>('router');


export const selectQueryParams =
  createSelector(selectRouterState, (state) => state.state.params);


export const selectQueryParam = (param: string) =>
  createSelector(selectQueryParams, (params: Params) => params && params[param]);
