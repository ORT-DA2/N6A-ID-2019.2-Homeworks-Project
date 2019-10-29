import { Pipe, PipeTransform } from '@angular/core';
import { Homework } from '../models/Homework';

@Pipe({
  name: 'homeworksFilter'
})
export class HomeworksFilterPipe implements PipeTransform {

  transform(list: Array<Homework>, arg: string): Array<Homework> {
    return list.filter(
      x => x.description.toLocaleLowerCase()
        .includes(arg.toLocaleLowerCase())
    );
  }
}