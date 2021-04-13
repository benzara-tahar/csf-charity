import { Component, Inject, OnInit } from '@angular/core';
import { DA_SERVICE_TOKEN, ITokenService } from '@delon/auth';

@Component({
  selector: 'layout-passport',
  templateUrl: './passport.component.html',
  styleUrls: ['./passport.component.less'],
})
export class LayoutPassportComponent implements OnInit {
  links = [
    {
      title: 'FAQ',
      href: '',
    },
    {
      title: 'Terms',
      href: '',
    },
    {
      title: 'Charity.com',
      href: 'https://www.charity.com',
    },
  ];

  constructor(@Inject(DA_SERVICE_TOKEN) private tokenService: ITokenService) {}

  ngOnInit(): void {
    this.tokenService.clear();
  }
}
