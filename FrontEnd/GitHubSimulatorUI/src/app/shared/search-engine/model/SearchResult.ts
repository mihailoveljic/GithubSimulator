import { Branch } from "src/app/modules/branch/model/Branch";
import { Comment } from "src/app/modules/comment/model/Comment";
import { Issue } from "src/app/modules/issues/model/Issue";
import { Label } from "src/app/modules/labels/model/Label";
import { Milestone } from "src/app/modules/milestones/model/Milestone";
import { Repository } from "src/app/modules/repositories/model/Repository";

export class SearchResult {
  repositories: Repository[] = [];
  branches: Branch[] = [];
  comments: Comment[] = [];
  issues: Issue[] =[];
  labels: Label[] = [];
  milestones: Milestone[] = [];
}
