import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ACLGuard } from '@delon/acl';
import { EmbededDatabase } from 'src/app/shared/db/embededDatabase';
import { AccessAccountsComponent } from './access-accounts/access-accounts.component';
import { AdminsListComponent } from './admins/admin-list.component';
import { AdminEventListComponent } from './events/event-list.component';
import { SubscriptionsComponent } from './subscriptions/subscriptions.component';
import { AdminTenantsListComponent } from './tenants/tenant-list.component';

const routes: Routes = [
  {
    path: 'tenants',
    component: AdminTenantsListComponent,
    canActivate: [ACLGuard],
    data: { guard: EmbededDatabase.adminRole },
  },
  {
    path: 'admins',
    component: AdminsListComponent,
    canActivate: [ACLGuard],
    data: { guard: EmbededDatabase.adminRole },
  },
  {
    path: 'access-accounts',
    component: AccessAccountsComponent,
    canActivate: [ACLGuard],
    data: { guard: EmbededDatabase.adminRole },
  },
  {
    path: 'subscriptions',
    component: SubscriptionsComponent,
    canActivate: [ACLGuard],
    data: { guard: EmbededDatabase.adminRole },
  },
  {
    path: 'events',
    component: AdminEventListComponent,
    canActivate: [ACLGuard],
    data: { guard: EmbededDatabase.adminRole },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
