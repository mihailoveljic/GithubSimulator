export class BranchDto {
    id: string | null= "";
    name: string = "";
    repositoryId: string = "";
    issueId: string| null = "";
    creatorId: string = "";
    pullRequest: string="";

    constructor(id: string, name: string, repositoryId: string, issueId: string, creatorId: string, pullRequest: string) {
        this.name = name;
        this.repositoryId = repositoryId;
        this.issueId = issueId;
        this.creatorId = creatorId;
        this.pullRequest = pullRequest;
    }
}
