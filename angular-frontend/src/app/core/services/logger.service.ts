import { Injectable } from '@angular/core';

import { environment } from '@env/environment';

type LogLevel = 'log' | 'info' | 'warn' | 'error' | 'trace';

enum LogLevelEnum {
  LOG = 'log',
  INFO = 'info',
  WARN = 'warn',
  ERROR = 'error',
  TRACE = 'trace'
}

@Injectable({
  providedIn: 'root'
})
export class LoggerService {
  /**
   * @description
   * General output of logging information.
   */
  public log(msg: string, ...data: any[]) {
    this.print(LogLevelEnum.LOG, msg, data);
  }

  /**
   * @description
   * Informative output of logging information.
   */
  public info(msg: string, ...data: any[]) {
    this.print(LogLevelEnum.INFO, msg, data);
  }

  /**
   * @description
   * Outputs a warning message.
   */
  public warn(msg: string, ...data: any[]) {
    this.print(LogLevelEnum.WARN, msg, data);
  }

  /**
   * @description
   * Outputs an error message.
   */
  public error(msg: string, ...data: any[]) {
    this.print(LogLevelEnum.ERROR, msg, data);
  }

  /**
   * @description
   * Outputs a stack trace.
   */
  public trace(msg: string, ...data: any[]) {
    this.print(LogLevelEnum.TRACE, msg, data);
  }

  /**
   * @description
   * Pretty print JSON.
   */
  public pretty(msg: string, ...data: any[]) {
    this.print(LogLevelEnum.LOG, msg, [JSON.stringify(data, null, '\t')]);
  }

  /**
   * @description
   * Prints the logging information, but ONLY if not in production.
   */
  private print(type: LogLevelEnum, msg: string, data: any[] = []) {
    if (!environment.production || type === 'error' || type === 'warn') {
      const message = this.colorize(type, msg);

      if (msg && data.length) {
        console[type](...message, ...data);
      } else if (msg && !data.length) {
        console[type](...message);
      } else if (!msg && data.length) {
        console[type](data);
      } else {
        console.error('Logger parameters are invalid: ', { type, msg, data });
      }
    }
  }

  /**
   * @description
   * Apply colour to the console message, otherwise the use
   * the default.
   */
  private colorize(type: LogLevelEnum, msg: string): string[] {
    let color = '';

    switch (type) {
      case LogLevelEnum.LOG:
        color = 'Yellow';
        break;
      case LogLevelEnum.INFO:
        color = 'DodgerBlue';
        break;
      case LogLevelEnum.WARN:
        color = 'Orange';
        break;
      case LogLevelEnum.TRACE:
      case LogLevelEnum.ERROR:
        color = 'Red';
        break;
    }

    if (color) {
      color = `color:${ color }`;
    }

    return [`%c${ msg }`, color];
  }
}
