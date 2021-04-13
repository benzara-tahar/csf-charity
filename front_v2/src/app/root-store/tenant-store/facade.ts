import { Injectable } from '@angular/core';
import { Store, Action, select } from '@ngrx/store';
import { State } from './state';
import { selectIsLoading, selectCurrentTenant } from './selectors';

@Injectable({
  providedIn: 'root',
})
export class TenantFacade {
  isLoading$ = this.store.pipe(select(selectIsLoading));
  currentTenant$ = this.store.pipe(select(selectCurrentTenant));

  constructor(private store: Store<State>) {}

  dispatch(action: Action) {
    this.store.dispatch(action);
  }
}
