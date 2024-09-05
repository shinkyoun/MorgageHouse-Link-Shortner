import { HttpClient } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { UrlLink } from '../model/url-link';
import { MatSelectChange } from '@angular/material/select';
import { MatSort, Sort } from '@angular/material/sort';
import { UrlShortnerService } from '../services/urlshortner.service';
import { LogService } from '../services/log.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-url-list-table',
  templateUrl: './url-list-table.component.html',
  styleUrls: ['./url-list-table.component.scss']
})

export class UrlListTableComponent implements OnInit {
  public displayedColumns: string[] = ['id', 'fullUrl', 'shortLink', 'action'];
  public urlAddForm: FormGroup;
  public dataSource: any;

  constructor(
    private http: UrlShortnerService,
    private logger: LogService,
    private formBuilder: FormBuilder
  ) {
    this.urlAddForm = this.formBuilder.group({
      fullUrl: ['']
    });
  }

  ngAfterViewInit() { }

  ngOnInit(): void {
    this.getUrlList();
  }

  getUrlList() {
    this.http.getUrlList().subscribe(
      (result) => {
        this.dataSource = new MatTableDataSource(result);
      },
      (error) => {
        console.error(error);
      }
    );
  }

  deleteUrl(id: string) {
    let confirm = window.confirm("Are you sure you want to delete this registered url?");
    if (confirm) {
      this.http.deleteUrl(id).subscribe({
        next: (res) => {
          alert('Url deleted!');
          this.getUrlList();
        },
        error: (err) => {
          console.log(err);
        },
      });
    }
  }

  onSubmit() {
    this.http.addUrl(this.urlAddForm.value).subscribe({
      next: (res) => {
        alert('Url added!');
        this.urlAddForm.reset();
        this.getUrlList();
      },
      error: (err) => {
        this.urlAddForm.reset();
        alert(`Failed to add url, ${err}`);
      },
    });
  }
}
