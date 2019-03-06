import { GameRoom } from "./GameRoom";
import { Player } from "./Player";
import { IGameRoomPlayer } from "./IGameRoomPlayer";
export class GameRoomPlayer implements IGameRoomPlayer {
    player?: Player | undefined;
    isViewer?: boolean;
    gameRoom?: GameRoom | undefined;
    id?: number;
    playerId?: number;
    roomId?: number;
    constructor(data?: IGameRoomPlayer) {
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
            this.player = data.player ? Player.fromJS(data.player) : <any>undefined;
            this.isViewer = data.isViewer;
            this.gameRoom = data.gameRoom ? GameRoom.fromJS(data.gameRoom) : <any>undefined;
            this.id = data.id;
            this.playerId = data.playerId;
            this.roomId = data.roomId;
        }
    }
    static fromJS(data: any): GameRoomPlayer {
        data = typeof data === "object" ? data : {};
        const result: GameRoomPlayer = new GameRoomPlayer();
        result.init(data);
        return result;
    }
    toJSON(data?: any): any {
        data = typeof data === "object" ? data : {};
        data.player = this.player ? this.player.toJSON() : <any>undefined;
        data.isViewer = this.isViewer;
        data.gameRoom = this.gameRoom ? this.gameRoom.toJSON() : <any>undefined;
        data.id = this.id;
        data.playerId = this.playerId;
        data.roomId = this.roomId;
        return data;
    }
}
