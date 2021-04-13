import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SimpleGuard } from '@delon/auth';
import { environment } from '@env/environment';
// layout
import { LayoutDefaultComponent } from '../layout/default/default.component';
import { LayoutFullScreenComponent } from '../layout/fullscreen/fullscreen.component';
import { LayoutPassportComponent } from '../layout/passport/passport.component';
// single pages
import { CallbackComponent } from './callback/callback.component';
// dashboard pages
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserLockComponent } from './passport/lock/lock.component';
// passport pages
import { UserLoginComponent } from './passport/login/login.component';
import { UserRegisterResultComponent } from './passport/register-result/register-result.component';
import { UserRegisterComponent } from './passport/register/register.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutDefaultComponent,
    canActivate: [SimpleGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, data: { title: 'Dashboard' } },
      { path: 'exception', loadChildren: () => import('./exception/exception.module').then((m) => m.ExceptionModule) },
      { path: 'admin', loadChildren: () => import('./admin/admin.module').then((m) => m.AdminModule) },
      { path: 'tenant', loadChildren: () => import('./tenant/tenant.module').then((m) => m.TenantModule) }
    ],
  },

  // passport
  {
    path: 'passport',
    component: LayoutPassportComponent,
    children: [
      { path: 'login', component: UserLoginComponent, data: { title: 'Login' } },
      { path: 'register', component: UserRegisterComponent, data: { title: 'Register' } },
      { path: 'register-result', component: UserRegisterResultComponent, data: { title: 'register result' } },
      { path: 'lock', component: UserLockComponent, data: { title: 'Lock' } },
    ],
  },
  // Single page not wrapped Layout
  { path: 'callback/:type', component: CallbackComponent },
  { path: '**', redirectTo: 'exception/404' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: environment.useHash,
      // NOTICE: If you use `reuse-tab` component and turn on keepingScroll you can set to `disabled`
      // Pls refer to https://ng-alain.com/components/reuse-tab
      scrollPositionRestoration: 'top',
    }),
  ],
  exports: [RouterModule],
})
export class RouteRoutingModule { }
