import { Component, Input, OnInit } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { NzDrawerRef } from 'ng-zorro-antd/drawer';
import { DistanceUnits } from 'src/app/routes/tenant/subscriptions/models/distanceUnits';
import { Subscription } from 'src/app/routes/tenant/subscriptions/models/subscription';

@Component({
  selector: 'app-subscription-details',
  templateUrl: './subscription-details.component.html',
  styles: [],
})
export class SubscriptionDetailsComponent implements OnInit {
  @Input()
  record: Subscription;
  i: any;
  DistanceUnits: typeof DistanceUnits = DistanceUnits;

  constructor(private ref: NzDrawerRef) { }
  ngOnInit(): void {
  }

  closeDrawer(): void {
    this.ref.close();
  }
}
