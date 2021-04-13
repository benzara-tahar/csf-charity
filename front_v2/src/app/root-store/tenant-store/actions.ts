import { createAction, props } from '@ngrx/store';
import { Tenant } from '@shared';

export const loadTenantInfo = createAction('[Tenant] Load Tenant Info', props<{ id: string }>());

export const loadTenantInfoSuccess = createAction('[Tenant] Load Tenant Info Success', props<{ tenant: Tenant }>());

export const updateTenant = createAction('[Tenant] Update Tenant', props<{ tenant: Tenant }>());

export const updateTenantSuccess = createAction('[Tenant] Update Tenant Success', props<{ tenant: Tenant }>());

export const failure = createAction('[Tenant] Failure', props<{ error: any }>());
