  document.getElementById("ToestemmingCheckbox").addEventListener("change", function () {
            const btn = document.getElementById("BetalingButton");
            btn.disabled = !this.checked;
        });