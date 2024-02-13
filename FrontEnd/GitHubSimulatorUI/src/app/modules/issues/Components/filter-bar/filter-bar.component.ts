import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LabelService } from 'src/app/services/label.service';

@Component({
  selector: 'app-filter-bar',
  templateUrl: './filter-bar.component.html',
  styleUrls: ['./filter-bar.component.scss'],
})
export class FilterBarComponent implements OnInit{
  constructor(private router: Router, private route: ActivatedRoute, private labelService: LabelService) {}

  labelNum: number = 0

  ngOnInit(): void {
    this.labelService.getAllLabels().subscribe((res) => {
      this.labelNum = res.length
    })
  }

  @Output() getAllIssuesEvent = new EventEmitter<void>();

  getQueryParamsString(): string {
    const queryParams = this.route.snapshot.queryParams;
    return Object.keys(queryParams)
      .map((key) => `${key}:${queryParams[key]}`)
      .join(' ');
  }

  searchIssuesFromInput(value: any) {
    // Split the search string into individual parameters
    const params = value.value
      .split(/\s+/)
      .map((param: any) => param.split(':'));
    // Construct the query parameters object
    const newQueryParams: any = {};
    params.forEach(([key, value]: [string, string]) => {
      if (key && value) {
        newQueryParams[key] = value;
      }
    });

    // Get the current URL without query parameters
    const currentUrlWithoutParams = this.getCurrentUrlWithoutParams();

    if (Object.keys(newQueryParams).length === 0) {
      this.router
        .navigate([currentUrlWithoutParams], {
          queryParams: { q: value.value },
        })
        .then(() => {
          this.getAllIssuesEvent.emit();
        });;
    } else {
      this.router.navigate([currentUrlWithoutParams], {
        queryParams: newQueryParams,
      }).then(() => {
        this.getAllIssuesEvent.emit();
      });
    }
  }

  filterIssues(searchString: string) {
    // Read existing query parameters
    const queryParams: any = {};

    const newParams = searchString.split('+').map((param) => {
      const [key, value] = param.split(':');
      return { key, value };
    });

    newParams.forEach((param) => {
      if (param.key && param.value !== undefined) {
        queryParams[param.key] = param.value;
      }
    });

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: queryParams
    }).then(() => {
      this.getAllIssuesEvent.emit();
    });

  }

  getCurrentUrlWithoutParams(): string {
    const currentUrl = this.router.url;
    const queryParamsIndex = currentUrl.indexOf('?');
    return queryParamsIndex !== -1
      ? currentUrl.substring(0, queryParamsIndex)
      : currentUrl;
  }
}