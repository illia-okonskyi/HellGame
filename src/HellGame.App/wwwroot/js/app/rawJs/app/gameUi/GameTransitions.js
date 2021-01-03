export class GameTransitions {
    constructor(onTransition) {
        this.divGameTransitions = document.querySelector("#divGameTransitions");

        this.onTransition = onTransition;
    }

    setLabel(header) {
        this.divGameTransitions.querySelector("h5").textContent = header;
    }

    setTransitions(transitions) {
        this.clearTransitions();
        transitions.forEach((transition) => this.addTransition(transition));
    }

    clearTransitions() {
        let lis = this.divGameTransitions.querySelectorAll("button");
        lis.forEach(li => li.remove());
    }

    addTransition(transition) {
        if (!transition.isVisible) {
            return;
        }

        let li = document.createElement("button");
        li.classList.add("list-group-item", "list-group-item-action", "list-group-item-info");
        li.dataset.transitionId = transition.id;
        li.innerHTML = transition.text;
        if (!transition.isEnabled) {
            li.disabled = true;
        }

        li.addEventListener("click", (event) => this.onLiClicked(event));

        let listGroup = this.divGameTransitions.querySelector(".list-group");
        listGroup.append(li);
    }

    onLiClicked(event) {
        const transitionId = event.target.closest("button").dataset.transitionId;
        this.onTransition(transitionId);
    }
}