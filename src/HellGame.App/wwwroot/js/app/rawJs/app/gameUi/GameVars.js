export class GameVars {
    constructor() {
        this.divGameVars = document.querySelector("#divGameVars");
    }

    setLabel(header) {
        this.divGameVars.querySelector("h5").textContent = header;
    }

    setGameVars(vars) {
        this.clearGameVars();
        vars.forEach((aVar) => this.addGameVar(aVar));
    }

    clearGameVars() {
        let trs = this.divGameVars.querySelectorAll("tbody > tr");
        trs.forEach(tr => tr.remove());
    }

    addGameVar(aVar) {
        let tr = document.createElement("tr");

        // Name cell
        let strong = document.createElement("strong");
        strong.textContent = aVar.name;
        let tdName = document.createElement("td");
        tdName.append(strong);
        tr.append(tdName);

        // Value cell
        let tdValue = document.createElement("td");
        tdValue.textContent = aVar.value;
        tr.append(tdValue);

        let tbody = this.divGameVars.querySelector("tbody");
        tbody.append(tr);
    }
}