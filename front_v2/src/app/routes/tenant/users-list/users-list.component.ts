import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper, SettingsService, _HttpClient } from '@delon/theme';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { User } from '../../admin/admins/models/user';
import { UserEditComponent } from './edit/edit.component';
import { UserService } from './user.service';

@UntilDestroy()

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styles: [
  ]
})
export class UsersListComponent implements OnInit {
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: { i18n: 'app.login.username' }, type: '', index: 'userName' },
    { title: 'Email', type: '', index: 'email' },
    { title: { i18n: 'admins.role' }, type: '', index: 'role', render: 'role' },
    {
      title: '',
      buttons: [
        {
          icon: 'edit',
          click: (data) => this.showEditModal(data as User),
        },
        {
          icon: 'delete',
          type: 'modal',
          click: (data) => this.showDeleteConfirmModal(data as User),
          iif: (data) => !(data.id == this.currentUser.id),
          className: 'danger',
        },
      ],
    },
  ];
  data: User[];
  currentUser: User;

  constructor(
    private modal: ModalHelper,
    public modalService: NzModalService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public userService: UserService,
    private settings: SettingsService,
  ) {
    this.currentUser = this.settings.user as User;
  }

  ngOnInit() {
    this.getAdmins();
  }

  showNewModal() {
    this.modal.createStatic(UserEditComponent, { tenantId: this.currentUser.tenantId }).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getAdmins();
      }
    });
  }

  showEditModal(data: User) {
    const params = { record: data, mode: 'edit' };
    this.modal.createStatic(UserEditComponent, params).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getAdmins();
      }
    });
  }

  showDeleteConfirmModal(user: User): void {
    this.modalService.confirm({
      nzTitle: this.transalteService.instant('admins.delete-admin'),
      nzOkText: this.transalteService.instant('common.yes'),
      nzOkType: 'danger',
      nzOnOk: () => this.deleteAdmin(user.id),
      nzCancelText: this.transalteService.instant('common.no'),
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  getAdmins() {
    this.userService
      .getAll(this.currentUser.tenantId)
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.data = data;
        this.st.reload();
      });
  }

  deleteAdmin(id: string) {
    this.userService
      .delete(id)
      .pipe(untilDestroyed(this))
      .subscribe(
        () => {
          this.notificationService.success(
            this.transalteService.instant('common.success'),
            this.transalteService.instant('admins.delete-success-message'),
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

