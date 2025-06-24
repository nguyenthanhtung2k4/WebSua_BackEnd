// wwwroot/js/AccountScript.js

document.addEventListener('DOMContentLoaded', () => {
    const loginToggle = document.getElementById('loginToggle');
    const registerToggle = document.getElementById('registerToggle');
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const switchToRegister = document.getElementById('switchToRegister');
    const switchToLogin = document.getElementById('switchToLogin');
    const formToggleContainer = document.querySelector('.form-toggle-container');
    const formWrapper = document.querySelector('.form-wrapper');


  //Ham  Register 
    const passInputReg = document.querySelector('.passInput-Reg');
    const passInputReg2 = document.querySelector('.passInput-Reg2');
    const passCheckReg = document.querySelector('.passCheck-Reg'); 

    // --- Password Length 
    passInputReg.addEventListener('input', function () {
        const passwordValue = passInputReg.value; 

  
        if (passwordValue.length > 0 && passwordValue.length < 6) {
            passCheckReg.textContent = "Mật khẩu phải lớn hơn 6 kí tự!";
            passCheckReg.style.color = "red";
        } else {
           
            passCheckReg.textContent = "";
            passCheckReg.style.color = "";
        }

        validatePasswordMatch();
    });

    passInputReg2.addEventListener('input', function () {
        validatePasswordMatch(); 
    });
    function validatePasswordMatch() {
        const passwordValue = passInputReg.value; 
        const confirmPasswordValue = passInputReg2.value; 
        if (confirmPasswordValue.length > 0 && passwordValue !== confirmPasswordValue) {
            passCheckReg.textContent = "Mật khẩu và xác nhận mật khẩu không khớp.";
            passCheckReg.style.color = "red";
        } else if (confirmPasswordValue.length > 0 && passwordValue === confirmPasswordValue) {
           
            passCheckReg.textContent = "";
            passCheckReg.style.color = "";
        }
       
    }


    // Ham check  login 
    const passCheck = document.querySelector('.passCheck');
    const passInput = document.querySelector('.passInput');

    passInput.addEventListener('input', function () {
        const l = passInput.value;
        if (l.length <= 6 && l.length >= 1) {
            passCheck.textContent = "Nhập mật khẩu lớn hơn 6 kí tự !";
        } else {
            passCheck.textContent = "";
        }
    });
    // Hàm cập nhật chiều cao của form-wrapper
    function updateFormWrapperHeight(form) {
        formWrapper.style.height = `${form.scrollHeight}px`;
    }

    // Hàm chuyển đổi form với hiệu ứng trượt
    function switchForms(formToShow, formToHide, toggleButtonActive, toggleButtonInactive) {
        if (formToShow.classList.contains('active')) {
            updateFormWrapperHeight(formToShow);
            return;
        }

        formToHide.style.transform = (formToShow === loginForm) ? 'translateX(100%)' : 'translateX(-100%)';
        formToHide.style.opacity = '0';
        formToHide.style.pointerEvents = 'none';

        if (toggleButtonActive === registerToggle) {
            formToggleContainer.classList.add('register-active');
        } else {
            formToggleContainer.classList.remove('register-active');
        }

        setTimeout(() => {
            formToHide.classList.remove('active');

            formToShow.classList.add('active');
            formToShow.style.transform = 'translateX(0)';
            formToShow.style.opacity = '1';
            formToShow.style.pointerEvents = 'auto';

            updateFormWrapperHeight(formToShow);
        }, 300);

        toggleButtonActive.classList.add('active');
        toggleButtonInactive.classList.remove('active');
    }

    // Gắn sự kiện cho các nút toggle
    loginToggle.addEventListener('click', (e) => {
        e.preventDefault();
        switchForms(loginForm, registerForm, loginToggle, registerToggle);
    });

    registerToggle.addEventListener('click', (e) => {
        e.preventDefault();
        switchForms(registerForm, loginForm, registerToggle, loginToggle);
    });

    // Gắn sự kiện cho các liên kết "Chưa có tài khoản?" và "Đã có tài khoản?"
    switchToRegister.addEventListener('click', (e) => {
        e.preventDefault();
        switchForms(registerForm, loginForm, registerToggle, loginToggle);
    });

    switchToLogin.addEventListener('click', (e) => {
        e.preventDefault();
        switchForms(loginForm, registerForm, loginToggle, registerToggle);
    });

    // --- Logic khởi tạo trạng thái ban đầu và xử lý thông báo lỗi/thành công ---

    // Lấy trạng thái từ ViewBag/TempData (có thể không tồn tại nếu không có lỗi/thành công)
    const hasLoginError = document.querySelector('#loginForm .message.error');
    const hasRegisterError = document.querySelector('#registerForm .message.error');
    const hasSuccessMessage = document.querySelector('.message.success');

    if (hasRegisterError) {
        // Nếu có lỗi trong form đăng ký, tự động chuyển sang form đăng ký
        switchForms(registerForm, loginForm, registerToggle, loginToggle);
    } else if (hasSuccessMessage) {
        // Nếu có thông báo thành công (thường là sau khi đăng ký), chuyển về form đăng nhập
        switchForms(loginForm, registerForm, loginToggle, registerToggle);
    } else {
        // Mặc định hiển thị form đăng nhập khi tải trang
        switchForms(loginForm, registerForm, loginToggle, registerToggle);
    }

    // Đảm bảo chiều cao form-wrapper được cập nhật sau khi xác định form hiển thị
    // Chờ một chút để DOM ổn định sau khi chuyển form nếu có
    setTimeout(() => {
        const currentActiveForm = document.querySelector('.auth-form.active');
        if (currentActiveForm) {
            updateFormWrapperHeight(currentActiveForm);
        }
    }, 50); // Khoảng trễ nhỏ để đảm bảo form đã được render đủ
});