import { NgModule, Type } from '@angular/core';
import { SharedModule } from '@shared';
import { NzListModule } from 'ng-zorro-antd/list';

import { TenantRoutingModule } from './tenant-routing.module';
import { UserEditComponent } from './users-list/edit/edit.component';
import { UsersListComponent } from './users-list/users-list.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { SubscriptionEditComponent } from './subscriptions/edit/edit.component';
import { TenantEventListComponent } from './events/event-list.component';

const COMPONENTS: Type<void>[] = [UsersListComponent, SubscriptionsComponent, TenantEventListComponent];
const COMPONENTS_NOROUNT: Type<void>[] = [UserEditComponent, SubscriptionEditComponent];
@NgModule({
  imports: [SharedModule, NzListModule, TenantRoutingModule],
  declarations: [...COMPONENTS, ...COMPONENTS_NOROUNT],
})
export class TenantModule {}
