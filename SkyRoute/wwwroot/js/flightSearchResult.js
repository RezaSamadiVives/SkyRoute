document.addEventListener('DOMContentLoaded', function () {
    function selectFlight(segmentId) {
        document.querySelectorAll('.card-body[data-flight-type="outbound"]').forEach(body => {
            body.classList.remove('selected');

            const checkbox = body.querySelector('input[type="checkbox"]');
            if (checkbox) {
                checkbox.disabled = false;
                checkbox.checked = false;
                checkbox.disabled = true;
            }

            const button = body.querySelector('button[data-flight-type="outbound"]');
            if (button) button.disabled = false;
        });

        const selectedBody = document.querySelector(`.card-body[data-segment-id="${segmentId}"]`);
        if (selectedBody) {
            selectedBody.classList.add('selected');

            const button = selectedBody.querySelector('button[data-flight-type="outbound"]');
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

    function selectReturnFlight(segmentId) {
        document.querySelectorAll('.card-body[data-flight-type="return"]').forEach(body => {
            body.classList.remove('selected');

            const checkbox = body.querySelector('input[type="checkbox"]');
            if (checkbox) {
                checkbox.disabled = false;
                checkbox.checked = false;
                checkbox.disabled = true;
            }

            const button = body.querySelector('button[data-flight-type="return"]');
            if (button) button.disabled = false;
        });

        const selectedBody = document.querySelector(`.card-body[data-segment-id="${segmentId}"]`);
        if (selectedBody) {
            selectedBody.classList.add('selected');

            const button = selectedBody.querySelector('button[data-flight-type="return"]');
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

            if (type === 'outbound') {
                selectFlight(segmentId);
            } else if (type === 'return') {
                selectReturnFlight(segmentId);
            }
        });
    });
});
