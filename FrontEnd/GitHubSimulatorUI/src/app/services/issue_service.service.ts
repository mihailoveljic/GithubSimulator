import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';

@Injectable({
  providedIn: 'root',
})
export class IssueService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) {}

  getAllIssues(): Observable<any> {
    return this.http.get(this.baseAddress + '/Issue/All');
  }

  getIssueById(id: string): Observable<any> {
    return this.http.get(this.baseAddress + '/Issue?id=' + id);
  }

  updateIssueTitle(issueId: string, newTitle: string) {
    let updateDto = { id: issueId, title: newTitle };
    return this.http.put(this.baseAddress + '/Issue/updateTitle', updateDto);
  }

  updateIssueAssignee(issueId: string, newAssignee: any) {
    let updateDto = { id: issueId, assignee: newAssignee };
    return this.http.put(this.baseAddress + '/Issue/updateAssignee', updateDto);
  }

  updateIssueMilestone(issueId: string, newMilestoneId: any) {
    let updateDto = { id: issueId, milestoneId: newMilestoneId };
    return this.http.put(
      this.baseAddress + '/Issue/updateMilestone',
      updateDto
    );
  }

  updateIssueLabels(issueId: string, newLabelIds: any) {
    return this.http.put(
      this.baseAddress + '/Issue/updateLabels/?issueId=' + issueId,
      { labelIds: newLabelIds }
    );
  }

  createIssue(issue: any) {
    return this.http.post(this.baseAddress + '/Issue/', issue);
  }

  openOrCloseIssue(id: string, isOpen: boolean) {
    let updateDto = { id: id, isOpen: isOpen };
    return this.http.put(this.baseAddress + '/Issue/openOrClose', updateDto);
  }

  searchIssues(searchString: string): Observable<any> {
    return this.http.post(this.baseAddress + '/Issue/searchIssues', {
      SearchString: searchString,
    });
  }
}
