import { ApiClient } from "./ApiClient.js";

import { GameControls } from "./gameUi/GameControls.js";
import { GameText } from "./gameUi/GameText.js";
import { GameImage } from "./gameUi/GameImage.js";
import { GameVars } from "./gameUi/GameVars.js";
import { GameTransitions } from "./gameUi/GameTransitions.js";
import { StatusBar } from "./gameUi/StatusBar.js";

import { Pinger } from "./Pinger.js";

export class Application {
    constructor(config) {
        this.config = config;
        this.apiClient = new ApiClient(config);

        this.gameControls = new GameControls(locale => this.setLocale(locale));
        this.gameText = new GameText();
        this.gameImage = new GameImage();
        this.gameVars = new GameVars();
        this.gameTransitions = new GameTransitions(id => this.transition(id));
        this.statusBar = new StatusBar();

        this.pinger = new Pinger(this.apiClient, error => this.onPingError(error));

        this.testUi();
    }

    onPingError(error) {
        this.statusBar.setStatus(`Error: ${error.message}`);
    }

    setLocale(locale) {
        this.setLocaleInternal(locale)
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    async setLocaleInternal(locale) {
        await this.apiClient.setLocale(locale);

        const lblGameControls = (await this.apiClient.getTextAsset("ui.Labels.lblGameControls")).data;
        const lblGameText = (await this.apiClient.getTextAsset("ui.Labels.lblGameText")).data;
        const lblGameImage = (await this.apiClient.getTextAsset("ui.Labels.lblGameImage")).data;
        const lblGameVars = (await this.apiClient.getTextAsset("ui.Labels.lblGameVars")).data;
        const lblGameTransitions = (await this.apiClient.getTextAsset("ui.Labels.lblGameTransitions")).data;
        const lblStatusBar = (await this.apiClient.getTextAsset("ui.Labels.lblStatusBar")).data;

        const btnRestart = (await this.apiClient.getTextAsset("ui.GameControls.btnRestart")).data;
        const btnSave = (await this.apiClient.getTextAsset("ui.GameControls.btnSave")).data;
        const btnLoad = (await this.apiClient.getTextAsset("ui.GameControls.btnLoad")).data;
        const btnExit = (await this.apiClient.getTextAsset("ui.GameControls.btnExit")).data;

        const aSource = (await this.apiClient.getTextAsset("ui.GameImage.aSource")).data;

        this.gameControls.setLabels(
            lblGameControls,
            btnRestart,
            btnSave,
            btnLoad,
            btnExit);

        this.gameText.setLabel(lblGameText);
        this.gameImage.setLabels(lblGameImage, aSource);
        this.gameVars.setLabel(lblGameVars);
        this.gameTransitions.setLabel(lblGameTransitions);
        this.statusBar.setLabel(lblStatusBar);

        await this.testUi();
    }

    transition(id) {
        this.statusBar.setStatus(`TRANSITION: ${id}`);
    }

    async testUi() {
        this.gameText.setGameText("Game <b>test</b> text");

        const imgData = await this.apiClient.getImageAsset("_test.image");
        this.gameImage.setGameImage(imgData.mediaType, imgData.sourceUrl, imgData.data);
        this.gameVars.setGameVars([
            { name: "var 1", value: "val1" },
            { name: "var 2", value: "val2" },
            { name: "var 3", value: "val3" }
        ]);

        this.gameTransitions.setTransitions([
            { id: "t0", text: "Transition 0", isEnabled: true, isVisible: true },
            { id: "t1", text: "Transition 1", isEnabled: true, isVisible: false },
            { id: "t2", text: "Transition 2", isEnabled: false, isVisible: true },
            { id: "t3", text: "Transition 3", isEnabled: true, isVisible: true }
        ]);
    }
}