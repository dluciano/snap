"use strict";

const { ccclass, property } = cc._decorator;

@ccclass
export default class SnapGame extends cc.Component {
    public roomId: number = -1;
    public createdBy: string = "";
    public gameId: number = -1;
    public currentPlayer: string = "";
    public playerData: any = {};
}
