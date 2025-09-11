import { Pipe, PipeTransform } from '@angular/core';
import { SelectionBox } from 'src/app/models/SelectionBox';

@Pipe({
    name: 'idToValue',
    standalone: false
})
export class IdToValuePipe implements PipeTransform {
  transform(id: string, dataSource: SelectionBox[]): any {
    if (!id || !dataSource) {
      return null;
    }

    const item = dataSource.find(item => item.Id.toString() === id);
    return item ? item.Valoare : null;
  }
}