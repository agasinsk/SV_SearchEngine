import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'splitCamelCase' })
export class SplitCamelCasePipe implements PipeTransform {
  transform(content: string): any {
    if (typeof content !== 'string') {
      return content;
    }
    return content.split(/([A-Z][a-z]+)/).filter(function (e: string) {
      return e;
    }).join(' ');
  }
}
