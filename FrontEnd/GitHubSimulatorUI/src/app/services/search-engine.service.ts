import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environment/environment';
import { SearchResult } from '../shared/search-engine/model/SearchResult';

@Injectable({
  providedIn: 'root'
})
export class SearchEngineService {
  baseAddress: string = environment.API_BASE_URL;

  private searchTermSubject = new BehaviorSubject<string>('');
  searchTerm$ = this.searchTermSubject.asObservable();

  constructor(private http: HttpClient) { }

  search(searchTerm: string): Observable<SearchResult> {
    return this.http.get<SearchResult>('https://localhost:7103/SearchEngine?searchTerm=' + searchTerm);
  }

  setSearchTerm(searchTerm: string) {
    this.searchTermSubject.next(searchTerm);
  }
}
