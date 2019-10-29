import { IExercise } from './IExercise';

export interface IHomework {
    id:string;
    description:string;
    score:number;
    dueDate:Date;
    exercises:Array<IExercise>;
}