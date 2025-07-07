document.addEventListener('DOMContentLoaded', () => {
    // --- Header & Scroll-to-Top Logic ---
    const header = document.getElementById('nav_header');
    const scrollToTop = document.querySelector('.scroll-to-top');

    window.addEventListener('scroll', () => {
        // Header compact effect
        header.classList.toggle('header-compact', window.scrollY > 50);
        // Show/hide scroll-to-top button
        scrollToTop.classList.toggle('visible', window.scrollY > 300);
    });

    // Smooth scroll to top when button is clicked
    scrollToTop.addEventListener('click', () => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    // --- Product Quantity Automation ---
    const decreaseBtn = document.getElementById('decrease-quantity');
    const increaseBtn = document.getElementById('increase-quantity');
    const quantityInput = document.getElementById('product-quantity');

    decreaseBtn.addEventListener('click', () => {
        let currentValue = parseInt(quantityInput.value);
        if (currentValue > 1) {
            quantityInput.value = currentValue - 1;
        }
    });

    increaseBtn.addEventListener('click', () => {
        let currentValue = parseInt(quantityInput.value);
        quantityInput.value = currentValue + 1;
    });

    // --- Product Image Gallery Automation ---
    const mainProductImage = document.getElementById('main-product-image');
    const thumbnails = document.querySelectorAll('.product-image-thumbnail');

    thumbnails.forEach(thumbnail => {
        thumbnail.addEventListener('click', () => {
            // Remove active class from all thumbnails
            thumbnails.forEach(t => t.classList.remove('active'));
            // Add active class to the clicked thumbnail
            thumbnail.classList.add('active');
            // Change main image source
            mainProductImage.src = thumbnail.dataset.src;
        });
    });

    // --- Review Star Rating Automation ---
    const stars = document.getElementById('stars');
    const selectedRatingInput = document.getElementById('selected-rating');
    let currentRating = 0; // To keep track of the selected rating

    // Function to highlight stars
    function highlightStars(count) {
        stars.querySelectorAll('i').forEach((star, index) => {
            if (index < count) {
                star.classList.remove('ri-star-line', 'text-gray-400');
                star.classList.add('ri-star-fill', 'text-yellow-400');
            } else {
                star.classList.remove('ri-star-fill', 'text-yellow-400');
                star.classList.add('ri-star-line', 'text-gray-400');
            }
        });
    }

    // Mouseover to show hover effect
    stars.addEventListener('mouseover', (e) => {
        if (e.target.tagName === 'I') {
            const value = parseInt(e.target.dataset.value);
            highlightStars(value);
        }
    });

    // Mouseout to revert to selected rating
    stars.addEventListener('mouseout', () => {
        highlightStars(currentRating);
    });

    // Click to set rating
    stars.addEventListener('click', (e) => {
        if (e.target.tagName === 'I') {
            const value = parseInt(e.target.dataset.value);
            currentRating = value; // Update selected rating
            selectedRatingInput.value = value; // Update hidden input
            highlightStars(currentRating); // Apply highlight
            console.log(`Selected rating: ${currentRating}`);
        }
    });

    // --- Handle Review Form Submission (Client-side simulation) ---
    const reviewForm = document.getElementById('review-form');
    reviewForm.addEventListener('submit', (e) => {
        e.preventDefault(); // Prevent default form submission

        const reviewerName = document.getElementById('reviewer-name').value.trim();
        const reviewMessage = document.getElementById('review-message').value.trim();
        const rating = parseInt(document.getElementById('selected-rating').value);

        if (!reviewerName || !reviewMessage || rating === 0) {
            alert('Vui lòng điền đầy đủ thông tin và chọn số sao đánh giá!');
            return;
        }

        const newReviewHtml = `
                    <div class="border-b border-gray-200 pb-6 mb-6">
                        <div class="flex justify-between items-center mb-2">
                            <h4 class="font-semibold text-lg">${reviewerName}</h4>
                            <div class="flex text-yellow-400">
                                ${'<i class="ri-star-fill"></i>'.repeat(rating)}
                                ${'<i class="ri-star-line"></i>'.repeat(5 - rating)}
                            </div>
                        </div>
                        <p class="text-gray-500 text-sm mb-2">Ngày: ${new Date().toLocaleDateString('vi-VN')}</p>
                        <p class="text-gray-700 leading-relaxed">${reviewMessage}</p>
                    </div>
                `;

        const reviewsContainer = document.querySelector('.reviews-container');
        reviewsContainer.insertAdjacentHTML('afterbegin', newReviewHtml); // Add new review at the top

        // Reset form and stars
        reviewForm.reset();
        currentRating = 0;
        selectedRatingInput.value = 0;
        highlightStars(0); // Reset star appearance
        alert('Đánh giá của bạn đã được gửi thành công!');

        // Optionally, scroll to the newly added review or the top of the reviews section
        reviewsContainer.scrollTop = 0;
    });

    const infoToggleBtn = document.getElementById('info-toggle');
    const infoContent = document.getElementById('info-content');
    const reviewsToggleBtn = document.getElementById('reviews-toggle');
    const reviewsContent = document.getElementById('reviews-content');

    const navToInfoBtn = document.getElementById('nav-to-info');
    const navToReviewsBtn = document.getElementById('nav-to-reviews');

    // Mảng chứa các cặp toggle để dễ quản lý
    const sections = [
        {
            toggleButton: infoToggleBtn,
            contentDiv: infoContent,
            navButton: navToInfoBtn,
            id: 'product-info-section' // ID của phần cha chứa cả button và content
        },
        {
            toggleButton: reviewsToggleBtn,
            contentDiv: reviewsContent,
            navButton: navToReviewsBtn,
            id: 'product-review-section'
        }
    ];

    // Hàm để mở một section cụ thể và đóng các section còn lại
    function openSection(sectionToOpen) {
        sections.forEach(section => {
            if (section === sectionToOpen) {
                // Mở section hiện tại
                section.toggleButton.classList.add('active');
                section.contentDiv.classList.add('active');
                section.navButton.classList.add('active');
            } else {
                // Đóng các section còn lại
                section.toggleButton.classList.remove('active');
                section.contentDiv.classList.remove('active');
                section.navButton.classList.remove('active');
            }
        });
    }

    // Gán sự kiện click cho các nút toggle bên trong
    sections.forEach(section => {
        section.toggleButton.addEventListener('click', () => {
            // Nếu section hiện tại đang active, thì đóng nó lại (chỉ khi click trực tiếp vào toggle button)
            if (section.contentDiv.classList.contains('active')) {
                section.toggleButton.classList.remove('active');
                section.contentDiv.classList.remove('active');
                section.navButton.classList.remove('active');
            } else {
                // Nếu không, mở section này và đóng các section khác
                openSection(section);
            }
        });
    });

    // Gán sự kiện click cho các nút điều hướng (nav buttons)
    navToInfoBtn.addEventListener('click', (e) => {
        e.preventDefault();
        openSection(sections[0]); // Mở phần thông tin
        const element = document.getElementById(sections[0].id);
        smoothScrollTo(element);
    });

    navToReviewsBtn.addEventListener('click', (e) => {
        e.preventDefault();
        openSection(sections[1]); // Mở phần đánh giá
        const element = document.getElementById(sections[1].id);
        smoothScrollTo(element);
    });

    // Hàm cuộn mượt mà (giữ nguyên từ code trước)
    function smoothScrollTo(element) {
        if (element) {
            const header = document.getElementById('nav_header'); // Đảm bảo header được định nghĩa
            const headerOffset = header ? header.offsetHeight : 0;
            const elementPosition = element.getBoundingClientRect().top + window.pageYOffset;
            const offsetPosition = elementPosition - headerOffset - 20; // Thêm khoảng đệm

            window.scrollTo({
                top: offsetPosition,
                behavior: "smooth"
            });
        }
    }


    // Khởi tạo: Mở 'Thông Tin Sản Phẩm' và kích hoạt nút điều hướng của nó khi tải trang
    openSection(sections[0]);



})
