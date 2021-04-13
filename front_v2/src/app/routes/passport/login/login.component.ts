import { Component, Inject, OnDestroy, Optional } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StartupService } from '@core';
import { ReuseTabService } from '@delon/abc/reuse-tab';
import { ACLService } from '@delon/acl';
import { DA_SERVICE_TOKEN, ITokenService, SocialOpenType, SocialService } from '@delon/auth';
import { SettingsService, _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'passport-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
  providers: [SocialService],
})
export class UserLoginComponent implements OnDestroy {
  constructor(
    fb: FormBuilder,
    private router: Router,
    private settingsService: SettingsService,
    private socialService: SocialService,
    @Optional()
    @Inject(ReuseTabService)
    private reuseTabService: ReuseTabService,
    @Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService,
    private startupSrv: StartupService,
    public http: _HttpClient,
    public msg: NzMessageService,
    public aclService: ACLService,
  ) {
    this.form = fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]],
      mobile: [null, [Validators.required]],
      captcha: [null, [Validators.required]],
      remember: [true],
    });
  }

  // #region fields

  get userName(): AbstractControl {
    return this.form.controls.userName;
  }
  get password(): AbstractControl {
    return this.form.controls.password;
  }

  form: FormGroup;
  error = '';
  type = 0;

  // #region get captcha

  count = 0;
  interval$: any;

  // #endregion

  switch({ index }: { index: number }): void {}

  // #endregion

  submit(): void {
    this.error = '';
    this.userName.markAsDirty();
    this.userName.updateValueAndValidity();
    this.password.markAsDirty();
    this.password.updateValueAndValidity();
    if (this.userName.invalid || this.password.invalid) {
      return;
    }
    // In the default configuration, all HTTP requests are mandatory [Verify](https://ng-alain.com/auth/getting-started) user Token
    // Generally, login requests do not require verification, so you can add: `/login?_allow_anonymous=true` to the request URL,
    //  which means that user Token verification is not triggered
    this.http
      .post(`${environment.urls.main}/api/auth/login`, {
        userName: this.userName.value,
        password: this.password.value,
      })
      .subscribe((res) => {
        console.log('res', res);
        
        if (res.message !== 'Success') {
          this.error = res.message;
          return;
        }
        // Clear routing reuse information
        this.reuseTabService.clear();
        // Set user Token information
        // TODO: Mock expired value
        // res.user.expired = +new Date() + 1000 * 60 * 5;
        res.user.avatar = './assets/tmp/img/avatar.jpg';
        this.settingsService.setUser(res.user);
        this.tokenService.set(res.user);
        this.aclService.setRole([res.user?.role?.name]);
        // Re-acquire the StartupService content, we always believe that
        //  application information will generally be affected by the current user authorization range
        this.startupSrv.load().then(() => {
          let url = this.tokenService.referrer!.url || '/';
          if (url.includes('/passport')) {
            url = '/';
          }
          this.router.navigateByUrl(url);
        });
      });
  }

  // #endregion

  ngOnDestroy(): void {
    if (this.interval$) {
      clearInterval(this.interval$);
    }
  }
}
