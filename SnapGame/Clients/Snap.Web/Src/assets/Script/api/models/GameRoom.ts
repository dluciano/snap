import { GameData } from "./GameData";
import { Player } from "./Player";
import { GameRoomPlayer } from "./GameRoomPlayer";
import { IGameRoom } from "./IGameRoom";
export class GameRoom implements IGameRoom {
    roomPlayers?: GameRoomPlayer[] | undefined;
    id?: number;
    canJoin?: boolean;
    gameIdentifier?: string;
    gamesData?: GameData | undefined;
    createdBy?: Player | undefined;
    constructor(data?: IGameRoom) {
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
            if (data.roomPlayers && data.roomPlayers.constructor === Array) {
                this.roomPlayers = [] as any;
                for (let item of data.roomPlayers) {
                    this.roomPlayers!.push(GameRoomPlayer.fromJS(item));
                }
            }
            this.id = data.id;
            this.canJoin = data.canJoin;
            this.gameIdentifier = data.gameIdentifier;
            this.gamesData = data.gamesData ? GameData.fromJS(data.gamesData) : <any>undefined;
            this.createdBy = data.createdBy ? Player.fromJS(data.createdBy) : <any>undefined;
        }
    }
    static fromJS(data: any): GameRoom {
        data = typeof data === "object" ? data : {};
        const result:GameRoom = new GameRoom();
        result.init(data);
        return result;
    }
    toJSON(data?: any):any {
        data = typeof data === "object" ? data : {};
        if (this.roomPlayers && this.roomPlayers.constructor === Array) {
            data.roomPlayers = [];
            for (let item of this.roomPlayers) {
                data.roomPlayers.push(item.toJSON());
            }
        }
        data.id = this.id;
        data.canJoin = this.canJoin;
        data.gameIdentifier = this.gameIdentifier;
        data.gamesData = this.gamesData ? this.gamesData.toJSON() : <any>undefined;
        data.createdBy = this.createdBy ? this.createdBy.toJSON() : <any>undefined;
        return data;
    }
}
