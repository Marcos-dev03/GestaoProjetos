<script>
	function togglePassword(inputId, button) {

		const input = document.getElementById(inputId);
	const icon = button.querySelector("i");

	if (input.type === "password") {

		input.type = "text";

	icon.classList.remove("bi-eye");
	icon.classList.add("bi-eye-slash");

	button.setAttribute("aria-label", "Ocultar senha");

		} else {

		input.type = "password";

	icon.classList.remove("bi-eye-slash");
	icon.classList.add("bi-eye");

	button.setAttribute("aria-label", "Mostrar senha");
		}
	}
</script>