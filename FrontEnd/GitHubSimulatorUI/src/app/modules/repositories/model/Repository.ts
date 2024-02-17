import { Visibility } from "./Visibility";

export class Repository {
  id: string = '';
  name: string = '';
  description: string = '';
  visibility: Visibility = Visibility.Private;
  stars_Count: number = 0;
  forks_Count: number = 0;
  watchers_Count: number = 0;
  created_At: Date = new Date();
  updated_At: Date = new Date();
  clone_Url: string = '';
  ssh_Url: string = '';
}
