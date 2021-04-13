import { Component, OnInit, ViewChild } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { _HttpClient } from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { NzSafeAny } from 'ng-zorro-antd/core/types';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { EmbededDatabase } from 'src/app/shared/db/embededDatabase';
import { Tenant } from '@shared';
import { UserService } from '../user.service';
import { User } from 'src/app/routes/admin/admins/models/user';

@UntilDestroy()
@Component({
  selector: 'app-user-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.less'],
})
export class UserEditComponent implements OnInit {
  params: any;
  tenantId: any;
  mode = 'new'; // 'new' or 'edit';
  record: any = {};
  form: FormGroup;
  error = '';
  count = 0;
  visible = false;
  status = 'pool';
  progress = 0;
  passwordProgressMap = {
    ok: 'success',
    pass: 'normal',
    pool: 'exception',
  };

  supportedRoles: any[] = [];
  selectedRole: any;
  tenants: Tenant[];

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
    public userService: UserService,
  ) {
    this.supportedRoles = EmbededDatabase.roles.filter((r) => r.name != EmbededDatabase.adminRole);
  }

  ngOnInit(): void {
    if (this.mode == 'edit') {
      this.form = this.fb.group({
        userName: [{ value: this.record.userName, disabled: true }, [Validators.required]],
        email: [this.record.email, [Validators.required, Validators.email]],
        role: [
          {
            value: this.supportedRoles.find((r) => r.name == this.record.role.name),
            disabled: true,
          },
          [Validators.required],
        ],
        tenantId: [this.record.tenantId, [Validators.required]],
      });
    } else {
      this.form = this.fb.group({
        userName: [null, [Validators.required]],
        password: [
          null,
          [
            Validators.required,
            Validators.minLength(6),
            Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*d)(?=.*[#$^+=!*()@%&]).{8,}$'),
            UserEditComponent.checkPassword.bind(this),
          ],
        ],
        confirm: [null, [Validators.required, Validators.minLength(6), UserEditComponent.passwordEquar]],
        role: [null, [Validators.required]],
        email: [null, [Validators.required, Validators.email]],
        tenantId: [this.tenantId, [Validators.required]],
      });
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
      this.updateTenantUser();
    } else {
      this.addTenantUser();
    }
  }

  addTenantUser() {
    const adminToRegister = {
      userName: this.form.controls.userName.value,
      password: this.form.controls.password.value,
      email: this.form.controls.email.value,
      role: this.form.controls.role.value.name,
      tenantId: this.form.controls.tenantId.value,
    } as User;
    this.userService
      .create(adminToRegister)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('admins.create-success-message'),
        );
        this.modal.close(true);
      });
  }

  updateTenantUser() {
    const adminToUpdate = {
      userName: this.form.controls.userName.value,
      email: this.form.controls.email.value,
    } as User;
    this.userService
      .update(adminToUpdate)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('admins.update-success-message'),
        );
        this.modal.close(true);
      });
  }

  compareObjects(o1: any, o2: any): boolean {
    if (o1 && o2) {
      return o1.name === o2.name;
    }
  }
}
