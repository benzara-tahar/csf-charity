import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { _HttpClient } from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { NzSafeAny } from 'ng-zorro-antd/core/types';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { Tenant } from '@shared';
import { IoTAccessAccount } from '../models/access-account';
import { AccessAccountService } from '../access-account.service';
import { EmbededDatabase } from 'src/app/shared/db/embededDatabase';
import differenceInCalendarDays from 'date-fns/differenceInCalendarDays';
import { TenantService } from 'src/app/services/tenant.service';

@UntilDestroy()
@Component({
  selector: 'app-access-account-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.less'],
})
export class AccessAccountEditComponent implements OnInit {
  params: any;
  mode = 'new'; // 'new' or 'edit';
  record: any = {};
  form: FormGroup;
  error = '';
  visible = false;
  status = 'pool';
  progress = 0;
  passwordProgressMap = {
    ok: 'success',
    pass: 'normal',
    pool: 'exception',
  };
  supportedPermissions: any[] = [];
  tenants: Tenant[];
  today = new Date();

  static checkPassword(control: FormControl): NzSafeAny {
    if (!control) {
      return null;
    }
    const self: any = this;
    self.visible = !!control.value;
    if (control.value && control.value.length > 9) {
      self.status = 'ok';
    } else if (control.value && control.value.length > 5) {
      self.status = 'pass';
    } else {
      self.status = 'pool';
    }

    if (self.visible) {
      self.progress = control.value.length * 10 > 100 ? 100 : control.value.length * 10;
    }
  }

  static passwordEquar(control: FormControl): { equar: boolean } | null {
    if (!control || !control.parent) {
      return null;
    }
    if (control.value !== control.parent.get('password')!.value) {
      return { equar: true };
    }
    return null;
  }

  constructor(
    private modal: NzModalRef,
    public http: _HttpClient,
    public fb: FormBuilder,
    public msg: NzMessageService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public accessAccountService: AccessAccountService,
    public tenantService: TenantService,
  ) {
    this.supportedPermissions = EmbededDatabase.supportedPermissions;
    this.getTenants();
  }

  ngOnInit(): void {
    if (this.mode == 'edit') {
      this.form = this.fb.group({
        userName: [{ value: this.record.userName, disabled: true }, [Validators.required]],
        tenantId: [{ value: this.record.tenant.id, disabled: true }],
        permissions: [this.record.permissions, [Validators.required]],
        validTo: [this.record.validTo != null ? new Date(this.record.validTo) : null],
      });
    } else {
      this.form = this.fb.group({
        userName: [null, [Validators.required]],
        password: [
          null,
          [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*+]).{8,}$'),
            AccessAccountEditComponent.checkPassword.bind(this),
          ],
        ],
        confirm: [null, [Validators.required, Validators.minLength(8), AccessAccountEditComponent.passwordEquar]],
        tenantId: [null, [Validators.required]],
        permissions: [[], [Validators.required]],
        validTo: [null],
      });
    }
  }

  close() {
    this.modal.destroy();
  }

  submit(): void {
    this.error = '';
    if (this.form.invalid) {
      return;
    }
    if (this.mode == 'edit') {
      this.updateAccessAccounts();
    } else {
      this.createAccessAccount();
    }
  }

  createAccessAccount() {
    const accessAccount = {
      userName: this.form.controls.userName.value,
      password: this.form.controls.password.value,
      tenantId: this.form.controls.tenantId.value,
      permissions: this.form.controls.permissions.value,
      validTo: this.form.controls.validTo.value,
    } as IoTAccessAccount;
    this.accessAccountService
      .create(accessAccount)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('access-accounts.create-success-message'),
        );
        this.modal.close(true);
      });
  }

  updateAccessAccounts() {
    const accessAccountToUpdate = {
      id: this.record.id,
      permissions: this.form.controls.permissions.value,
      validTo: this.form.controls.validTo.value,
    } as IoTAccessAccount;
    this.accessAccountService
      .update(accessAccountToUpdate)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('access-accounts.update-success-message'),
        );
        this.modal.close(true);
      });
  }

  getTenants() {
    this.tenantService
      .getAll()
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.tenants = data;
      });
  }

  disabledDate = (current: Date): boolean => {
    return differenceInCalendarDays(current, this.today) < 0;
  };
}
