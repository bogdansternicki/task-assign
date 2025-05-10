import { CommonTask } from "./common-task";

export interface MaintenanceTask extends CommonTask {
  services: string;
  servers: string;
  date: Date;
}
