import { RouteReducerState } from '@app/store/router-store/state';
import { AuthStoreState } from './auth-store';
import { TenantStoreState } from './tenant-store';

export interface State {
  auth: AuthStoreState.State;
  tenant: TenantStoreState.State;
  router: RouteReducerState;
}
