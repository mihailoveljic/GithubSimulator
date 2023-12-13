import { Visibility } from "./Visibility";

export class Repository {
  id: string = '';
  name: string = '';
  description: string = '';
  visibility: Visibility = Visibility.Private;
}
