export class StatusBar {
    constructor() {
        this.divStatusBar = document.querySelector("#divStatusBar");
        this.lblStatus = this.divStatusBar.querySelector("label");
    }

    setStatus(status) {
        this.lblStatus.textContent = status;
    }

    setLabel(header) {
        this.divStatusBar.querySelector("h5").textContent = header;
    }
}