import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingModule } from './setting/setting.module';
import { ReferentialModule } from './referential/referential.module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    SettingModule,
    ReferentialModule
  ]
})
export class AdminModule { }
