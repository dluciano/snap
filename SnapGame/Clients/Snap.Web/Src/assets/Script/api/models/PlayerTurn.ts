import { Player } from "./Player";
import { GameData } from "./GameData";
import { IPlayerTurn } from "./IPlayerTurn";
export class PlayerTurn implements IPlayerTurn {
    player?: Player | undefined;
    next?: PlayerTurn | undefined;
    firstPlayers?: GameData[] | undefined;
    currentTurns?: GameData[] | undefined;
    id?: number;
    constructor(data?: IPlayerTurn) {
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
            this.next = data.next ? PlayerTurn.fromJS(data.next) : <any>undefined;
            if (data.firstPlayers && data.firstPlayers.constructor === Array) {
                this.firstPlayers = [] as any;
                for (let item of data.firstPlayers) {
                    this.firstPlayers!.push(GameData.fromJS(item));
                }
            }
            if (data.currentTurns && data.currentTurns.constructor === Array) {
                this.currentTurns = [] as any;
                for (let item of data.currentTurns) {
                    this.currentTurns!.push(GameData.fromJS(item));
                }
            }
            this.id = data.id;
        }
    }
    static fromJS(data: any): PlayerTurn {
        data = typeof data === "object" ? data : {};
        const result: PlayerTurn = new PlayerTurn();
        result.init(data);
        return result;
    }
    toJSON(data?: any): any {
        data = typeof data === "object" ? data : {};
        data.player = this.player ? this.player.toJSON() : <any>undefined;
        data.next = this.next ? this.next.toJSON() : <any>undefined;
        if (this.firstPlayers && this.firstPlayers.constructor === Array) {
            data.firstPlayers = [];
            for (let item of this.firstPlayers) {
                data.firstPlayers.push(item.toJSON());
            }
        }
        if (this.currentTurns && this.currentTurns.constructor === Array) {
            data.currentTurns = [];
            for (let item of this.currentTurns) {
                data.currentTurns.push(item.toJSON());
            }
        }
        data.id = this.id;
        return data;
    }
}
