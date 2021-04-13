import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { NzListModule } from 'ng-zorro-antd/list';
import { AdminRoutingModule } from './admin-routing.module';
import { AdminsListComponent } from './admins/admin-list.component';
import { AdminsEditComponent } from './admins/edit/edit.component';
import { AdminsViewComponent } from './admins/view/view.component';
import { AdminEventListComponent } from './events/event-list.component';
import { AdminTenantsEditComponent } from './tenants/edit/edit.component';
import { AdminTenantsListComponent } from './tenants/tenant-list.component';
import { AdminTenantsViewComponent } from './tenants/view/view.component';
import { AccessAccountsComponent } from './access-accounts/access-accounts.component';
import { AccessAccountEditComponent } from './access-accounts/edit/edit.component';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { SubscriptionDetailsComponent } from './subscriptions/subscription-details/subscription-details.component';

const COMPONENTS: Type<void>[] = [
  AdminTenantsListComponent,
  AdminsListComponent,
  AccessAccountsComponent,
  SubscriptionsComponent,
  AdminEventListComponent,
];
const COMPONENTS_NOROUNT: Type<void>[] = [
  AdminTenantsEditComponent,
  AdminTenantsViewComponent,
  AdminsEditComponent,
  AdminsViewComponent,
  AccessAccountEditComponent,
  SubscriptionDetailsComponent,
];

@NgModule({
  imports: [SharedModule, NzListModule, NzDatePickerModule, AdminRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_NOROUNT],
})
export class AdminModule {}
