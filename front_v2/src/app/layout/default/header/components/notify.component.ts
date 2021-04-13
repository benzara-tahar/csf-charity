import { ChangeDetectionStrategy, ChangeDetectorRef, Component } from '@angular/core';
import { NoticeIconList, NoticeIconSelect, NoticeItem } from '@delon/abc/notice-icon';
import add from 'date-fns/add';
import formatDistanceToNow from 'date-fns/formatDistanceToNow';
import parse from 'date-fns/parse';
import { NzI18nService } from 'ng-zorro-antd/i18n';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'header-notify',
  template: `
    <notice-icon
      [data]="data"
      [count]="count"
      [loading]="loading"
      btnClass="alain-default__nav-item"
      btnIconClass="alain-default__nav-item-icon"
      (select)="select($event)"
      (clear)="clear($event)"
      (popoverVisibleChange)="loadData()"
    ></notice-icon>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderNotifyComponent {
  data: NoticeItem[] = [
    {
      title: 'Deployments',
      list: [],
      emptyText: 'No deployments found!',
      emptyImage: 'https://gw.alipayobjects.com/zos/rmsportal/wAhyIChODzsoKIOBHcBk.svg',
      clearText: 'Clear Deployments',
    },
    {
      title: 'Events',
      list: [],
      emptyText: 'No events found!',
      emptyImage: 'https://gw.alipayobjects.com/zos/rmsportal/HsIsxMZiWKrNUavQUXqx.svg',
      clearText: 'Clear Events',
    },
  ];
  count = 5;
  loading = false;

  constructor(private msg: NzMessageService, private nzI18n: NzI18nService, private cdr: ChangeDetectorRef) {}

  private updateNoticeData(notices: NoticeIconList[]): NoticeItem[] {
    const data = this.data.slice();
    data.forEach((i) => (i.list = []));

    notices.forEach((item) => {
      const newItem = { ...item } as NoticeIconList;
      if (typeof newItem.datetime === 'string') {
        newItem.datetime = parse(newItem.datetime, 'yyyy-MM-dd', new Date());
      }
      if (newItem.datetime) {
        newItem.datetime = formatDistanceToNow(newItem.datetime as Date, { locale: this.nzI18n.getDateLocale() });
      }
      if (newItem.extra && newItem.status) {
        newItem.color = ({
          todo: undefined,
          processing: 'blue',
          urgent: 'red',
          doing: 'gold',
        } as { [key: string]: string | undefined })[newItem.status];
      }
      data.find((w) => w.title === newItem.type)!.list.push(newItem);
    });
    return data;
  }

  loadData(): void {
    if (this.loading) {
      return;
    }
    this.loading = true;
    setTimeout(() => {
      const now = new Date();
      this.data = this.updateNoticeData([
        // {
        //   id: '000000001',
        //   avatar: 'https://gw.alipayobjects.com/zos/rmsportal/ThXAXghbEsBCCSDihZxY.png',
        //   title: 'Obeyda hang the production',
        //   datetime: add(now, { days: 10 }),
        //   type: 'Deployments',
        // },
        // {
        //   id: '000000002',
        //   avatar: 'https://gw.alipayobjects.com/zos/rmsportal/OKJXDXrmkNshAMvwtvhu.png',
        //   title: 'ZineEddine loves ionic',
        //   datetime: add(now, { days: -3 }),
        //   type: 'Deployments',
        // },
        // {
        //   id: '000000004',
        //   avatar: 'https://gw.alipayobjects.com/zos/rmsportal/GvqBnKhFgObvnSGkDsje.png',
        //   title: 'Azure function stopped',
        //   datetime: add(now, { years: -1 }),
        //   type: 'Deployments',
        // },
        // {
        //   id: '000000005',
        //   avatar: 'https://gw.alipayobjects.com/zos/rmsportal/ThXAXghbEsBCCSDihZxY.png',
        //   title: 'Azure function deplyed',
        //   datetime: '2017-08-07',
        //   type: 'Deployments',
        // },
        // {
        //   id: '000000010',
        //   title: 'device.intstalled',
        //   description: 'EX652AZ 6mn ago',
        //   extra: 'failed',
        //   status: 'urgent',
        //   type: 'Events',
        // },
        // {
        //   id: '000000011',
        //   title: 'device.telemetry',
        //   description: 'ZA632PW',
        //   extra: 'pending',
        //   status: 'doing',
        //   type: 'Events',
        // },
      ]);

      this.loading = false;
      this.cdr.detectChanges();
    }, 500);
  }

  clear(type: string): void {
    this.msg.success(`清空了 ${type}`);
  }

  select(res: NoticeIconSelect): void {
    this.msg.success(`点击了 ${res.title} 的 ${res.item.title}`);
  }
}
