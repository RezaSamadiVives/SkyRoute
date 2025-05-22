function toggleReturnDate() {
    const retourRadio = document.getElementById('retour');
    const returnDateContainer = document.getElementById('returnDateContainer');
    returnDateContainer.style.display = retourRadio.checked ? 'block' : 'none';
}

function updateDestinationOptions() {
    const fromCitySelect = document.getElementById('fromCity');
    const toCitySelect = document.getElementById('toCity');
    const selectedFrom = fromCitySelect.value;

    for (let i = 0; i < toCitySelect.options.length; i++) {
        let option = toCitySelect.options[i];
        option.disabled = false;
        if (option.value === selectedFrom && option.value !== "") {
            option.disabled = true;
        }
    }

    if (toCitySelect.value === selectedFrom) {
        toCitySelect.value = "";
    }
}

document.addEventListener("DOMContentLoaded", function () {
    const departureInput = document.querySelector('input[name="DepartureDate"]');
    const returnInput = document.querySelector('input[name="ReturnDate"]');

    const today = new Date();
    const minDepartureDate = new Date(today.setDate(today.getDate() + 3));
    const minDepartureStr = minDepartureDate.toISOString().split('T')[0];
    departureInput.min = minDepartureStr;

    if (!departureInput.value) {
        departureInput.value = minDepartureStr;
    }

    departureInput.addEventListener('change', function () {
        const selectedDeparture = new Date(this.value);
        if (isNaN(selectedDeparture)) return;

        const minReturnDate = new Date(selectedDeparture);
        minReturnDate.setDate(minReturnDate.getDate() + 1);
        const minReturnStr = minReturnDate.toISOString().split('T')[0];
        returnInput.min = minReturnStr;

        if (new Date(returnInput.value) <= selectedDeparture) {
            returnInput.value = minReturnStr;
        }
    });

    if (departureInput.value) {
        const selectedDeparture = new Date(departureInput.value);
        const minReturnDate = new Date(selectedDeparture.setDate(selectedDeparture.getDate() + 1));
        returnInput.min = minReturnDate.toISOString().split('T')[0];
    }

    document.querySelector('form').addEventListener('submit', function (e) {
        if (!$(this).valid()) {
            return;
        }

        const submitContainer = document.getElementById('submitContainer');
        submitContainer.classList.add('loading');
    });


    
    toggleReturnDate();
});
