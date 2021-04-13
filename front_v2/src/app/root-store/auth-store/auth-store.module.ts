import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from 'src/app/shared/shared.module';
import { AuthEffects } from './effects';
import { authReducer } from './reducer';

@NgModule({
  declarations: [],
  imports: [SharedModule, StoreModule.forFeature('auth', authReducer), EffectsModule.forFeature([AuthEffects])],
  providers: [AuthEffects],
})
export class AuthStoreModule {}
