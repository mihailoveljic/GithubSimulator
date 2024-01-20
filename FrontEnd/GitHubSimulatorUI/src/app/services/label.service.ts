import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { UpdateLabelRequest } from '../modules/labels/model/dtos/UpdateLabelRequest';
import { Label } from '../modules/labels/model/Label';
import { InsertLabelRequest } from '../modules/labels/model/dtos/InsertLabelRequest';

@Injectable({
  providedIn: 'root'
})
export class LabelService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) { }

  getAllLabels(): Observable<Label[]> {
    return this.http.get<Label[]>('https://localhost:7103/Label/All');
  }

  getLabelById(id: string): Observable<Label> {
    return this.http.get<Label>('https://localhost:7103/Label/${id}');
  }

  createLabel(dto: InsertLabelRequest): Observable<Label> {
    return this.http.post<Label>('https://localhost:7103/Label', dto);
  }

  updateLabel(dto: UpdateLabelRequest): Observable<Label> {
    return this.http.put<Label>('https://localhost:7103/Label', dto);
  }

  deleteLabel(id: string): Observable<boolean> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.delete<boolean>('https://localhost:7103/Label?id='+ id,
    {headers: headers, responseType: 'json'});
  }
}
