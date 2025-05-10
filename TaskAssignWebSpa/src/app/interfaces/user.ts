import { UserType } from "../enums/user-type";

export interface User {
  id: number;
  name: string;
  type: UserType;
}
