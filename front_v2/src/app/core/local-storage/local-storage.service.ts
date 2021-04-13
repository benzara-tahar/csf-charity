import { Injectable } from '@angular/core';
import { Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';


@Injectable({ providedIn: 'root' })
export class LocalStorageService {
  constructor(@Inject(PLATFORM_ID) protected platformId: any) {}

  reviver(key, value) {
    const dateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:[0-9.]*Z$/;
    if (typeof value === 'string' && dateFormat.test(value)) {
      return new Date(value);
    }
    return value;
  }

  setSavedState(state: any, localStorageKey: string) {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(localStorageKey, JSON.stringify(state));
    }
  }

  getSavedState(localStorageKey: string): any {
    if (isPlatformBrowser(this.platformId)) {
      return JSON.parse(localStorage.getItem(localStorageKey), this.reviver);
    } else {
      return {};
    }
  }
}
