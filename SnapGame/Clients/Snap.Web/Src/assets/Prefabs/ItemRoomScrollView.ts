"use strict";

import "../Script/ConnectionManager";
import ConnectionManager from "../Script/ConnectionManager";
const { ccclass, property } = cc._decorator;

@ccclass
export default class ItemRoomScrollView extends cc.Component {
    id: number = -1;
    @property()
    roomSceneName: string = "";
    onLoad(): void {
        this.node.on("mousedown", (event) => {
            ConnectionManager.getInstance().joinGame(this.id, false);
        });
    }
}
