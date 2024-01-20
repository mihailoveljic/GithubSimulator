export class UpdateLabelRequest {
  id: string = '';
  name: string = '';
  description: string = '';
  color: string = '';

  constructor(id: string, name: string, description: string, color: string) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.color = color;
  }
}
