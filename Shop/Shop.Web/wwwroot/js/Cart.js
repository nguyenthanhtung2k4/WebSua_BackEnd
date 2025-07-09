document.addEventListener('DOMContentLoaded', () => {
    const cartItemsTableBody = document.querySelector('#cart-table tbody');
    const totalItemsSelectedSpan = document.getElementById('total-items-selected');
    const cartTotalSelectedSpan = document.getElementById('cart-total-selected');
    const emptyCartMessage = document.getElementById('empty-cart-message');
    const cartTable = document.getElementById('cart-table');
    const historyList = document.getElementById('history-list');
    const emptyHistoryMessage = document.getElementById('empty-history-message');
    const toast = document.getElementById('toast');

    // Khởi tạo giỏ hàng và lịch sử mua hàng là mảng rỗng (không dùng localStorage)
    let cart = [];
    let purchaseHistory = [];

    // Loại bỏ hàm saveCart() và savePurchaseHistory() vì không cần lưu trữ cục bộ
    function showToast(message) {
        toast.textContent = message;
        toast.classList.add('show');
        setTimeout(() => {
            toast.classList.remove('show');
        }, 3000);
    }

    // Debounce function to limit rapid updates
    function debounce(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }

    function updateCartDisplay() {
        cartItemsTableBody.classList.add('fade-out');
        cartTable.classList.add('loading');
        setTimeout(() => {
            cartItemsTableBody.innerHTML = '';
            if (cart.length === 0) {
                emptyCartMessage.style.display = 'block';
                cartTable.style.display = 'none';
            } else {
                emptyCartMessage.style.display = 'none';
                cartTable.style.display = 'table';
                cart.forEach(item => {
                    const row = document.createElement('tr');
                    row.className = 'hover:bg-gray-50 fade-in';
                    row.setAttribute('aria-label', `Sản phẩm ${item.name}`);
                    row.innerHTML = `
                                <td class="px-3 py-4 whitespace-nowrap">
                                    <input type="checkbox" class="product-checkbox" data-id="${item.id}" ${item.selected ? 'checked' : ''} aria-label="Chọn sản phẩm ${item.name}">
                                </td>
                                <td class="px-3 py-4 whitespace-nowrap text-sm text-gray-900">
                                    ${item.id}
                                </td>
                                <td class="px-3 py-4 whitespace-nowrap">
                                    <div class="flex items-center">
                                        <div class="flex-shrink-0 h-16 w-16">
                                            <img class="h-16 w-16 rounded-md object-cover" src="${item.image}" alt="${item.name}">
                                        </div>
                                        <div class="ml-4">
                                            <div class="text-base font-medium text-dark-blue">${item.name}</div>
                                        </div>
                                    </div>
                                </td>
                                <td class="px-3 py-4 whitespace-nowrap text-lg font-medium text-primary">
                                    ${item.price.toLocaleString('vi-VN')} VNĐ
                                </td>
                                <td class="px-3 py-4 whitespace-nowrap">
                                    <div class="quantity-control">
                                        <button class="decrease-quantity-btn" data-id="${item.id}" aria-label="Giảm số lượng ${item.name}">
                                            <i class="ri-subtract-line"></i>
                                        </button>
                                        <input type="number" value="${item.quantity}" min="1" readonly class="quantity-input" data-id="${item.id}" aria-label="Số lượng ${item.name}">
                                        <button class="increase-quantity-btn" data-id="${item.id}" aria-label="Tăng số lượng ${item.name}">
                                            <i class="ri-add-line"></i>
                                        </button>
                                    </div>
                                </td>
                                <td class="px-3 py-4 whitespace-nowrap text-lg font-semibold text-dark-blue">
                                    <span class="item-total">
                                        ${(item.price * item.quantity).toLocaleString('vi-VN')}
                                    </span> VNĐ
                                </td>
                                <td class="px-3 py-4 whitespace-nowrap text-right text-sm font-medium">
                                    <button class="remove-item-btn text-red-500 hover:text-red-700 transition-colors duration-200" data-id="${item.id}" aria-label="Xóa ${item.name} khỏi giỏ hàng">
                                        <i class="ri-delete-bin-line ri-lg"></i>
                                    </button>
                                </td>
                            `;
                    cartItemsTableBody.appendChild(row);
                });
            }
            cartTable.classList.remove('loading');
            cartItemsTableBody.classList.remove('fade-out');
            updateCartSummary();
        }, 300);
    }

    function updateCartSummary() {
        let totalItemsSelected = 0;
        let cartTotalSelected = 0;

        cart.forEach(item => {
            if (item.selected) {
                totalItemsSelected += item.quantity;
                cartTotalSelected += item.price * item.quantity;
            }
        });

        totalItemsSelectedSpan.textContent = totalItemsSelected;
        cartTotalSelectedSpan.textContent = `${cartTotalSelected.toLocaleString('vi-VN')} VNĐ`;
    }

    function displayPurchaseHistory() {
        historyList.classList.add('fade-out');
        historyList.classList.add('loading');
        setTimeout(() => {
            historyList.innerHTML = '';
            if (purchaseHistory.length === 0) {
                emptyHistoryMessage.style.display = 'block';
            } else {
                emptyHistoryMessage.style.display = 'none';
                purchaseHistory.forEach(order => {
                    const orderDiv = document.createElement('div');
                    orderDiv.className = 'border border-gray-200 rounded-lg p-4 bg-light-blue fade-in';
                    orderDiv.setAttribute('aria-label', `Đơn hàng ${order.orderId}`);
                    let itemsHtml = order.items.map(item => `
                                <div class="flex justify-between items-center text-sm text-gray-700 mb-1">
                                    <span>- ${item.name} (x${item.quantity})</span>
                                    <span>${(item.price * item.quantity).toLocaleString('vi-VN')} VNĐ</span>
                                </div>
                            `).join('');
                    orderDiv.innerHTML = `
                                <div class="flex justify-between items-center mb-3 pb-2 border-b border-gray-300">
                                    <h3 class="text-lg font-semibold text-dark-blue">Đơn hàng: ${order.orderId}</h3>
                                    <span class="text-sm text-gray-500">${order.date}</span>
                                </div>
                                <div class="mb-3">
                                    ${itemsHtml}
                                </div>
                                <div class="flex justify-between items-center pt-2 border-t border-gray-300">
                                    <span class="text-base font-medium">Tổng cộng:</span>
                                    <span class="text-xl font-bold text-primary">${order.total.toLocaleString('vi-VN')} VNĐ</span>
                                </div>
                            `;
                    historyList.appendChild(orderDiv);
                });
            }
            historyList.classList.remove('loading');
            historyList.classList.remove('fade-out');
        }, 300);
    }

    // Debounced quantity update functions
    const updateQuantity = debounce((id, change) => {
        const item = cart.find(i => i.id === id);
        if (item) {
            if (change === 'increase') {
                item.quantity++;
            } else if (change === 'decrease' && item.quantity > 1) {
                item.quantity--;
            }
            updateCartDisplay(); // Chỉ cập nhật hiển thị, không lưu vào localStorage
            showToast(`${change === 'increase' ? 'Tăng' : 'Giảm'} số lượng ${item.name}`);
        }
    }, 300);

    // Event delegation for cart actions
    cartItemsTableBody.addEventListener('click', (event) => {
        const target = event.target.closest('.increase-quantity-btn, .decrease-quantity-btn, .remove-item-btn, .product-checkbox');
        if (!target) return;

        const id = target.dataset.id;
        if (target.classList.contains('increase-quantity-btn')) {
            updateQuantity(id, 'increase');
        } else if (target.classList.contains('decrease-quantity-btn')) {
            updateQuantity(id, 'decrease');
        } else if (target.classList.contains('remove-item-btn')) {
            const item = cart.find(i => i.id === id);
            if (item && confirm(`Bạn có chắc muốn xóa "${item.name}" khỏi giỏ hàng?`)) {
                cart = cart.filter(i => i.id !== id);
                updateCartDisplay(); // Chỉ cập nhật hiển thị, không lưu vào localStorage
                showToast(`Đã xóa ${item.name} khỏi giỏ hàng`);
            }
        } else if (target.classList.contains('product-checkbox')) {
            const item = cart.find(i => i.id === id);
            if (item) {
                item.selected = target.checked;
                updateCartSummary(); // Chỉ cập nhật hiển thị, không lưu vào localStorage
            }
        }
    });

    // Event delegation for adding products to cart
    document.querySelector('.grid').addEventListener('click', (event) => {
        const button = event.target.closest('.add-to-cart-btn');
        if (!button) return;

        const id = button.dataset.id;
        const name = button.dataset.name;
        const price = parseInt(button.dataset.price);
        const image = button.dataset.image;

        const existingItem = cart.find(item => item.id === id);
        if (existingItem) {
            existingItem.quantity++;
        } else {
            cart.push({
                id,
                name,
                price,
                image,
                quantity: 1,
                selected: true
            });
        }
        updateCartDisplay(); // Chỉ cập nhật hiển thị, không lưu vào localStorage
        showToast(`Đã thêm ${name} vào giỏ hàng`);
    });

    // Checkout with loading state
    document.getElementById('checkout-btn').addEventListener('click', () => {
        const checkoutBtn = document.getElementById('checkout-btn');
        checkoutBtn.classList.add('loading');
        checkoutBtn.disabled = true;

        setTimeout(() => {
            const selectedItems = cart.filter(item => item.selected);
            if (selectedItems.length === 0) {
                showToast('Vui lòng chọn ít nhất một sản phẩm để thanh toán!');
                checkoutBtn.classList.remove('loading');
                checkoutBtn.disabled = false;
                return;
            }

            const newOrderId = 'DH' + (purchaseHistory.length + 1).toString().padStart(3, '0');
            const currentDate = new Date();
            const formattedDate = `${currentDate.getDate().toString().padStart(2, '0')}/${(currentDate.getMonth() + 1).toString().padStart(2, '0')}/${currentDate.getFullYear()}`;
            let totalAmountForCheckout = 0;

            const checkoutItems = selectedItems.map(item => {
                totalAmountForCheckout += item.price * item.quantity;
                return {
                    id: item.id,
                    name: item.name,
                    price: item.price,
                    quantity: item.quantity
                };
            });

            const newOrder = {
                orderId: newOrderId,
                date: formattedDate,
                total: totalAmountForCheckout,
                items: checkoutItems
            };

            purchaseHistory.push(newOrder);
            // Không lưu purchaseHistory vào localStorage
            cart = cart.filter(item => !item.selected);
            updateCartDisplay(); // Chỉ cập nhật hiển thị giỏ hàng
            displayPurchaseHistory(); // Chỉ cập nhật hiển thị lịch sử mua hàng
            showToast(`Đơn hàng ${newOrderId} đã được thanh toán thành công với tổng số tiền ${totalAmountForCheckout.toLocaleString('vi-VN')} VNĐ!`);
            checkoutBtn.classList.remove('loading');
            checkoutBtn.disabled = false;
        }, 500);
    });

    // Keyboard navigation for accessibility
    document.addEventListener('keydown', (event) => {
        if (event.key === 'Enter' && event.target.classList.contains('product-checkbox')) {
            event.target.click();
        }
    });

    // Initial display
    updateCartDisplay();
    displayPurchaseHistory();
});