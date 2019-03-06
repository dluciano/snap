import { GameData } from "./GameData";
import { Player } from "./Player";
import { GameRoomPlayer } from "./GameRoomPlayer";
export interface IGameRoom {
    roomPlayers?: GameRoomPlayer[] | undefined;
    id?: number;
    canJoin?: boolean;
    gameIdentifier?: string;
    gamesData?: GameData | undefined;
    createdBy?: Player | undefined;
}
