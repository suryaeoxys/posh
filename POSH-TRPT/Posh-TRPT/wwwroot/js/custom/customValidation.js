let valid = true;
var pattern = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/
const formValid = document.getElementById("#signInEmailId");
formValid.addEventListener("onkeypress", function validate(e) {
    e.preventDefault();
    if (!formValid.value || !pattern.test(formValid.value)) {
        const nameError = document.getElementById("nameError");
        nameError.classList.add("visible");
        loginEmail.classList.add("invalid");
        nameError.setAttribute("aria-hidden", false);
        nameError.setAttribute("aria-invalid", true);
        document.getElementById("nameError").style.color = "red";
        valid = false;
        console.log(valid);
    }
    else {
        const nameError = document.getElementById("nameError");
        nameError.classList.add("hidden");
        loginEmail.classList.add("valid");
        nameError.setAttribute("aria-hidden", true);
        nameError.setAttribute("aria-invalid", false);
        valid = true;
        console.log(valid);
    }
    return valid;
});
