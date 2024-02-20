import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';

@Injectable({
  providedIn: 'root',
})
export class PullRequestService {
  baseAddress: string = environment.API_BASE_URL;

  constructor(private http: HttpClient) {}

  getAllPullRequest(repo: string): Observable<any> {
    return this.http.get(this.baseAddress + '/PullRequest/All{repo}');
  }

  getPullRequestById(id: string): Observable<any> {
    return this.http.get(this.baseAddress + '/PullRequest?id=' + id);
  }

  getPullRequestByIndex(repo: string, index: string): Observable<any> {
    return this.http.get(this.baseAddress + '/PullRequest/pull/' + repo +'/' + index);
  }

  getPullRequestDiff(repo: string, index: string): Observable<any> {
    return this.http.get(this.baseAddress + '/PullRequest/pullDiff/' + repo +'/' + index, {responseType:"text"});
  }

  getPullRequestCommit(repo: string, index: string): Observable<any> {
    return this.http.get(this.baseAddress + '/PullRequest/pullCommits/' + repo +'/' + index);
  }

  updatePullRequest(id: string, dto: any ) {
    return this.http.put(this.baseAddress + '/PullRequest/'+ id, dto);
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

  createPullRequest(pull: any, repo: string) {
    return this.http.post(this.baseAddress + '/PullRequest/' + repo, pull);
  }

  mergePullRequest(pull: any, repo: string, index : string) {
    return this.http.post(this.baseAddress + '/PullRequest/pullMerge/' + repo + '/' + index, pull);
  }


  searchPullRequest(searchString: string, repo: string): Observable<any> {
    return this.http.post(this.baseAddress + '/PullRequest/pullSearch/' + repo, {
      SearchString: searchString,
    });
  }
}
