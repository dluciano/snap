import { PlayerTurn } from "./PlayerTurn";
import { GameState } from "./GameState";
export interface IGameData {
    firstPlayer?: PlayerTurn | undefined;
    currentTurn?: PlayerTurn | undefined;
    turns?: PlayerTurn[] | undefined;
    id?: number;
    currentState?: GameState;
}
