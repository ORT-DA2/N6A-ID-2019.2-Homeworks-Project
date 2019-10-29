import { IExercise } from "./IExercise";

export class Exercise implements IExercise {
    id: string;
    problem: string;
    score: number;

    constructor(id:string = "", problem:string = "", score:number = 0) {
        this.id = id;
        this.problem = problem;
        this.score = score;
    }
}