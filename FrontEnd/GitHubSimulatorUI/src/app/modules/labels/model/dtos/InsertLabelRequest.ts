export class InsertLabelRequest {
  name: string;
  description: string;
  color: string;

  constructor(name: string, description: string, color: string) {
    this.name = name;
    this.description = description;
    this.color = color;
  }
}
