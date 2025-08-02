     function removeFlightSegment(segmentType) {
            fetch(`/ShoppingCart/RemoveSegment?segment=${segmentType}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                }
            })
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    // Toon modal
                    var myModal = new bootstrap.Modal(document.getElementById('removeModal'));
                    myModal.show();

                    // Herlaad pagina na korte vertraging
                    setTimeout(() => location.reload(), 1500);
                }
            })
            .catch(error => console.error("Fout bij verwijderen:", error));
        }