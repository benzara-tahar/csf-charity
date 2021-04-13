import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from 'src/app/shared/shared.module';
import { TenantEffects } from './effects';
import { reducer } from './reducer';

@NgModule({
  declarations: [],
  imports: [SharedModule, StoreModule.forFeature('tenant', reducer), EffectsModule.forFeature([TenantEffects])],
  providers: [TenantEffects],
})
export class TenantStoreModule {}
