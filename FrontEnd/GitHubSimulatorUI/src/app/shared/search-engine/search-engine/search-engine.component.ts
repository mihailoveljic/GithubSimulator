import { Component, OnInit } from '@angular/core';
import { SearchResult } from '../model/SearchResult';
import { SearchEngineService } from 'src/app/services/search-engine.service';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-search-engine',
  templateUrl: './search-engine.component.html',
  styleUrls: ['./search-engine.component.css']
})
export class SearchEngineComponent implements OnInit {

  searchResult: SearchResult = {
    labels: [],
    repositories: [],
    issues: [],
    comments: [],
    milestones: [],
    branches: []
  }

  searchResultBackUp: SearchResult = {
    labels: [],
    repositories: [],
    issues: [],
    comments: [],
    milestones: [],
    branches: []
  }

  searchTerm: string = '';
  isLoading: boolean = false;

  constructor(private searchEngineService: SearchEngineService) { }

  ngOnInit() {
    this.searchEngineService.searchTerm$
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
      )
      .subscribe(searchTerm => {
        this.searchTerm = searchTerm;
        if (!this.searchTerm) {
          this.searchResult = this.searchResultBackUp;
          return;
        }
        this.isLoading = true;
        this.searchEngineService.search(this.searchTerm).subscribe(result => {
          this.searchResult = result;
          this.isLoading = false;
        });
      });
    }
}
