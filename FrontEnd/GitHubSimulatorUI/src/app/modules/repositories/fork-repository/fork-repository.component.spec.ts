import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForkRepositoryComponent } from './fork-repository.component';

describe('ForkRepositoryComponent', () => {
  let component: ForkRepositoryComponent;
  let fixture: ComponentFixture<ForkRepositoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForkRepositoryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForkRepositoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
