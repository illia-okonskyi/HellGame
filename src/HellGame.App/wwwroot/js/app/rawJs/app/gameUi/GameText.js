export class GameText {
    constructor() {
        this.divGameText = document.querySelector("#divGameText");
    }

    setLabel(header) {
        this.divGameText.querySelector("h5").textContent = header;
    }

    setGameText(gameText) {
        let cardBody = this.divGameText.querySelector(".card-body");
        cardBody.innerHTML = gameText;
    }
}