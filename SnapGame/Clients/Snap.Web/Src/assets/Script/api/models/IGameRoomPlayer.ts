import { GameRoom } from "./GameRoom";
import { Player } from "./Player";
export interface IGameRoomPlayer {
    player?: Player | undefined;
    isViewer?: boolean;
    gameRoom?: GameRoom | undefined;
    id?: number;
    playerId?: number;
    roomId?: number;
}
