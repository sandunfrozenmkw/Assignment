import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccountsUploadComponent } from './accounts-upload.component';

describe('AccountsUploadComponent', () => {
  let component: AccountsUploadComponent;
  let fixture: ComponentFixture<AccountsUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountsUploadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountsUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
