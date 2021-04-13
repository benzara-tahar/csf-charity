import { Component, OnInit, ViewChild } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { _HttpClient } from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { EmbededDatabase } from 'src/app/shared/db/embededDatabase';
import { TenantService } from 'src/app/services/tenant.service';
import { IotProvider, Tenant } from '@shared';
@UntilDestroy()
@Component({
  selector: 'app-tenants-edit',
  templateUrl: './edit.component.html',
})
export class AdminTenantsEditComponent implements OnInit {
  mode = 'new'; // 'new' or 'edit';
  record: any = null;
  form: FormGroup;
  error = '';
  count = 0;
  supportedIoTProviders: IotProvider[] = [];

  constructor(
    private modal: NzModalRef,
    public http: _HttpClient,
    public fb: FormBuilder,
    public msg: NzMessageService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public tenantService: TenantService,
  ) {
    this.supportedIoTProviders = EmbededDatabase.supportedIoTProviders;
  }

  ngOnInit(): void {
    if (this.record) {
      this.mode = 'edit';
      this.form = this.fb.group({
        name: [this.record.name, [Validators.required]],
        iotProviders: [[...this.record.supportedIoTProviders], [Validators.required]],
      });
    } else {
      this.form = this.fb.group({
        name: [null, [Validators.required]],
        iotProviders: [[], [Validators.required]],
      });
    }
  }

  compareObjects(o1: any, o2: any): boolean {
    if (o1 && o2) {
      return o1.name === o2.name;
    }
  }

  close() {
    this.modal.destroy();
  }

  submit(): void {
    this.error = '';
    Object.keys(this.form.controls).forEach((key) => {
      this.form.controls[key].markAsDirty();
      this.form.controls[key].updateValueAndValidity();
    });
    if (this.form.invalid) {
      return;
    }
    if (this.mode == 'edit') {
      this.updateTenant();
    } else {
      this.createTenant();
    }
  }

  createTenant() {
    const newTenant = {
      name: this.form.controls.name.value,
      supportedIoTProviders: this.form.controls.iotProviders.value,
    } as Tenant;
    this.tenantService
      .create(newTenant)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('tenants.create-success-message'),
        );
        this.modal.close(true);
      });
  }

  updateTenant() {
    const updatedTenant = {
      id: this.record.id,
      name: this.form.controls.name.value,
      supportedIoTProviders: this.form.controls.iotProviders.value,
    } as Tenant;
    this.tenantService
      .update(updatedTenant)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('tenants.update-success-message'),
        );
        this.modal.close(true);
      });
  }
}
