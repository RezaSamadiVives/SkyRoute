
const sessionDurationSeconds = 30 * 60;
let remainingTime = parseInt(sessionStorage.getItem("remainingTime")) || sessionDurationSeconds;
let timerInterval;

function formatTime(seconds) {
    const m = Math.floor(seconds / 60).toString().padStart(2, '0');
    const s = (seconds % 60).toString().padStart(2, '0');
    return `${m}:${s}`;
}

function updateTimerUI(timerDisplay, modalCountdown, wrapper) {
    if (remainingTime <= 0) {
        clearInterval(timerInterval);
        modalCountdown.textContent = "00:00";

        const modalInstance = bootstrap.Modal.getOrCreateInstance(document.getElementById("sessionModal"));

        //Verberg modal (veiligheid, mocht hij open staan)
        modalInstance.hide();

        //Sessie wissen & pagina herladen
        fetch("/Session/Clear", { method: "POST" }).then(() => {
            sessionStorage.removeItem("sessionActive");
            sessionStorage.removeItem("remainingTime");
            location.reload();
        });

        return;
    }

    // Normale countdown flow
    timerDisplay.textContent = formatTime(remainingTime);
    modalCountdown.textContent = formatTime(remainingTime);

    if (remainingTime <= 60) {
        wrapper.classList.remove('alert-warning');
        wrapper.classList.add('alert-danger');
    }

    if (remainingTime === 5 * 60) {
        bootstrap.Modal.getOrCreateInstance(document.getElementById("sessionModal")).show();
    }

    // Sla bij elke tik op in sessionStorage
    sessionStorage.setItem("remainingTime", remainingTime.toString());

    remainingTime--;
}

function startSessionTimer() {
    const wrapper = document.getElementById("session-timer-wrapper");
    const display = document.getElementById("session-timer");
    const modalCountdown = document.getElementById("modal-countdown");
    const extendButton = document.getElementById("extend-session");
    const restartButton = document.getElementById("restart-session");

    if (!wrapper || !display || !modalCountdown) {
        console.warn("Session timer elementen niet gevonden.");
        return;
    }

    console.log("Sessietimer gestart.");
    wrapper.style.display = "block";
    display.textContent = formatTime(remainingTime);
    modalCountdown.textContent = formatTime(remainingTime);

    timerInterval = setInterval(() => updateTimerUI(display, modalCountdown, wrapper), 1000);

    extendButton?.addEventListener("click", () => {
        fetch("/Session/KeepAlive", { method: "POST" }).then(() => {
            clearInterval(timerInterval);
            remainingTime = sessionDurationSeconds;
            sessionStorage.setItem("remainingTime", remainingTime.toString());
            display.textContent = formatTime(remainingTime);
            modalCountdown.textContent = formatTime(remainingTime);


            wrapper.classList.remove('alert-danger');
            wrapper.classList.add('alert-warning');
            timerInterval = setInterval(() => updateTimerUI(display, modalCountdown, wrapper), 1000);
            bootstrap.Modal.getOrCreateInstance(document.getElementById("sessionModal")).hide();
        });
    });

    restartButton?.addEventListener("click", () => {
        fetch("/Session/Clear", { method: "POST" }).then(() => {
            sessionStorage.removeItem("sessionActive");
            location.reload();
        });
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const sessionActive = sessionStorage.getItem("sessionActive");
    console.log("DOM geladen, sessionActive:", sessionActive);
    if (sessionActive === "true") {
        startSessionTimer();
    } else {
        console.log("Geen actieve sessie. Timer wordt niet gestart.");
    }
});
