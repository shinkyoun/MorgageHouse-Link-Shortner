import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [HttpClientTestingModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve url links from the server', () => {
    const mocklinkUrls = [
      { id: '1235', fullUrl: "http://tetsts/ffff", shortLink: "dfffg" },
      { id: '1235', fullUrl: "http://tetsts/ffff", shortLink: "dfffg" }
    ];

    component.ngOnInit();

    const req = httpMock.expectOne('/linkshortner');
    expect(req.request.method).toEqual('GET');
    req.flush(mocklinkUrls);

    expect(component.linkUrls).toEqual(mocklinkUrls);
  });
});
