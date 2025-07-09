// Header Compact on Scroll
window.addEventListener('scroll', function () {
    const header = document.getElementById('nav_header');
    if (window.scrollY > 50) {
        header.classList.add('header-compact');
    } else {
        header.classList.remove('header-compact');
    }
});

// Scroll to Top Button
const scrollToTopBtn = document.getElementById('scrollToTopBtn');
window.addEventListener('scroll', () => {
    if (window.scrollY > 300) {
        scrollToTopBtn.classList.add('visible');
    } else {
        scrollToTopBtn.classList.remove('visible');
    }
});

scrollToTopBtn.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

// User Profile Navigation (Sidebar) Logic
const sidebarNavItems = document.querySelectorAll('.sidebar-nav-item');
const contentSections = document.querySelectorAll('.user-content-section');

sidebarNavItems.forEach(item => {
    item.addEventListener('click', function (event) {
        event.preventDefault();

        // Remove active class from all nav items
        sidebarNavItems.forEach(navItem => navItem.classList.remove('active'));
        // Add active class to the clicked item
        this.classList.add('active');

        // Hide all content sections
        contentSections.forEach(section => section.classList.remove('active'));

        // Show the target content section
        const targetId = this.dataset.target;
        const targetSection = document.getElementById(targetId);
        if (targetSection) {
            targetSection.classList.add('active');
            // Optional: scroll to the top of the content section
            targetSection.scrollIntoView({ behavior: 'smooth', block: 'start', inline: 'nearest' });
        }
    });
});

// Initialize: Ensure the first section (Account Info) is active on page load
document.addEventListener('DOMContentLoaded', () => {
    const initialActiveNav = document.querySelector('.sidebar-nav-item.active');
    if (initialActiveNav) {
        const targetId = initialActiveNav.dataset.target;
        const targetSection = document.getElementById(targetId);
        if (targetSection) {
            targetSection.classList.add('active');
        }
    } else {
        // Fallback: activate the first item if no active class is set
        if (sidebarNavItems.length > 0) {
            sidebarNavItems[0].classList.add('active');
            const targetId = sidebarNavItems[0].dataset.target;
            const targetSection = document.getElementById(targetId);
            if (targetSection) {
                targetSection.classList.add('active');
            }
        }
    }
});

// Simple form submission handler (for demonstration)
document.getElementById('account-form').addEventListener('submit', function (e) {
    e.preventDefault();
    alert('Thông tin tài khoản đã được cập nhật thành công!');
    // In a real application, you would send this data to a server
});