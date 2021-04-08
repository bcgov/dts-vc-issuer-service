import { HttpHeaders } from '@angular/common/http';

export class ApiHttpResponse<T> {
  public readonly status: number;
  public readonly headers: HttpHeaders;
  public readonly result: T;
  public readonly message?: string;

  public constructor(status: number, headers: HttpHeaders, result: T, message?: string) {
    this.status = status;
    this.headers = headers;
    this.result = result;
    this.message = message;
  }
}
