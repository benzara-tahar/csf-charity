import { Data, Params } from '@angular/router';
import { RouterReducerState } from '@ngrx/router-store';

export interface RouterState {
  url: string;
  queryParams: Params;
  params: Params;
  data: Data;
}
export type RouteReducerState = RouterReducerState<RouterState>;
