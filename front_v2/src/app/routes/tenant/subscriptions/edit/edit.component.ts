import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SettingsService, _HttpClient } from '@delon/theme';
import { TranslateService } from '@ngx-translate/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { SubscriptionService } from '../subscription.service';
import { Subscription } from '../models/subscription';
import { EmbededDatabase } from 'src/app/shared/db/embededDatabase';

const urlRegex = 'https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,}';

@UntilDestroy()
@Component({
  selector: 'subscription-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.less']
})
export class SubscriptionEditComponent implements OnInit {
  params: any;
  tenantId: any;
  mode = 'new'; // 'new' or 'edit';
  record: any = {};
  form: FormGroup;
  error = '';
  webhookDefinitions: any[];
  distanceUnits: any[];

  constructor(
    private modal: NzModalRef,
    public http: _HttpClient,
    public fb: FormBuilder,
    public msg: NzMessageService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public subscriptionService: SubscriptionService,
    public settingService: SettingsService
  ) {
    this.distanceUnits = EmbededDatabase.distanceUnits;
    this.getWebhookDefinitions();
  }

  ngOnInit(): void {
    if (this.mode == 'edit') {
      this.form = this.fb.group({
        name: [this.record.name, [Validators.required]],
        webhookUri: [this.record.webhookUri, [Validators.required, Validators.pattern(urlRegex)]],
        secret: [this.record.secret, [Validators.required]],
        isActive: [this.record.isActive, [Validators.required]],
        batchInterval: [this.record.batchInterval, [Validators.required, Validators.min(180)]],
        webhooks: [[...this.record.webhooks], [Validators.required]],
        maxAttemptBeforeRejection: [this.record.maxAttemptBeforeRejection, [Validators.required]],
        preferredDistanceUnit: [this.record.preferredDistanceUnit, [Validators.required]],
      });
    } else {
      this.form = this.fb.group({
        name: [null, [Validators.required]],
        webhookUri: ['https://', [Validators.required, Validators.pattern(urlRegex)]],
        secret: [null, [Validators.required]],
        isActive: [false, [Validators.required]],
        batchInterval: [180, [Validators.required, Validators.min(180)]],
        webhooks: [[], [Validators.required]],
        maxAttemptBeforeRejection: [0, [Validators.required]],
        preferredDistanceUnit: [0, [Validators.required]]
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
      this.updateTenantSubscription();
    } else {
      this.addTenantSubscription();
    }
  }

  addTenantSubscription() {
    const subscription = {
      name: this.form.controls.name.value,
      webhookUri: this.form.controls.webhookUri.value,
      secret: this.form.controls.secret.value,
      isActive: this.form.controls.isActive.value,
      batchInterval: this.form.controls.batchInterval.value,
      maxAttemptBeforeRejection: this.form.controls.maxAttemptBeforeRejection.value,
      tenantId: this.settingService.user.tenantId,
      webhooks: this.form.controls.webhooks.value,
      preferredDistanceUnit: this.form.controls.preferredDistanceUnit.value
    } as Subscription;
    this.subscriptionService
      .create(subscription)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('subscriptions.create-success-message'),
        );
        this.modal.close(true);
      });
  }

  updateTenantSubscription() {
    const subscription = {
      id: this.record.id,
      name: this.form.controls.name.value,
      webhookUri: this.form.controls.webhookUri.value,
      secret: this.form.controls.secret.value,
      isActive: this.form.controls.isActive.value,
      batchInterval: this.form.controls.batchInterval.value,
      maxAttemptBeforeRejection: this.form.controls.maxAttemptBeforeRejection.value,
      tenantId: this.record.tenantId,
      webhooks: this.form.controls.webhooks.value,
      preferredDistanceUnit: this.form.controls.preferredDistanceUnit.value
    } as Subscription;
    this.subscriptionService
      .update(subscription)
      .pipe(untilDestroyed(this))
      .subscribe(() => {
        this.notificationService.success(
          this.transalteService.instant('common.success'),
          this.transalteService.instant('subscriptions.update-success-message'),
        );
        this.modal.close(true);
      });
  }

  getWebhookDefinitions() {
    this.subscriptionService
      .getWebhookDefinitions()
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.webhookDefinitions = data;
      });
  }

  generateRandomSecret() {
    const secret = `whs_${Math.random().toString(36).substring(2, 15) + Math.random().toString(36).substring(2, 15)}`;
    this.form.controls.secret.patchValue(secret, { onlySelf: true });
  }

}

