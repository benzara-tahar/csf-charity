import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper, SettingsService, _HttpClient } from '@delon/theme';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { User } from '../../admin/admins/models/user';
import { DistanceUnits } from '../../tenant/subscriptions/models/distanceUnits';
import { Subscription } from '../../tenant/subscriptions/models/subscription';
import { SubscriptionService } from '../../tenant/subscriptions/subscription.service';
import { Tenant } from '@shared';

import { SubscriptionDetailsComponent } from './subscription-details/subscription-details.component';
import { TenantService } from 'src/app/services/tenant.service';

@UntilDestroy()
@Component({
  selector: 'app-admin-subscriptions',
  templateUrl: './subscriptions.component.html',
  styles: [],
})
export class SubscriptionsComponent implements OnInit {
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: { i18n: 'subscriptions.subscription-name' }, type: '', index: 'name' },
    { title: { i18n: 'subscriptions.subscription-tenant' }, type: '', index: 'tenant.name' },
    { title: { i18n: 'subscriptions.subscription.is-active' }, type: '', index: 'isActive', render: 'isActive' },
    {
      title: { i18n: 'subscriptions.subscription.distanceUnit' },
      type: '',
      index: 'preferredDistanceUnit',
      render: 'preferredDistanceUnit',
    },
    { title: { i18n: 'subscriptions.subscription-webhook-uri' }, type: '', index: 'webhookUri' },
    {
      title: '',
      buttons: [
        {
          icon: 'eye',
          type: 'drawer',
          drawer: {
            title: '',
            component: SubscriptionDetailsComponent,
          },
        },
        {
          icon: 'delete',
          type: 'modal',
          click: (data) => this.showDeleteConfirmModal(data as Subscription),
          className: 'danger',
        },
      ],
    },
  ];
  data: Subscription[];
  tenants: Tenant[];
  currentUser: User;
  selectedTenant: string = 'all';
  DistanceUnits: typeof DistanceUnits = DistanceUnits;

  constructor(
    private modal: ModalHelper,
    public modalService: NzModalService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public subscriptionService: SubscriptionService,
    public tenantService: TenantService,
    private settings: SettingsService,
  ) {
    this.currentUser = this.settings.user as User;
  }

  ngOnInit() {
    this.getTenants();
    this.getSubscriptions();
  }

  showDeleteConfirmModal(user: Subscription): void {
    this.modalService.confirm({
      nzTitle: this.transalteService.instant('subscriptions.delete-subscription'),
      nzOkText: this.transalteService.instant('common.yes'),
      nzOkType: 'danger',
      nzOnOk: () => this.deleteSubscription(user.id),
      nzCancelText: this.transalteService.instant('common.no'),
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  getSubscriptions() {
    this.subscriptionService
      .filterSubscriptions(this.selectedTenant)
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.data = data;
        this.st.reload();
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

  deleteSubscription(id: string) {
    this.subscriptionService
      .delete(id)
      .pipe(untilDestroyed(this))
      .subscribe(
        () => {
          this.notificationService.success(
            this.transalteService.instant('common.success'),
            this.transalteService.instant('subscriptions.delete-success-message'),
          );
          const index = this.data.findIndex((u) => u.id == id);
          if (index > -1) {
            this.data.splice(index, 1);
            this.st.reload();
          }
        },
        (err) => {
          console.log(err);
        },
      );
  }
}
