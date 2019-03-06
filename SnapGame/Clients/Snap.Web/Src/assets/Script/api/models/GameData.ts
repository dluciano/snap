import { PlayerTurn } from "./PlayerTurn";
import { GameState } from "./GameState";
import { IGameData } from "./IGameData";
export abstract class GameData implements IGameData {
    firstPlayer?: PlayerTurn | undefined;
    currentTurn?: PlayerTurn | undefined;
    turns?: PlayerTurn[] | undefined;
    id?: number;
    currentState?: GameState;
    constructor(data?: IGameData) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
    init(data?: any): void {
        if (data) {
            this.firstPlayer = data.firstPlayer ? PlayerTurn.fromJS(data.firstPlayer) : <any>undefined;
            this.currentTurn = data.currentTurn ? PlayerTurn.fromJS(data.currentTurn) : <any>undefined;
            if (data.turns && data.turns.constructor === Array) {
                this.turns = [] as any;
                for (let item of data.turns) {
                    this.turns!.push(PlayerTurn.fromJS(item));
                }
            }
            this.id = data.id;
            this.currentState = data.currentState;
        }
    }
    static fromJS(data: any): GameData {
        data = typeof data === "object" ? data : {};
        throw new Error("The abstract class 'GameData' cannot be instantiated.");
    }
    toJSON(data?: any): void {
        data = typeof data === "object" ? data : {};
        data.firstPlayer = this.firstPlayer ? this.firstPlayer.toJSON() : <any>undefined;
        data.currentTurn = this.currentTurn ? this.currentTurn.toJSON() : <any>undefined;
        if (this.turns && this.turns.constructor === Array) {
            data.turns = [];
            for (let item of this.turns) {
                data.turns.push(item.toJSON());
            }
        }
        data.id = this.id;
        data.currentState = this.currentState;
        return data;
    }
}
