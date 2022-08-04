import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountsDisplayComponent } from './accounts-display.component';

describe('BoardAdminComponent', () => {
  let component: AccountsDisplayComponent;
  let fixture: ComponentFixture<AccountsDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountsDisplayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountsDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
