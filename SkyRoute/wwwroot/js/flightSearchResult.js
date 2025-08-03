document.addEventListener('DOMContentLoaded', function () {
    function selectFlight(segmentId) {
        updateSelection('outbound', segmentId);
        postSelection(segmentId, false); // false = geen retour
    }

    function selectReturnFlight(segmentId) {
        updateSelection('return', segmentId);
        postSelection(segmentId, true); // true = retour
    }

    function updateSelection(type, segmentId) {
        document.querySelectorAll(`.card-body[data-flight-type="${type}"]`).forEach(body => {
            body.classList.remove('selected');

            const checkbox = body.querySelector('input[type="checkbox"]');
            if (checkbox) {
                checkbox.disabled = false;
                checkbox.checked = false;
                checkbox.disabled = true;
            }

            const button = body.querySelector(`button[data-flight-type="${type}"]`);
            if (button) button.disabled = false;
        });

        const selectedBody = document.querySelector(`.card-body[data-flight-type="${type}"][data-segment-id="${segmentId}"]`);
        if (selectedBody) {
            selectedBody.classList.add('selected');

            const button = selectedBody.querySelector(`button[data-flight-type="${type}"]`);
            if (button) button.disabled = true;

            const checkbox = selectedBody.querySelector('input[type="checkbox"]');
            if (checkbox) {
                checkbox.disabled = false;
                checkbox.checked = true;
                checkbox.disabled = true;
            }
        }

        updateBookButtonState();
    }

    function updateBookButtonState() {
        const bookBtn = document.getElementById('bookFlightBtn');
        if (!bookBtn) return;

        const isRoundTrip = document.querySelectorAll('.card-body[data-flight-type="return"]').length > 0;
        const selectedOutbound = document.querySelector('.card-body[data-flight-type="outbound"].selected');
        const selectedReturn = document.querySelector('.card-body[data-flight-type="return"].selected');

        if (
            (!isRoundTrip && selectedOutbound) ||
            (isRoundTrip && selectedOutbound && selectedReturn)
        ) {
            bookBtn.classList.remove('disabled');
        } else {
            bookBtn.classList.add('disabled');
        }
    }

    function postSelection(segmentId, isRetour) {
        const isBusiness = document.querySelector('input[name="SelectedTripClass"]')?.value === 'Business';
        const adultCount = parseInt(document.querySelector('#AdultPassengers')?.value || "1");
        const kidsCount = parseInt(document.querySelector('#KidsPassengers')?.value || "0");

        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;


        fetch('/FlightSearch/Select', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify({
                segmentId: segmentId,
                isBusiness: isBusiness,
                isRetour: isRetour,
                adultPassengers: adultCount,
                kidsPassengers: kidsCount
            })
        })
            .then(res => {
                if (!res.ok) {
                    throw new Error("Netwerkfout tijdens selecteren van vlucht.");
                }
                return res.json();
            })
            .then(data => {
                if (!data.success) {
                    alert("Vlucht kon niet worden geselecteerd. Probeer opnieuw.");
                }
            })
            .catch(error => {
                console.error("Fout tijdens fetch:", error);
                alert("Er is een fout opgetreden bij het selecteren van de vlucht.");
            });
    }

    const bookFlightBtn = document.getElementById('bookFlightBtn');
    if (bookFlightBtn) {
        bookFlightBtn.addEventListener('click', function (e) {
            if (this.classList.contains('disabled')) {
                e.preventDefault();
            }
        });
    }

    document.querySelectorAll('.select-flight-btn').forEach(button => {
        button.addEventListener('click', function () {
            const segmentId = this.getAttribute('data-segment-id');
            const type = this.getAttribute('data-flight-type');

            sessionStorage.setItem("sessionActive", "true");
            
            if (type === 'outbound') {
                selectFlight(segmentId);
            } else if (type === 'return') {
                selectReturnFlight(segmentId);
            }

        });
    });
});
