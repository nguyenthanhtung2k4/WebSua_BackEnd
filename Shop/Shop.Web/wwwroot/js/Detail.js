// product-review.js (Fixed Version)

document.addEventListener('DOMContentLoaded', () => {
    // --- Header & Scroll-to-Top Logic ---
    const header = document.getElementById('nav_header');
    const scrollToTop = document.querySelector('.scroll-to-top');

    if (header && scrollToTop) {
        window.addEventListener('scroll', () => {
            header.classList.toggle('header-compact', window.scrollY > 50);
            scrollToTop.classList.toggle('visible', window.scrollY > 300);
        });

        scrollToTop.addEventListener('click', () => {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    // --- Product Quantity Automation ---
    const decreaseBtn = document.getElementById('decrease-quantity');
    const increaseBtn = document.getElementById('increase-quantity');
    const quantityInput = document.getElementById('product-quantity');

    if (decreaseBtn && quantityInput) {
        decreaseBtn.addEventListener('click', () => {
            let currentValue = parseInt(quantityInput.value || 1);
            if (currentValue > 1) {
                quantityInput.value = currentValue - 1;
            }
        });
    }

    if (increaseBtn && quantityInput) {
        increaseBtn.addEventListener('click', () => {
            let currentValue = parseInt(quantityInput.value || 1);
            quantityInput.value = currentValue + 1;
        });
    }

    // --- Product Image Gallery Automation ---
    const mainProductImage = document.getElementById('main-product-image');
    const thumbnails = document.querySelectorAll('.product-image-thumbnail');

    if (mainProductImage && thumbnails.length > 0) {
        thumbnails.forEach(thumbnail => {
            thumbnail.addEventListener('click', () => {
                thumbnails.forEach(t => t.classList.remove('active'));
                thumbnail.classList.add('active');
                mainProductImage.src = thumbnail.dataset.src;
            });
        });
    }

    // --- Review Star Rating Automation ---
    const stars = document.getElementById('stars');
    const selectedRatingInput = document.getElementById('selected-rating');
    let currentRating = 0;

    function highlightStars(count) {
        stars?.querySelectorAll('i').forEach((star, index) => {
            if (index < count) {
                star.classList.remove('ri-star-line', 'text-gray-400');
                star.classList.add('ri-star-fill', 'text-yellow-400');
            } else {
                star.classList.remove('ri-star-fill', 'text-yellow-400');
                star.classList.add('ri-star-line', 'text-gray-400');
            }
        });
    }

    stars?.addEventListener('mouseover', (e) => {
        if (e.target.tagName === 'I') {
            const value = parseInt(e.target.dataset.value);
            highlightStars(value);
        }
    });

    stars?.addEventListener('mouseout', () => {
        highlightStars(currentRating);
    });

    stars?.addEventListener('click', (e) => {
        if (e.target.tagName === 'I') {
            const value = parseInt(e.target.dataset.value);
            currentRating = value;
            selectedRatingInput.value = value;
            highlightStars(currentRating);
        }
    });

    // --- Toggle Section and Navigation ---
    const infoToggleBtn = document.getElementById('info-toggle');
    const infoContent = document.getElementById('info-content');
    const reviewsToggleBtn = document.getElementById('reviews-toggle');
    const reviewsContent = document.getElementById('reviews-content');

    const navToInfoBtn = document.getElementById('nav-to-info');
    const navToReviewsBtn = document.getElementById('nav-to-reviews');

    const sections = [
        {
            toggleButton: infoToggleBtn,
            contentDiv: infoContent,
            navButton: navToInfoBtn,
            id: 'product-info-section'
        },
        {
            toggleButton: reviewsToggleBtn,
            contentDiv: reviewsContent,
            navButton: navToReviewsBtn,
            id: 'product-review-section'
        }
    ];

    function openSection(sectionToOpen) {
        sections.forEach(section => {
            const { toggleButton, contentDiv, navButton } = section;
            const isActive = section === sectionToOpen;
            toggleButton?.classList.toggle('active', isActive);
            contentDiv?.classList.toggle('active', isActive);
            navButton?.classList.toggle('active', isActive);
        });
    }

    sections.forEach(section => {
        section.toggleButton?.addEventListener('click', () => {
            const isActive = section.contentDiv.classList.contains('active');
            if (isActive) {
                section.toggleButton.classList.remove('active');
                section.contentDiv.classList.remove('active');
                section.navButton.classList.remove('active');
            } else {
                openSection(section);
            }
        });
    });

    navToInfoBtn?.addEventListener('click', (e) => {
        e.preventDefault();
        openSection(sections[0]);
        smoothScrollTo(document.getElementById(sections[0].id));
    });

    navToReviewsBtn?.addEventListener('click', (e) => {
        e.preventDefault();
        openSection(sections[1]);
        smoothScrollTo(document.getElementById(sections[1].id));
    });

    function smoothScrollTo(element) {
        if (element) {
            const header = document.getElementById('nav_header');
            const headerOffset = header ? header.offsetHeight : 0;
            const elementPosition = element.getBoundingClientRect().top + window.pageYOffset;
            const offsetPosition = elementPosition - headerOffset - 20;
            window.scrollTo({ top: offsetPosition, behavior: "smooth" });
        }
    }

    // Mở mặc định phần Thông Tin Sản Phẩm
    openSection(sections[0]);
});
