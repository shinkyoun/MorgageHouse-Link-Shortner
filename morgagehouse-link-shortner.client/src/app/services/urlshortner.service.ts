import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UrlShortnerService {
  //baseUrl: string = "http://localhost:3000/";

  constructor(private httpClient: HttpClient) { }

  addUrl(data: any): Observable<any> {
    return this.httpClient.post(`/api/linkshortner`, data);
  }

  getUrlList(): Observable<any> {
    return this.httpClient.get('/api/linkshortner');
  }

  deleteUrl(id: string): Observable<any> {
    return this.httpClient.delete(`/api/linkshortner/${id}`);
  }
}
