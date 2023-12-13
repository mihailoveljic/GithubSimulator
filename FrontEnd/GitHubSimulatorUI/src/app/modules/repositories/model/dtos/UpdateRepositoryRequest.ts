import { Visibility } from "../Visibility";

export class UpdateRepositoryRequest {
  id: string = '';
  name: string = '';
  description: string = '';
  visibility: Visibility = Visibility.Private;
}
