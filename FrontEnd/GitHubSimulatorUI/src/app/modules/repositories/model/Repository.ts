import { Visibility } from "./Visibility";

export class Repository {
  id: string = '';
  name: string = '';
  description: string = '';
  visibility: Visibility = Visibility.Private;
  starsCount: number = 0;
  forksCount: number = 0;
  watchersCount: number = 0;
  createdAt: Date = new Date();
  updatedAt: Date = new Date();
}
