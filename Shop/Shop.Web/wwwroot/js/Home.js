// Slider functionality
const slider = document.querySelector('.slider');
const sliderItems = document.querySelectorAll('.slider-item');
const dotsContainer = document.querySelector('.dots-container');
const dots = document.querySelectorAll('.dot');
const prevBtn = document.querySelector('.slider-prev');
const nextBtn = document.querySelector('.slider-next');
let currentIndex = 0;
const totalSlides = sliderItems.length;
let slideInterval;

function goToSlide(index) {
    if (index < 0) {
        index = totalSlides - 1;
    } else if (index >= totalSlides) {
        index = 0;
    }
    currentIndex = index;
    slider.style.transform = `translateX(-${currentIndex * 100}%)`;
    dots.forEach((dot, i) => {
        dot.classList.toggle('active', i === currentIndex);
    });
}

function startSlideInterval() {
    clearInterval(slideInterval); // Clear existing interval before starting a new one
    slideInterval = setInterval(() => {
        goToSlide(currentIndex + 1);
    }, 5000); // 5 seconds
}

prevBtn.addEventListener('click', () => {
    goToSlide(currentIndex - 1);
    startSlideInterval(); // Reset interval
});
nextBtn.addEventListener('click', () => {
    goToSlide(currentIndex + 1);
    startSlideInterval(); // Reset interval
});

dots.forEach((dot, i) => {
    dot.addEventListener('click', () => {
        goToSlide(i);
        startSlideInterval(); // Reset interval
    });
});

startSlideInterval(); // Start automatic sliding on load

// Scroll animation
const fadeElements = document.querySelectorAll('.fade-in, .fade-up');
const scrollToTop = document.querySelector('.scroll-to-top');
const header = document.querySelector('header');
let navHeader = document.getElementById("nav_header");



const observerOptions = {
    root: null,
    rootMargin: '0px',
    threshold: 0.3 // Trigger when 10% of the element is visible
};

const observer = new IntersectionObserver((entries, observer) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            // Add a small delay for staggered effect, especially for .fade-up elements
            const delay = entry.target.classList.contains('fade-up') ? 100 : 0;
            setTimeout(() => {
                entry.target.classList.add('visible');
                // observer.unobserve(entry.target); // Uncomment if you want the animation to run only once
            }, delay);
        } else {
            // Optionally remove 'visible' class if element scrolls out of view
            // entry.target.classList.remove('visible');
        }
    });
}, observerOptions);

fadeElements.forEach(element => {
    observer.observe(element);
});

// Scroll to top functionality
scrollToTop.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

//    code nay  la  code  bo sung 
const menu = document.getElementById('menu');
function checkFade() {

    // Header animation
    const currentScrollTop = window.pageYOffset;
    if (currentScrollTop > 70) {
        if (currentScrollTop > lastScrollTop) {
            // Scrolling down
            header.style.transform = 'translateY(-100%)';
            menu.style.transform = 'translateX(-100%)';
        } else {
            // Scrolling up
            menu.style.transform = 'translateX(0)';
            menu.style.padding = '0.5rem 0';
            header.style.transform = 'translateY(0)';
            header.style.padding = '0.5rem 0';
        }
    } else {
        header.style.transform = 'translateY(0)';
        header.style.padding = '1rem 0';
    }
    lastScrollTop = currentScrollTop;
}
// Scroll to top functionality
scrollToTop.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});
// Check elements on load
setTimeout(checkFade, 1000);
// Check elements on scroll
window.addEventListener('scroll', checkFade);



// Header sticky/compact logic
let lastScrollY = window.scrollY;
window.addEventListener('scroll', () => {
    // Code goc 

    if (window.scrollY > 70) { // When scrolled past 70px
        header.classList.remove('header-compact');
    } else {
        header.classList.add('header-compact');
    }




    // Show/hide scroll to top button
    if (window.scrollY > 300) { // Show after scrolling 300px
        scrollToTop.classList.add('visible');
    } else {
        scrollToTop.classList.remove('visible');
    }

    lastScrollY = window.scrollY;
});











// Mobile menu toggle (already present, ensured it's functional)
const menuButton = document.querySelector('button.md\\:hidden'); // Select the correct button
const mobileMenu = document.querySelector('header nav'); // Select the nav element

menuButton.addEventListener('click', () => {
    mobileMenu.classList.toggle('hidden');
    // Add or remove classes for styling the mobile menu when it's shown
    mobileMenu.classList.toggle('flex');
    mobileMenu.classList.toggle('flex-col');
    mobileMenu.classList.toggle('absolute');
    mobileMenu.classList.toggle('top-16');
    mobileMenu.classList.toggle('left-0');
    mobileMenu.classList.toggle('right-0');
    mobileMenu.classList.toggle('bg-white');
    mobileMenu.classList.toggle('shadow-md');
    mobileMenu.classList.toggle('p-4');
    mobileMenu.classList.toggle('z-40'); /* Z-index slightly lower than header */
    mobileMenu.classList.toggle('items-center'); /* Center align items */
    mobileMenu.classList.toggle('space-x-8'); /* Remove horizontal spacing */
    mobileMenu.classList.toggle('space-y-4'); /* Add vertical spacing for links */