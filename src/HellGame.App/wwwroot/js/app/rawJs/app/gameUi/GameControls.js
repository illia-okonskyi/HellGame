export class GameControls {
    constructor(onSetLocaleRequested) {
        this.onSetLocaleRequested = onSetLocaleRequested;

        this.divGameControls = document.querySelector("#divGameControls");

        this.btnRestart = document.querySelector("#btnRestart");
        this.btnSave = document.querySelector("#btnSave");
        this.btnLoad = document.querySelector("#btnLoad");
        this.btnExit = document.querySelector("#btnExit");

        this.lblStatus = document.querySelector("#lblStatus");

        this.btnGroupLocales = document.querySelector("#btnGroupLocales");

        let localeButtons = this.btnGroupLocales.querySelectorAll("button");
        for (let localeButton of localeButtons) {
            localeButton.addEventListener("click", (event) => this.onLocaleButtonClicked(event));
        }

        localeButtons[0].click();
    }

    setStatus(status) {
        this.lblStatus.textContent = status;
    }

    setLabels(header, btnRestart, btnSave, btnLoad, btnExit) {
        this.divGameControls.querySelector("h5").textContent = header;
        this.btnRestart.textContent = btnRestart;
        this.btnSave.textContent = btnSave;
        this.btnLoad.textContent = btnLoad;
        this.btnExit.textContent = btnExit;
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
}