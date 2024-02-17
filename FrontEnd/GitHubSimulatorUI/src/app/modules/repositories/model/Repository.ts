import { Visibility } from "./Visibility";

export class Repository {
  id: string = '';
  name: string = '';
  description: string = '';
  visibility: Visibility = Visibility.Private;
  stars_Count: number = 0;
  forks_Count: number = 0;
  watchers_Count: number = 0;
  createdAt: Date = new Date();
  updatedAt: Date = new Date();
}
