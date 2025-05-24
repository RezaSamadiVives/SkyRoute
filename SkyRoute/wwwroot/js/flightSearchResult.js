function selectFlight(segmentId) {
    // Deselecteer alle checkboxes en card-bodies voor heenvluchten
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

    // Selecteer de juiste
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
}

function selectReturnFlight(segmentId) {
    // Deselecteer alle terugvluchten
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

    // Selecteer de juiste
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
}