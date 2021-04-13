import { Injectable } from '@angular/core';
import { Action, select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';

import * as selectors from './selectors';
import { State } from './state';

@Injectable({
  providedIn: 'root',
})
export class AuthFacade {
  isLoading$: Observable<boolean>;
  isLoggedIn$: Observable<boolean>;
  currentLanguage$: Observable<string>;

  constructor(private store: Store<State>) {
    this.isLoading$ = this.store.pipe(select(selectors.selectAuthIsLoading));
    this.isLoggedIn$ = this.store.pipe(select(selectors.selectAuthIsLoggedIn));
    this.currentLanguage$ = this.store.pipe(select(selectors.selectCurrentLanguage));
  }

  // Wrapper function for dispatching actions to be called
  // inside components insetad of abstracting each action
  dispatch(action: Action) {
    this.store.dispatch(action);
  }
}
