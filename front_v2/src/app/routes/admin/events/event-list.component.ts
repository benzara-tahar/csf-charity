import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper, _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { WebhookEvent, WebhookEventStatus } from '../../../shared/model/webhook-event';
import { WebhookEventComponent } from '../../../shared/components/webhook-event/webhook-event.component';
import { Observable } from 'rxjs';

import { TenantService } from 'src/app/services/tenant.service';
import { Tenant } from '@shared';
@UntilDestroy()
@Component({
  selector: 'app-admin-events',
  templateUrl: './event-list.component.html',
})
export class AdminEventListComponent implements OnInit {
  @ViewChild('st', { static: true }) st: STComponent;
  columns: STColumn[] = [
    {
      title: { i18n: 'webhook.name' },
      type: '',
      index: 'name',
      render: 'custom-name',
      filter: {
        menus: [
          { text: 'device.telemetry', value: 'device.telemetry' },
          { text: 'device.installed', value: 'device.installed' },
          { text: 'device.uninstalled', value: 'device.uninstalled' },
          { text: 'device.connected', value: 'device.connected' },
          { text: 'device.disconnected', value: 'device.disconnected' },
          { text: 'device.online', value: 'device.online' },
          { text: 'device.offline', value: 'device.offline' },
          { text: 'trunk.locked', value: 'trunk.locked' },
          { text: 'trunk.unlocked', value: 'trunk.unlocked' },
        ],
        clearText: this.translate.instant('reset'),
      },
    },
    { title: { i18n: 'webhook.date' }, type: 'date', index: 'enqueuedAtUtc', sort: true },
    { title: { i18n: 'webhook.url' }, render: 'custom-url', index: 'url' },
    {
      title: { i18n: 'webhook.status' },
      type: '',
      render: 'custom-status',
      index: 'status',
      filter: {
        menus: [
          { text: this.translate.instant('webhook.status.Pending'), value: WebhookEventStatus.Pending },
          { text: this.translate.instant('webhook.status.DeliveryFailed'), value: WebhookEventStatus.DeliveryFailed },
          { text: this.translate.instant('webhook.status.DeliverySucceeded'), value: WebhookEventStatus.DeliverySucceeded },
          { text: this.translate.instant('webhook.status.Rejected'), value: WebhookEventStatus.Rejected },
        ],
        clearText: this.translate.instant('reset'),
      },
    },
    { title: { i18n: 'webhook.retryCount' }, type: '', index: 'retryCount', sort: true },
    {
      title: '',
      buttons: [
        {
          icon: 'eye',
          type: 'drawer',
          drawer: {
            title: '',
            component: WebhookEventComponent,
          },
        },
      ],
    },
  ];
  data = null;
  tenants$: Observable<Tenant[]>;
  params: any = {};
  constructor(
    public modalService: NzModalService,
    public tenantService: TenantService,
    public notificationService: NzNotificationService,
    public translate: TranslateService,
  ) {
    this.data = `${environment.urls.main}/api/admin/events`;
    this.tenants$ = this.tenantService.getAll();
  }

  ngOnInit() {}

  filter() {
    if (this.params.tenantId) {
      this.st.req.params = this.params;
    }

    this.st.load(1);
  }
  reset() {
    this.st.req.params = {};
    this.st.reset();
  }
}
