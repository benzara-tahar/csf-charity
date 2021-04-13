import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { EmbededDatabase } from 'src/app/shared/db/embededDatabase';
import { TenantEventListComponent } from './events/event-list.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { UsersListComponent } from './users-list/users-list.component';

const routes: Routes = [
  {
    path: 'users',
    component: UsersListComponent,
    canActivate: [ACLGuard],
    data: { guard: [EmbededDatabase.adminRole, EmbededDatabase.tenantAdminRole] },
  },
  {
    path: 'subscriptions',
    component: SubscriptionsComponent,
    canActivate: [ACLGuard],
    data: { guard: [EmbededDatabase.tenantAdminRole] },
  },

  {
    path: 'events',
    component: TenantEventListComponent,
    canActivate: [ACLGuard],
    data: { guard: [EmbededDatabase.tenantAdminRole] },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TenantRoutingModule {}
