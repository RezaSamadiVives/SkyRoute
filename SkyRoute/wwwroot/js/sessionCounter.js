document.addEventListener("DOMContentLoaded", function () {
    const sessionTimeoutSeconds = 30 * 60; // 30 minuten = 1800 seconden
    let remainingTime = sessionTimeoutSeconds;

    const timerWrapper = document.getElementById("session-timer-wrapper");
    const timerDisplay = document.getElementById("session-timer");

    if (!timerWrapper || !timerDisplay) return; // veilig afbreken als DOM-elementen ontbreken

    timerWrapper.style.display = "block"; // toon de timer

    function formatTime(seconds) {
        const minutes = Math.floor(seconds / 60).toString().padStart(2, '0');
        const secs = (seconds % 60).toString().padStart(2, '0');
        return `${minutes}:${secs}`;
    }

    function updateTimer() {
        if (remainingTime <= 0) {
            timerWrapper.innerHTML = "⚠️ Je sessie is verlopen. Laad de pagina opnieuw om verder te gaan.";
            return;
        }

        timerDisplay.textContent = formatTime(remainingTime);

        if (remainingTime <= 60) {
            timerWrapper.classList.remove('alert-warning');
            timerWrapper.classList.add('alert-danger');
        }

        remainingTime--;
    }

    updateTimer(); // start met eerste display
    setInterval(updateTimer, 1000); // update elke seconde
});
