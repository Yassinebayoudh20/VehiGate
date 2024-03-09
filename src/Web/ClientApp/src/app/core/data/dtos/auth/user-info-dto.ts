// user.model.ts
export class UserInfo {
  id: number;
  username: string;
  email: string;
  roles: string[] | string;

  constructor(id: number, username: string, email: string, roles: string[] | string) {
    this.id = id;
    this.username = username;
    this.email = email;
    this.roles = roles;
  }
}
