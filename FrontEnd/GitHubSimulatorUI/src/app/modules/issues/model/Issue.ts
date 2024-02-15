export class Issue {
  id: string | undefined;
  title: string | undefined;
  description: string | undefined;
  createdAt: Date | undefined;
  assignee : {
    email: string | undefined;
  } | undefined;
  repositoryId: string | undefined;
  milestoneId: string | undefined;
}
