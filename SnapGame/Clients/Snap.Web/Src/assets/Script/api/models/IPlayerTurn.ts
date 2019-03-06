import { Player } from "./Player";
import { PlayerTurn } from "./PlayerTurn";
import { GameData } from "./GameData";
export interface IPlayerTurn {
    player?: Player | undefined;
    next?: PlayerTurn | undefined;
    firstPlayers?: GameData[] | undefined;
    currentTurns?: GameData[] | undefined;
    id?: number;
}
