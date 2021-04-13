import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { throwIfAlreadyLoaded } from '@core';
import { DelonMockModule } from '@delon/mock';
import { AlainThemeModule } from '@delon/theme';
import { AlainConfig, ALAIN_CONFIG } from '@delon/util';

// Please refer to: https://ng-alain.com/docs/global-config
// #region NG-ALAIN Config

import { DelonACLModule } from '@delon/acl';

const alainConfig: AlainConfig = {
  st: {
    modal: { size: 'lg' },
    singleSort: { key: 'sort', nameSeparator: ':' },
    size: 'middle',
    res: {
      reName: { total: 'data.total', list: 'data.results' },
    },
  },
  pageHeader: { homeI18n: 'app.home' },
  lodop: {
    license: `A59B099A586B3851E0F0D7FDBF37B603`,
    licenseA: `C94CEE276DB2187AE6B65D56B3FC2848`,
  },
  auth: { login_url: '/passport/login' },
};

const alainModules = [AlainThemeModule.forRoot(), DelonACLModule.forRoot(), DelonMockModule.forRoot()];
const alainProvides = [{ provide: ALAIN_CONFIG, useValue: alainConfig }];

// mock
import { environment } from '@env/environment';
import * as MOCKDATA from '../../_mock';
if (!environment.production) {
  alainConfig.mock = { data: MOCKDATA };
}

// #region reuse-tab
/**
 * If you need [Route Reuse](https://ng-alain.com/components/reuse-tab), you need:
 * 1. Import the `ReuseTabModule` module in `shared-delon.module.ts`
 * 2. Register `RouteReuseStrategy`
 * 3. Modify in `src/app/layout/default/default.component.html`:
 * ```html
 * <section class="alain-default__content">
 * <reuse-tab #reuseTab></reuse-tab>
 * <router-outlet (activate)="reuseTab.activate($event)"></router-outlet>
 * </section>
 *  ```
 */
// import { RouteReuseStrategy } from '@angular/router';
// import { ReuseTabService, ReuseTabStrategy } from '@delon/abc/reuse-tab';
// alainProvides.push({
//   provide: RouteReuseStrategy,
//   useClass: ReuseTabStrategy,
//   deps: [ReuseTabService],
// } as any);

// #endregion

// #endregion

// Please refer to: https://ng.ant.design/docs/global-config/en#how-to-use
// #region NG-ZORRO Config

import { NzConfig, NZ_CONFIG } from 'ng-zorro-antd/core/config';

const ngZorroConfig: NzConfig = {};

const zorroProvides = [{ provide: NZ_CONFIG, useValue: ngZorroConfig }];

// #endregion

@NgModule({
  imports: [...alainModules],
})
export class GlobalConfigModule {
  constructor(@Optional() @SkipSelf() parentModule: GlobalConfigModule) {
    throwIfAlreadyLoaded(parentModule, 'GlobalConfigModule');
  }

  static forRoot(): ModuleWithProviders<GlobalConfigModule> {
    return {
      ngModule: GlobalConfigModule,
      providers: [...alainProvides, ...zorroProvides],
    };
  }
}
