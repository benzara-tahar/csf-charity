import { Tenant } from '@shared';

export interface State {
  isLoading: boolean;
  currentTenant: Tenant;
}

export const initialState: State = {
  isLoading: false,
  currentTenant: {} as Tenant,
};
