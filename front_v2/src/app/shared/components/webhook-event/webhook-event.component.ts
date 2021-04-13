import { Component, Input, OnInit } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { NzDrawerRef } from 'ng-zorro-antd/drawer';
import { WebhookEvent } from '../../model/webhook-event';

@Component({
  selector: 'app-events-view',
  templateUrl: './webhook-event.component.html',
  styles: [
    `
      .payload {
        background: #141223;
        color: #f48cfb;
        border-radius: 4px;
      }
    `,
  ],
})
export class WebhookEventComponent implements OnInit {
  @Input()
  record: WebhookEvent;
  payload: any = {};
  i: any;

  constructor(private ref: NzDrawerRef) {}
  ngOnInit(): void {
    if (this.record) {
      this.payload = JSON.parse(this.record.payload);
    }
  }

  ok(): void {
    this.ref.close();
  }
}
