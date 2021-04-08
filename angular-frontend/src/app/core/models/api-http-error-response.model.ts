export class ApiHttpErrorResponse {
  public readonly status: number;
  public readonly errors: any;
  public readonly message?: string;

  public constructor(status: number, errors: any, message?: string) {
    this.status = status;
    this.errors = errors;
    this.message = message;
  }
}
