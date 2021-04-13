import { Component, OnInit, ViewChild } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper, _HttpClient } from '@delon/theme';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { TranslateService } from '@ngx-translate/core';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { AdminTenantsEditComponent } from './edit/edit.component';
import { TenantService } from 'src/app/services/tenant.service';
import { Tenant } from '@shared';
@UntilDestroy()
@Component({
  selector: 'app-admin-tenants-tenants',
  templateUrl: './tenant-list.component.html',
})
export class AdminTenantsListComponent implements OnInit {
  @ViewChild('st', { static: false }) st: STComponent;
  columns: STColumn[] = [
    { title: { i18n: 'tenants.tenant-name' }, type: '', index: 'name' },
    { title: { i18n: 'tenants.tenant-iot-providers' }, index: 'supportedIoTProviders', render: 'custom-providers' },
    {
      title: '',
      buttons: [
        {
          icon: 'edit',
          click: (data) => this.showEditModal(data as Tenant),
        },
        {
          icon: 'delete',
          type: 'modal',
          click: (data) => this.showDeleteConfirmModal(data as Tenant),
          className: 'danger',
        },
      ],
    },
  ];
  data: Tenant[];

  constructor(
    private modal: ModalHelper,
    public modalService: NzModalService,
    public notificationService: NzNotificationService,
    public transalteService: TranslateService,
    public tenantService: TenantService,
  ) {}

  ngOnInit() {
    this.getTenants();
  }

  showNewModal() {
    this.modal.createStatic(AdminTenantsEditComponent, {}).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getTenants();
      }
    });
  }

  showEditModal(data: Tenant) {
    const params = { record: data };
    this.modal.createStatic(AdminTenantsEditComponent, params).subscribe(() => this.st.reload());
    this.modalService.openModals[0].afterClose.subscribe((response) => {
      if (response) {
        this.getTenants();
      }
    });
  }

  showDeleteConfirmModal(user: Tenant): void {
    this.modalService.confirm({
      nzTitle: this.transalteService.instant('tenants.delete-tenant'),
      nzOkText: this.transalteService.instant('common.yes'),
      nzOkType: 'danger',
      nzOnOk: () => this.deleteTenant(user.id),
      nzCancelText: this.transalteService.instant('common.no'),
      nzOnCancel: () => console.log('Cancel'),
    });
  }

  getTenants() {
    this.tenantService
      .getAll()
      .pipe(untilDestroyed(this))
      .subscribe((data) => {
        this.data = data;
        this.st.reload();
      });
  }

  deleteTenant(id: string) {
    this.tenantService
      .delete(id)
      .pipe(untilDestroyed(this))
      .subscribe(
        () => {
          this.notificationService.success(
            this.transalteService.instant('common.success'),
            this.transalteService.instant('tenants.delete-success-message'),
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
