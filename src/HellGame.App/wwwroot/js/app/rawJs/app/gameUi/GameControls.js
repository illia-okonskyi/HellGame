export class GameControls {
    constructor(onReStartGame, onLoadGame, onSaveGame, onExitGame, onSetLocaleRequested) {
        this.onReStartGame = onReStartGame;
        this.onLoadGame = onLoadGame;
        this.onSaveGame = onSaveGame;
        this.onExitGame = onExitGame;
        this.onSetLocaleRequested = onSetLocaleRequested;

        this.userNamePrompt = "Enter username";

        this.divGameControls = document.querySelector("#divGameControls");

        this.btnRestart = document.querySelector("#btnRestart");
        this.btnSave = document.querySelector("#btnSave");
        this.btnLoad = document.querySelector("#btnLoad");
        this.btnExit = document.querySelector("#btnExit");

        this.btnRestart.addEventListener("click", (event) => this.onReStartClick(event));
        this.btnSave.addEventListener("click", (event) => this.onSaveClick(event));
        this.btnLoad.addEventListener("click", (event) => this.onLoadClick(event));
        this.btnExit.addEventListener("click", (event) => this.onExitClick(event));

        this.btnGroupLocales = document.querySelector("#btnGroupLocales");

        let localeButtons = this.btnGroupLocales.querySelectorAll("button");
        for (let localeButton of localeButtons) {
            localeButton.addEventListener("click", (event) => this.onLocaleButtonClicked(event));
        }

        this.btnSave.disabled = true;

        localeButtons[0].click();
    }

    setStatus(status) {
        this.lblStatus.textContent = status;
    }

    setLabels(header, btnRestart, btnSave, btnLoad, btnExit, userNamePrompt) {
        this.divGameControls.querySelector("h5").textContent = header;
        this.btnRestart.textContent = btnRestart;
        this.btnSave.textContent = btnSave;
        this.btnLoad.textContent = btnLoad;
        this.btnExit.textContent = btnExit;
        this.userNamePrompt = userNamePrompt;
    }

    setSaveEnabled() {
        this.btnSave.disabled = false;
    }

    onLocaleButtonClicked(event) {
        const locale = event.target.dataset.locale;
        const target = event.target;

        if (target.classList.contains("btn-primary")) {
            return;
        }

        this.onSetLocaleRequested(locale);

        let localeButtons = this.btnGroupLocales.querySelectorAll("button");
        for (let localeButton of localeButtons) {
            localeButton.classList.remove("btn-primary");
            localeButton.classList.remove("btn-secondary");

            const targetClass = localeButton === target
                ? "btn-primary"
                : "btn-secondary";

            localeButton.classList.add(targetClass);
        }
    }

    onReStartClick() {
        const userName = prompt(this.userNamePrompt, "userName");
        if (!userName) {
            return;
        }
        this.onReStartGame(userName);
    }

    onLoadClick() {
        let inputElem = document.createElement("input");
        inputElem.type = "file";
        inputElem.accept = ".dat"

        inputElem.onchange = e => {
            const file = e.target.files[0];
            let reader = new FileReader();
            reader.readAsText(file, 'UTF-8');
            reader.onload = readerEvent => {
                const fileData = readerEvent.target.result;
                this.onLoadGame(fileData);
            }
        }

        inputElem.click();
    }

    onSaveClick() {
        this.onSaveGame();
    }

    onExitClick() {
        this.onExitGame();
    }
}