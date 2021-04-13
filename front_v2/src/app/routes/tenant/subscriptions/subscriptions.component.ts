import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper, SettingsService, _HttpClient } from '@delon/theme';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { User } from '../../admin/admins/models/user';
import { SubscriptionEditComponent } from './edit/edit.component';
import { DistanceUnits } from './models/distanceUnits';
import { Subscription } from './models/subscription';
import { SubscriptionService } from './subscription.service';

@UntilDestroy()

@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions.component.html',
  styles: [
  ]
})
export class SubscriptionsComponent implements OnInit {
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: { i18n: 'subscriptions.subscription-name' }, type: '', index: 'name' },
    { title: { i18n: 'subscriptions.subscription.is-active' }, type: '', index: 'isActive', render: 'isActive' },
    { title: { i18n: 'subscriptions.subscription.distanceUnit' }, type: '', index: 'preferredDistanceUnit', render: 'preferredDistanceUnit' },
    { title: { i18n: 'subscriptions.subscription-webhook-uri' }, type: '', index: 'webhookUri' },
    {
      title: '',
      buttons: [
        {
          icon: 'edit',
          click: (data) => this.showEditModal(data as Subscription),
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
  currentUser: User;
  DistanceUnits: typeof DistanceUnits = DistanceUnits;

  constructor(
    private modal: ModalHelper,
    public modalService: NzModalService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public subscriptionService: SubscriptionService,
    private settings: SettingsService,
  ) {
    this.currentUser = this.settings.user as User;
  }

  ngOnInit() {
    this.getTenantSubscriptions();
  }

  showNewModal() {
    this.modal.createStatic(SubscriptionEditComponent, { tenantId: this.currentUser.tenantId }).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getTenantSubscriptions();
      }
    });
  }

  showEditModal(data: Subscription) {
    const params = { record: data, mode: 'edit' };
    this.modal.createStatic(SubscriptionEditComponent, params).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getTenantSubscriptions();
      }
    });
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

  getTenantSubscriptions() {
    this.subscriptionService
      .getByTenant()
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.data = data;
        this.st.reload();
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


