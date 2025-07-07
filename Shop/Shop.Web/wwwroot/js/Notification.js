// JavaScript để điều khiển thông báo chào mừng
        document.addEventListener('DOMContentLoaded', function() {
            const messageElement = document.getElementById('welcomeMessage');
            const greetingTextElement = document.getElementById('greetingText');
            const closeBtn = messageElement.querySelector('.close-btn');
            const progressBar = document.getElementById('progressBar'); // Lấy element của progress bar

            let timeoutId; // Biến để lưu trữ ID của setTimeout chính
            let hideTimeoutId; // Biến để lưu trữ ID của setTimeout ẩn cuối cùng

            // Hàm để hiển thị thông báo
            function showWelcomeMessage() {
                // Reset progress bar trước khi hiển thị
                progressBar.style.transition = 'width 0s linear'; // Đặt lại transition về 0 để reset
                progressBar.style.width = '100%'; // Đặt lại width về 100%
                
                messageElement.classList.remove('hidden');
                // Sử dụng setTimeout nhỏ để đảm bảo trình duyệt đã render lại width: 100% trước khi thêm transition
                setTimeout(() => {
                    messageElement.classList.add('show');
                    progressBar.style.transition = 'width 5s linear'; // Kích hoạt transition 5s
                    progressBar.style.width = '0%'; // Bắt đầu chạy về 0
                }, 50); // Chờ 50ms để reset xong

                // Tự động ẩn sau 5 giây (sau khi progress bar chạy hết)
                timeoutId = setTimeout(function() {
                    messageElement.classList.remove('show');
                    hideTimeoutId = setTimeout(() => {
                        messageElement.classList.add('hidden');
                    }, 500); // Thời gian của transition đóng (0.5s)
                }, 5000); // 5000 milliseconds = 5 giây
            }

            // Hàm để ẩn thông báo ngay lập tức
            function hideWelcomeMessageImmediately() {
                clearTimeout(timeoutId); // Hủy setTimeout tự động ẩn
                clearTimeout(hideTimeoutId); // Hủy setTimeout ẩn cuối cùng nếu đang chạy

                messageElement.classList.remove('show');
                progressBar.style.transition = 'width 0.1s linear'; // Ngừng transition của progress bar nhanh chóng
                progressBar.style.width = '100%'; // Reset lại thanh progress ngay lập tức
                
                setTimeout(() => {
                    messageElement.classList.add('hidden');
                }, 500); // Đợi cho transition đóng (0.5s) hoàn tất
            }

            // Xử lý nút đóng 'x'
            closeBtn.addEventListener('click', function() {
                hideWelcomeMessageImmediately();
            });

            // --- Thêm nút để bạn dễ dàng thử nghiệm ---
            const showBtn = document.getElementById('showWelcomeBtn');
            const hideBtn = document.getElementById('hideWelcomeBtn');

            if (showBtn) {
                showBtn.addEventListener('click', function() {
                    hideWelcomeMessageImmediately(); // Ẩn thông báo cũ trước khi hiển thị mới
                    // setTimeout để đảm bảo ẩn hoàn tất trước khi show lại
                    setTimeout(showWelcomeMessage, 600); // Chờ 0.6s để hiệu ứng ẩn xong
                });
            }

            if (hideBtn) {
                hideBtn.addEventListener('click', function() {
                    hideWelcomeMessageImmediately();
                });
            }

            // Gọi hàm showWelcomeMessage khi trang được tải lần đầu
            showWelcomeMessage(); 
        });

// JavaScript thông báo đăng nhập
document.addEventListener('DOMContentLoaded', function () {
    const loginNotification = document.getElementById('login-notification');
    const closeLoginNotificationButton = document.getElementById('close-login-notification');
    const showLoginNotificationButton = document.getElementById('show-login-notification');
    const body = document.body; // Reference to the body element

    // Function to show the notification
    function showNotification() {
        loginNotification.classList.remove('hidden');
        body.classList.add('no-scroll'); // Add class to body to prevent scrolling

        // Focus on the first interactive element inside the modal for accessibility
        // This is typically the close button or the primary action button
        setTimeout(() => {
            loginNotification.classList.add('show');
            closeLoginNotificationButton.focus(); // Focus on the close button when opened
        }, 10);
    }

    // Function to hide the notification
    function hideNotification() {
        loginNotification.classList.remove('show');
        loginNotification.addEventListener('transitionend', function handler() {
            loginNotification.classList.add('hidden');
            body.classList.remove('no-scroll'); // Remove class from body to re-enable scrolling
            loginNotification.removeEventListener('transitionend', handler);
        }, { once: true });
    }

    // Close when clicking the "Đóng" button
    closeLoginNotificationButton.addEventListener('click', hideNotification);

    // Close when clicking outside the modal content
    loginNotification.addEventListener('click', function (event) {
        if (event.target === loginNotification) {
            hideNotification();
        }
    });

    // Close with Escape key for accessibility
    document.addEventListener('keydown', function (event) {
        if (event.key === 'Escape' && !loginNotification.classList.contains('hidden')) {
            hideNotification();
        }
    });

    // Example trigger: When the "Add to cart" button is clicked
    body.addEventListener('click', showNotification);
});