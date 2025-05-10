import { CommonTask } from "./common-task";

export interface DeploymentTask extends CommonTask {
  deploymentScope: string;
  date: Date;
}
