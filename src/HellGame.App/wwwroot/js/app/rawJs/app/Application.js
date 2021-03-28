import { ApiClient } from "./ApiClient.js";

import { GameControls } from "./gameUi/GameControls.js";
import { GameText } from "./gameUi/GameText.js";
import { GameImage } from "./gameUi/GameImage.js";
import { GameVars } from "./gameUi/GameVars.js";
import { GameTransitions } from "./gameUi/GameTransitions.js";
import { StatusBar } from "./gameUi/StatusBar.js";

import { BlobSaver } from "./BlobSaver.js"
import { Pinger } from "./Pinger.js";

export class Application {
    constructor(config) {
        this.gameActive = false;
        this.config = config;
        this.apiClient = new ApiClient(config);

        this.gameControls = new GameControls(
            userName => this.reStartGame(userName),
            fileData => this.loadGame(fileData),
            () => this.saveGame(),
            () => this.exitGame(),
            locale => this.setLocale(locale));
        this.gameText = new GameText();
        this.gameImage = new GameImage();
        this.gameVars = new GameVars();
        this.gameTransitions = new GameTransitions(id => this.transition(id));
        this.statusBar = new StatusBar();

        //this.pinger = new Pinger(this.apiClient, error => this.onPingError(error));
    }

    reStartGame(userName) {
        this.reStartGameInternal(userName)
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    loadGame(fileData) {
        this.loadGameInternal(fileData)
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    saveGame() {
        this.saveGameInternal()
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    exitGame() {
        if (!this.gameActive) {
            return;
        }

        this.exitGameInternal()
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    transition(key) {
        this.transitionInternal(key)
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    setLocale(locale) {
        this.setLocaleInternal(locale)
            .catch((error) => this.statusBar.setStatus(`Error: ${error.message}`));
    }

    onPingError(error) {
        this.statusBar.setStatus(`Error: ${error.message}`);
    }

    async reStartGameInternal(userName) {
        await this.apiClient.startGame(userName);
        this.gameActive = true;
        await this.applyGameState();
        this.gameControls.setSaveEnabled();
    }

    async saveGameInternal() {
        const fileData = await this.apiClient.saveGame();
        const blob = new Blob([fileData], { type: 'text/plain' });
        const fileName = "HellGameSaveGame.dat";
        const blobSaver = new BlobSaver(blob, fileName);
        blobSaver.save();
    }

    async loadGameInternal(fileData) {
        await this.apiClient.loadGame(fileData);
        this.gameActive = true;
        await this.applyGameState();
        this.gameControls.setSaveEnabled();
    }

    async exitGameInternal() {
        await this.apiClient.exitGame();
        this.gameActive = true;
        await this.applyGameState();
    }

    async applyGameState() {
        const state = await this.apiClient.getGameState();
        const gameText = (await this.apiClient.getTextAsset(state.textAssetKey)).data;
        const gameImgData = await this.apiClient.getImageAsset(state.imageAssetKey);
        const vars = await this.apiClient.getVars();
        const stateTransitions = state.transitions;
        let transitions = [];
        for (const t of stateTransitions) {
            const text = (await this.apiClient.getTextAsset(t.textAssetKey)).data;
            transitions.push({
                key: t.key,
                text: text,
                isEnabled: t.isEnabled,
                isVisible: t.isVisible
            });
        }

        this.gameText.setGameText(gameText);
        this.gameImage.setGameImage(gameImgData.mediaType, gameImgData.sourceUrl, gameImgData.data);
        this.gameVars.setGameVars(vars);
        this.gameTransitions.setTransitions(transitions);
        this.statusBar.setStatus("OK");
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
        const userNamePrompt = (await this.apiClient.getTextAsset("ui.GameControls.userNamePrompt")).data;

        const aSource = (await this.apiClient.getTextAsset("ui.GameImage.aSource")).data;

        this.gameControls.setLabels(
            lblGameControls,
            btnRestart,
            btnSave,
            btnLoad,
            btnExit,
            userNamePrompt);

        this.gameText.setLabel(lblGameText);
        this.gameImage.setLabels(lblGameImage, aSource);
        this.gameVars.setLabel(lblGameVars);
        this.gameTransitions.setLabel(lblGameTransitions);
        this.statusBar.setLabel(lblStatusBar);

        this.statusBar.setStatus("OK");

        if (this.gameActive) {
            await this.applyGameState();
        }
    }

    async transitionInternal(key) {
        await this.apiClient.transition(key);
        await this.applyGameState();
    }

    async testUi() {
        const gameText = (await this.apiClient.getTextAsset("_test.text")).data;

        this.gameText.setGameText(gameText);

        const imgData = await this.apiClient.getImageAsset("_test.image");
        this.gameImage.setGameImage(imgData.mediaType, imgData.sourceUrl, imgData.data);

        const vars = await this.apiClient.getVars();
        this.gameVars.setGameVars(vars);

        this.gameTransitions.setTransitions([
            { key: "t0", text: "Transition 0", isEnabled: true, isVisible: true },
            { key: "t1", text: "Transition 1", isEnabled: true, isVisible: false },
            { key: "t2", text: "Transition 2", isEnabled: false, isVisible: true },
            { key: "t3", text: "Transition 3", isEnabled: true, isVisible: true }
        ]);
    }
}