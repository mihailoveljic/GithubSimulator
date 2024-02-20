
import { State } from "./State";

export class Milestone {
  id: string | undefined;
  title: string | undefined;
  description: string | undefined;
  dueDate: Date | undefined;
  state: State | undefined;
}
