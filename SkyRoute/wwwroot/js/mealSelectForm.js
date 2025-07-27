document.addEventListener("DOMContentLoaded", function () {
    const mealInputs = document.querySelectorAll(".meal-radio");
    const continueButton = document.getElementById("continueBtn");

    const requiredMap = {};
    const selectedMap = {};

    mealInputs.forEach(input => {
        const pid = input.dataset.passengerId;
        const fid = input.dataset.flightId;

        if (!requiredMap[pid]) requiredMap[pid] = new Set();
        requiredMap[pid].add(fid);
    });

    function checkIfAllSelected() {
        for (let pid in requiredMap) {
            const requiredFlights = requiredMap[pid];
            const selectedFlights = selectedMap[pid] || new Set();
            if (selectedFlights.size !== requiredFlights.size) {
                continueButton.classList.add("disabled");
                return;
            }
        }
        continueButton.classList.remove("disabled");
    }

    mealInputs.forEach(input => {
        input.addEventListener("change", function () {
            if (this.checked) {
                const passengerId = this.dataset.passengerId;
                const flightId = parseInt(this.dataset.flightId);
                const mealId = parseInt(this.value);

                document.querySelectorAll(`input[name="${this.name}"]`).forEach(sibling => {
                    sibling.closest(".form-check").classList.remove("selected");
                });


                this.closest(".form-check").classList.add("selected");

                fetch("/MealOption/SaveMealOption", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "RequestVerificationToken": document.querySelector("input[name='__RequestVerificationToken']").value
                    },
                    body: JSON.stringify({
                        tempPassengerId: parseInt(passengerId),
                        flightId: flightId,
                        mealOptionId: mealId
                    })
                })
                    .then(res => res.json())
                    .then(data => {
                        showInfoModal(data.message, !data.success);
                        if (!data.success) {
                            showInfoModal(data.message || "Er is een fout opgetreden.");
                            return;
                        }

                        if (!selectedMap[passengerId]) selectedMap[passengerId] = new Set();
                        selectedMap[passengerId].add(flightId);
                        checkIfAllSelected();
                    })
                    .catch(err => {
                        console.error("Fout bij ajax:", err);
                        showInfoModal("Er is een technische fout opgetreden.", true);
                    });
            }
        });
    });

    function showInfoModal(message, isError = false) {
        const modalMessage = document.getElementById("messageModalBody");
        modalMessage.textContent = message;

        const modalElement = document.getElementById("messageModal");
        const modalTitle = modalElement.querySelector(".modal-title");
        const modalHeader = modalElement.querySelector(".modal-header");

        if (isError) {
            modalTitle.textContent = "Foutmelding";
            modalHeader.classList.remove("bg-success");
            modalHeader.classList.add("bg-danger");
        } else {
            modalTitle.textContent = "Succes";
            modalHeader.classList.remove("bg-danger");
            modalHeader.classList.add("bg-success");
        }

        const modal = new bootstrap.Modal(modalElement);
        modal.show();
    }


    checkIfAllSelected();
});
