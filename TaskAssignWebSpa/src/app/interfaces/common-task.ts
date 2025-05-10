import { TaskStatus } from "../enums/task-status";
import { TaskType } from "../enums/task-type";
import { User } from "./user";

export interface CommonTask {
  id: number;
  description: string;
  difficultyScale: 1 | 2 | 3 | 4 | 5;
  type: TaskType;
  status: TaskStatus;
  userId: number | null;
  user: User;
}
