import { Pipe, PipeTransform } from '@angular/core';

import * as moment from 'moment';

@Pipe({
  name: 'formatDate'
})
export class FormatDatePipe implements PipeTransform {
  public transform(date: string, format: string = 'D MMM YYYY'): string {
    return (date)
      ? moment(date).format(format)
      : date;
  }
}
