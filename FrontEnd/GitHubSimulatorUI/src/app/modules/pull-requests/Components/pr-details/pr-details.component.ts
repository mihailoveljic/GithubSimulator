import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { PullRequestService } from 'src/app/services/pull-request.service';

@Component({
  selector: 'app-issue-details',
  templateUrl: './pr-details.component.html',
  styleUrls: ['./pr-details.component.scss'],
})
export class PRDetailsComponent implements OnInit {
  @ViewChild(MatAccordion) accordion!: MatAccordion;

  pullTitleEdited: string = '';

  pullDetails: any = {};
  pullRemote: any ={};

  transformData(inputData: any): any {
    return {
      source: inputData.source,
      target: inputData.target,
      assignee: inputData.assignee,
      base: inputData.base,
      body: inputData.body,
      head: inputData.head,
      title: inputData.title,
      repoName: inputData.repoName,
      assignees:inputData.assignees,
      issueId: inputData.issueId,
      milestoneId: inputData.milestoneId,
      repositoryId: inputData.repositoryId,
      isOpen: inputData.isOpen,
      number: inputData.number,
      events: inputData.events,
      labelIds: inputData.labels.map((label: any) => label.id)
    };
  }

  @ViewChild(MatTabGroup) tabGroup!: MatTabGroup;

  // Funkcija za promenu taba na osnovu indeksa
  promeniTab(indeks: number) {
    this.tabGroup.selectedIndex = indeks;
  }

  klikniNaRed(red: any) {
    // Prosledi podatak u treći tab i promeni na njega
    this.promeniTab(2);
    // Pozovi odgovarajucu metodu ili ucitaj odgovarajuci sadrzaj na osnovu prosledjenog podatka
    // Implementacija zavisi od vaših potreba
  }

  

  constructor(
    private route: ActivatedRoute,
    private pullRequestService: PullRequestService,
    private datePipe: DatePipe
  ) {}

  commits : any={};
  numCommits: number=0;
  diff: any={}
  files: any={}

  

  displayedColumns: string[] = ['sha', 'message', 'date', 'additions', 'deletions'];
  panelOpenState = false;
  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const pullId = params['id'];
      this.pullRequestService.getPullRequestById(pullId).subscribe((res) => {
        this.pullDetails = res;
        console.log("Ovde se nesto desilo", this.pullDetails)
        
        this.pullRequestService.getPullRequestDiff(this.pullDetails.repoName, "3").subscribe((res) => {
          this.diff = res;
          this.files = extractChangedFiles(this.diff);
console.log(this.files);
          console.log("Ovde je diff", this.diff)
        });
        
      this.pullRequestService.getPullRequestCommit(this.pullDetails.repoName, this.pullDetails.number).subscribe((res) => {
        this.commits = res;
        this.numCommits = this.commits.length
        console.log("Ovde se nesto desilo", this.commits)
      });
        
        this.pullRequestService.getPullRequestByIndex(this.pullDetails.repoName, this.pullDetails.number).subscribe((res)=>{
          this.pullRemote = res;
        });
        localStorage.setItem("pullId", this.pullDetails.id);
        this.pullTitleEdited = this.pullDetails.title;
      });
      
      console.log("Ovde se nesto desilo", this.pullDetails)
      
    });
  }

  commentNum: number = 0;

  isEditing = false;

  startEditing() {
    this.isEditing = true;
  }

  cancelEditing() {
    this.isEditing = false;
    this.pullTitleEdited = this.pullDetails.title;
  }

  confirmEditing() {
    this.isEditing = false;
    this.pullDetails.title = this.pullTitleEdited;

    this.pullRequestService
      .updatePullRequest(this.pullDetails.id, this.transformData(this.pullDetails))
      .subscribe((res) => {
      });
  }

  getFormatedDate(unformatedDateInput: any) {
    if (unformatedDateInput === null || unformatedDateInput === undefined)
      return;
    const unformatedDate = new Date(unformatedDateInput);
    return this.datePipe.transform(unformatedDate, 'dd-MM-yyyy HH:mm');
  }

  openOrCloseIssue(id: string, isOpen: boolean) {
    this.pullDetails.isOpen = isOpen
    this.pullRequestService.updatePullRequest(id, this.transformData(this.pullDetails)).subscribe((res) => {
    });
  }
  
}

export interface ChangedFile {
  fileName: string;
  content: string[];
}

export function extractChangedFiles(text: string): ChangedFile[] {
  const files: ChangedFile[] = [];
  const lines = text.split('\n');
  let fileName: string | null = null;
  let content: string[] = [];
  let inFileSection = false;

  const fileNameRegex = /^diff --git a\/(.+) b\/.+/; // Regularni izraz za izdvajanje imena fajla

  for (const line of lines) {
    const fileNameMatch = line.match(fileNameRegex);
    if (fileNameMatch) {
      // Uzimanje imena fajla iz linije diff --git
      if (fileName && content.length > 0) {
        // Dodavanje prethodnog fajla u listu
        files.push({ fileName, content });
        content = [];
      } else if (fileName && content.length === 0) {
        // Dodavanje praznog fajla ako je detektovan
        files.push({ fileName, content: [] });
        content = [];
      }
      fileName = fileNameMatch[1];
      inFileSection = false; // Resetovanje stanja sekcije fajla
    } else if (line.startsWith('@@')) {
      content.push(line);

      // Početak sekcije fajla
      inFileSection = true;
    } else if (inFileSection && !line.startsWith('\\ No newline at end of file')) {
      // Sadržaj fajla
      content.push(line);
    }
  }

  // Dodavanje poslednjeg fajla ako postoji
  if (fileName && content.length > 0) {
    files.push({ fileName, content });
  } else if (fileName && content.length === 0) {
    // Dodavanje praznog fajla ako je detektovan
    files.push({ fileName, content: [] });
  }

  return files;
}


import { Directive, Output, EventEmitter } from '@angular/core';
import { MatAccordion } from '@angular/material/expansion';
import { MatTabGroup } from '@angular/material/tabs';

@Directive({
  selector: '[appExpansionState]'
})
export class ExpansionStateDirective {
  @Output() opened = new EventEmitter<void>();
  @Output() closed = new EventEmitter<void>();

  constructor() { }

  panelOpened() {
    this.opened.emit();
  }

  panelClosed() {
    this.closed.emit();
  }
}