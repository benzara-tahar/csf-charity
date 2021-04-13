import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper, SettingsService, _HttpClient } from '@delon/theme';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { User } from '../admins/models/user';
import { AccessAccountService } from './access-account.service';
import { AccessAccountEditComponent } from './edit/edit.component';
import { IoTAccessAccount } from './models/access-account';

@UntilDestroy()
@Component({
  selector: 'app-access-accounts',
  templateUrl: './access-accounts.component.html',
})
export class AccessAccountsComponent implements OnInit {
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: { i18n: 'access-accounts.username' }, type: '', index: 'userName' },
    { title: { i18n: 'access-accounts.tenant' }, type: '', index: 'tenant.name' },
    { title: { i18n: 'access-accounts.valid-to' }, type: '', index: 'validTo', render: 'validTo' },
    {
      title: '',
      buttons: [
        {
          icon: 'edit',
          click: (data) => this.showEditModal(data as IoTAccessAccount),
        },
        {
          icon: 'delete',
          type: 'modal',
          click: (data) => this.showDeleteConfirmModal(data as IoTAccessAccount),
          className: 'danger',
        },
      ],
    },
  ];
  data: IoTAccessAccount[];
  currentUser: User;

  constructor(
    private modal: ModalHelper,
    public modalService: NzModalService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public accessAccountService: AccessAccountService,
    private settings: SettingsService,
  ) {
    this.currentUser = this.settings.user as User;
  }

  ngOnInit() {
    this.getAccessAccounts();
  }

  showNewModal() {
    this.modal.createStatic(AccessAccountEditComponent, {}).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getAccessAccounts();
      }
    });
  }

  showEditModal(data: IoTAccessAccount) {
    const params = { record: data, mode: 'edit' };
    this.modal.createStatic(AccessAccountEditComponent, params).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getAccessAccounts();
      }
    });
  }

  showDeleteConfirmModal(user: IoTAccessAccount): void {
    this.modalService.confirm({
      nzTitle: this.transalteService.instant('access-accounts.delete-account'),
      nzOkText: this.transalteService.instant('common.yes'),
      nzOkType: 'danger',
      nzOnOk: () => this.deleteAccessAccount(user.id),
      nzCancelText: this.transalteService.instant('common.no'),
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  getAccessAccounts() {
    this.accessAccountService
      .getAll()
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.data = data;
        this.st.reload();
      });
  }

  deleteAccessAccount(id: string) {
    this.accessAccountService
      .delete(id)
      .pipe(untilDestroyed(this))
      .subscribe(
        () => {
          this.notificationService.success(
            this.transalteService.instant('common.success'),
            this.transalteService.instant('access-accounts.delete-success-message'),
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

