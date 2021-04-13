import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReferencialsComponent } from './referencials.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    SharedModule
  ],
  declarations: [ReferencialsComponent]
})
export class ReferencialsModule { }
