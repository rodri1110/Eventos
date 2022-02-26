import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../utils/Constants';

@Pipe({
  name: 'DatePipes'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  override transform(value: any, args?: string): any {
    return super.transform(value, Constants.DATE_TIME_FMT);
  }

}
