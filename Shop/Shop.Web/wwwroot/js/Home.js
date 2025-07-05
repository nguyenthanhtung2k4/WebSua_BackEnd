document.addEventListener('DOMContentLoaded', () => {
    // Scroll-to-top button
    const scrollToTop = document.createElement('div');
    scrollToTop.className = 'scroll-to-top';
    scrollToTop.innerHTML = '<i class="ri-arrow-up-line ri-lg"></i>';
    document.body.appendChild(scrollToTop);

    window.addEventListener('scroll', () => {
        scrollToTop.classList.toggle('visible', window.scrollY > 300);
    });

    scrollToTop.addEventListener('click', () => {
        window.scrollTo({ top: 0, behavior: 'smooth' });
    });

    // Status Carousel Logic
    const statusItems = document.querySelectorAll('#status-carousel .status-item');
    let currentIndex = 0;

    function showStatus(index) {
        statusItems.forEach((item, i) => {
            if (i === index) {
                item.classList.add('active');
            } else {
                item.classList.remove('active');
            }
        });
    }

    function nextStatus() {
        currentIndex = (currentIndex + 1) % statusItems.length;
        showStatus(currentIndex);
    }

    // Show the first status initially
    showStatus(currentIndex);

    // Auto-advance status every 5 seconds (adjust as needed)
    setInterval(nextStatus, 5000); // 5 seconds

    // Intersection Observer for fade-in animations
    const fadeInElements = document.querySelectorAll('.fade-in-up');
    const observerOptions = {
        root: null, // viewport as the root
        rootMargin: '0px',
        threshold: 0.1 // Trigger when 10% of the element is visible
    };

    const fadeInObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                observer.unobserve(entry.target); // Stop observing once it's visible
            }
        });
    }, observerOptions);

    fadeInElements.forEach(element => {
        fadeInObserver.observe(element);
    });

    // Header compact on scroll
    const header = document.getElementById('nav_header');
    window.addEventListener('scroll', () => {
        header.classList.toggle('header-compact', window.scrollY > 50);
    });
});

// ///  SP ----  Check  sp lien quan
// Smooth scrolling for navigation links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();

        document.querySelector(this.getAttribute('href')).scrollIntoView({
            behavior: 'smooth'
        });
    });
});

// Fade-up animation on scroll
const fadeUpElements = document.querySelectorAll('.fade-up');
const observerOptions = {
    root: null,
    rootMargin: '0px',
    threshold: 0.1
};

const observer = new IntersectionObserver((entries, observer) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('visible');
            observer.unobserve(entry.target);
        }
    });
}, observerOptions);

fadeUpElements.forEach(el => observer.observe(el));

// Scroll-to-top button
const scrollToTopButton = document.querySelector('.scroll-to-top');

window.addEventListener('scroll', () => {
    if (window.scrollY > 200) {
        scrollToTopButton.classList.add('visible');
    } else {
        scrollToTopButton.classList.remove('visible');
    }
});

scrollToTopButton.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

// Header shrink on scroll
const header = document.getElementById('nav_header');
window.addEventListener('scroll', () => {
    if (window.scrollY > 50) {
        header.classList.add('header-compact');
    } else {
        header.classList.remove('header-compact');
    }
});

// Product filtering (from previous version, assuming it's still desired)
const filterButtons = document.querySelectorAll('.filter-btn');
filterButtons.forEach(button => {
    button.addEventListener('click', () => {
        filterButtons.forEach(btn => btn.classList.remove('active'));
        button.classList.add('active');
        const filter = button.getAttribute('data-filter');
        const products = document.querySelectorAll('.product-item');

        products.forEach(product => {
            const category = product.getAttribute('data-category');
            product.style.display = (filter === 'Tất cả' || category === filter) ? 'block' : 'none';
        });
    });
});
