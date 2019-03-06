import { GameRoom } from "./GameRoom";
import { GameRoomPlayer } from "./GameRoomPlayer";
import { PlayerTurn } from "./PlayerTurn";
import { IPlayer } from "./IPlayer";
export class Player implements IPlayer {
    username?: string | undefined;
    gameRoomPlayers?: GameRoomPlayer[] | undefined;
    playerTurns?: PlayerTurn[] | undefined;
    id?: number;
    createdRooms?: GameRoom[] | undefined;
    constructor(data?: IPlayer) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    (<any>this)[property] = (<any>data)[property];
                }
            }
        }
    }
    init(data?: any): IPlayer {
        if (data) {
            this.username = data.username;
            if (data.gameRoomPlayers && data.gameRoomPlayers.constructor === Array) {
                this.gameRoomPlayers = [] as any;
                for (let item of data.gameRoomPlayers) {
                    this.gameRoomPlayers!.push(GameRoomPlayer.fromJS(item));
                }
            }
            if (data.playerTurns && data.playerTurns.constructor === Array) {
                this.playerTurns = [] as any;
                for (let item of data.playerTurns) {
                    this.playerTurns!.push(PlayerTurn.fromJS(item));
                }
            }
            this.id = data.id;
            if (data.createdRooms && data.createdRooms.constructor === Array) {
                this.createdRooms = [] as any;
                for (let item of data.createdRooms) {
                    this.createdRooms!.push(GameRoom.fromJS(item));
                }
            }
        }
        return this;
    }

    toJSON(data?: any): any {
        data = typeof data === "object" ? data : {};
        data.username = this.username;
        if (this.gameRoomPlayers && this.gameRoomPlayers.constructor === Array) {
            data.gameRoomPlayers = [];
            for (let item of this.gameRoomPlayers) {
                data.gameRoomPlayers.push(item.toJSON());
            }
        }
        if (this.playerTurns && this.playerTurns.constructor === Array) {
            data.playerTurns = [];
            for (let item of this.playerTurns) {
                data.playerTurns.push(item.toJSON());
            }
        }
        data.id = this.id;
        if (this.createdRooms && this.createdRooms.constructor === Array) {
            data.createdRooms = [];
            for (let item of this.createdRooms) {
                data.createdRooms.push(item.toJSON());
            }
        }
        return data;
    }
}
