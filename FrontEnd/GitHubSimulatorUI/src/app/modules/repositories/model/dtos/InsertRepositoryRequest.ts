import { Visibility } from "../Visibility";

export class InsertRepositoryRequest {
  name: string = '';
  description: string = '';
  visibility: Visibility = Visibility.Private;
  gitignores: string = '';
  license: string = '';
}
