import { GameRoom } from "./GameRoom";
import { GameRoomPlayer } from "./GameRoomPlayer";
import { PlayerTurn } from "./PlayerTurn";

export interface IInitiable<TEntity> {
    init(data: any): TEntity;
}

export interface IPlayer extends IInitiable<IPlayer> {
    username?: string | undefined;
    gameRoomPlayers?: GameRoomPlayer[] | undefined;
    playerTurns?: PlayerTurn[] | undefined;
    id?: number;
    createdRooms?: GameRoom[] | undefined;
}
