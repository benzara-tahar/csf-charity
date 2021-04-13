import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { SettingsService, User } from '@delon/theme';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent implements OnInit {
  user: User;

  constructor(private settings: SettingsService) {
    this.user = this.settings.user;
  }

  ngOnInit() {}
}
