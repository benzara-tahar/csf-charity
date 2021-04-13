import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccessAccountsComponent } from './access-accounts.component';

describe('AccessAccountsComponent', () => {
  let component: AccessAccountsComponent;
  let fixture: ComponentFixture<AccessAccountsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccessAccountsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessAccountsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
