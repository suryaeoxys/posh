let formValid = true;
const Emailregex = /^([a-zA-Z0-9_\.\-\+])+\@@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
const togglePassword = document.querySelector("#togglePassword");
const password = document.querySelector("#password");

togglePassword.addEventListener("click", function () {
	const type = password.getAttribute("type") === "password" ? "text" : "password";
	password.setAttribute("type", type);
	this.classList.toggle("bi-eye");
});
$('#signForm').submit(function () {


	var nam = document.getElementById('emailIdErrorr');
	var namm = document.getElementById('emailIdErrorr');
	var namp = document.getElementById('pswIdErrorr');
	var nam2 = document.getElementById('emailpswError');
	nam2.style.display = 'none';
	namm.style.display = 'none';
	namp.style.display = 'none';
	if (Emailregex.test(document.getElementById('signInEmailId').value)) {
		namm.style.display = 'none';
		nam.innerHTML = "Enter valid email address*";
		nam.style.display = 'block';
		nam.style.color = "red";
		document.getElementById('signInEmailId').addEventListener("submit", function (event) {
			event.preventDefault();
		});
		formValid = false;
		return formValid;
	}

	if ($.trim($("#signInEmailId").val()).length === 0 && $.trim($("#password").val()).length > 0) {
		namp.style.display = 'none';
		namm.innerHTML = "Email address is required*";
		namm.style.display = 'block';
		namm.style.color = "red";
		document.getElementById('signInEmailId').addEventListener("submit", function (event) {
			event.preventDefault();
		});
		formValid = false;

		return formValid;
	}
	else if ($.trim($("#signInEmailId").val()).length > 0 && $.trim($("#password").val()).length === 0) {
		namm.style.display = 'none';
		namp.innerHTML = "Password is required*";
		namp.style.display = 'block';
		namp.style.color = "red";
		document.getElementById('password').addEventListener("submit", function (event) {
			event.preventDefault();
		});
		formValid = false;

		return formValid;
	}

	else if ($.trim($("#signInEmailId").val()).length === 0 && $.trim($("#password").val()).length === 0) {
		namm.innerHTML = "Email address is required*";
		namm.style.display = 'block';
		namm.style.color = "red";
		namp.innerHTML = "Password is required*";
		namp.style.display = 'block';
		namp.style.color = "red";
		document.getElementById('password').addEventListener("submit", function (event) {
			event.preventDefault();
		});
		formValid = false;
		return formValid;
	}

	if ($.trim($("#password").val()).length === 0) {
		namp.innerHTML = "Password is required*";
		namp.style.display = 'block';
		namp.style.color = "red";
		document.getElementById('password').addEventListener("submit", function (event) {
			event.preventDefault();
		});
		formValid = false;
	}


	else {
		nam2.style.display = 'none';
		formValid = true;

	}

});