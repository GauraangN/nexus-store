/**
 * NexusStore - Dynamic Interactivity & Cart Drawer Engine
 */

document.addEventListener('DOMContentLoaded', () => {
    initCartDrawer();
    initQuickAddButtons();
    initSearch();
});

// Toast notification helper
function showToast(message, type = 'success') {
    let container = document.getElementById('toastContainer');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toastContainer';
        container.className = 'toast-nexus-container';
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `toast-nexus ${type}`;
    const icon = type === 'success' ? '✓' : '!';
    toast.innerHTML = `<span style="font-weight: 800;">${icon}</span> <span>${message}</span>`;

    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateY(10px)';
        toast.style.transition = 'all 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, 3200);
}

// Cart Drawer open/close
function openCartDrawer() {
    const backdrop = document.getElementById('cartDrawerBackdrop');
    const drawer = document.getElementById('cartDrawer');
    if (backdrop && drawer) {
        refreshCartDrawer();
        backdrop.classList.add('active');
        drawer.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
}

function closeCartDrawer() {
    const backdrop = document.getElementById('cartDrawerBackdrop');
    const drawer = document.getElementById('cartDrawer');
    if (backdrop && drawer) {
        backdrop.classList.remove('active');
        drawer.classList.remove('active');
        document.body.style.overflow = '';
    }
}

function initCartDrawer() {
    const trigger = document.getElementById('cartTriggerBtn');
    const closeBtn = document.getElementById('cartDrawerCloseBtn');
    const backdrop = document.getElementById('cartDrawerBackdrop');

    if (trigger) trigger.addEventListener('click', openCartDrawer);
    if (closeBtn) closeBtn.addEventListener('click', closeCartDrawer);
    if (backdrop) backdrop.addEventListener('click', closeCartDrawer);
}

// Fetch and render cart drawer contents
async function refreshCartDrawer() {
    try {
        const response = await fetch('/Cart/GetCartData');
        if (!response.ok) return;
        const data = await response.json();

        // Update badge counters everywhere
        const badges = document.querySelectorAll('.cart-count-badge');
        badges.forEach(b => {
            b.textContent = data.totalItems;
            b.style.display = data.totalItems > 0 ? 'inline-block' : 'none';
        });

        const drawerBody = document.getElementById('cartDrawerItemsContainer');
        const drawerSubtotal = document.getElementById('drawerSubtotal');
        const drawerTotal = document.getElementById('drawerGrandTotal');
        const drawerCheckoutBtn = document.getElementById('drawerCheckoutBtn');

        if (!drawerBody) return;

        if (!data.items || data.items.length === 0) {
            drawerBody.innerHTML = `
                <div class="cart-empty-state">
                    <div class="cart-empty-icon">🛍️</div>
                    <h4 style="margin-bottom: 0.5rem; color: #0f172a;">Your cart is empty</h4>
                    <p style="font-size: 0.88rem; margin-bottom: 1.5rem;">Explore our curated gear and find something exceptional.</p>
                    <a href="/Shop" class="btn-nexus-primary" onclick="closeCartDrawer()">Start Shopping</a>
                </div>
            `;
            if (drawerSubtotal) drawerSubtotal.textContent = '$0.00';
            if (drawerTotal) drawerTotal.textContent = '$0.00';
            if (drawerCheckoutBtn) {
                drawerCheckoutBtn.classList.add('disabled');
                drawerCheckoutBtn.style.pointerEvents = 'none';
                drawerCheckoutBtn.style.opacity = '0.5';
            }
            return;
        }

        if (drawerCheckoutBtn) {
            drawerCheckoutBtn.classList.remove('disabled');
            drawerCheckoutBtn.style.pointerEvents = 'auto';
            drawerCheckoutBtn.style.opacity = '1';
        }

        let html = '';
        data.items.forEach(item => {
            html += `
                <div class="cart-item-row" id="drawer-item-${item.productId}">
                    <img src="${item.imageUrl}" alt="${item.productName}" class="cart-item-img" />
                    <div class="cart-item-info">
                        <div class="cart-item-name">${item.productName}</div>
                        <div class="cart-item-price">$${item.unitPrice.toFixed(2)}</div>
                        <div class="cart-item-qty-wrap">
                            <button type="button" class="cart-qty-btn" onclick="updateItemQty(${item.productId}, ${item.quantity - 1})">-</button>
                            <span class="cart-qty-val">${item.quantity}</span>
                            <button type="button" class="cart-qty-btn" onclick="updateItemQty(${item.productId}, ${item.quantity + 1})">+</button>
                        </div>
                    </div>
                    <button type="button" class="btn-cart-remove" onclick="removeDrawerItem(${item.productId})" title="Remove item">
                        ✕
                    </button>
                </div>
            `;
        });

        drawerBody.innerHTML = html;
        if (drawerSubtotal) drawerSubtotal.textContent = data.subtotal;
        if (drawerTotal) drawerTotal.textContent = data.grandTotal;

    } catch (err) {
        console.error('Error refreshing cart:', err);
    }
}

// Quick Add To Cart from product card or details
async function addToCart(productId, quantity = 1) {
    try {
        const formData = new FormData();
        formData.append('productId', productId);
        formData.append('quantity', quantity);
        formData.append('isAjax', 'true');

        const response = await fetch('/Cart/Add', {
            method: 'POST',
            body: formData,
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        });

        if (response.ok) {
            const data = await response.json();
            showToast('Item added to cart!', 'success');
            openCartDrawer();
        } else {
            showToast('Could not add item to cart', 'error');
        }
    } catch (err) {
        console.error(err);
        showToast('Network error adding to cart', 'error');
    }
}

async function updateItemQty(productId, quantity) {
    try {
        const formData = new FormData();
        formData.append('productId', productId);
        formData.append('quantity', quantity);
        formData.append('isAjax', 'true');

        const response = await fetch('/Cart/Update', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        if (response.ok) {
            await refreshCartDrawer();
            // If we are on the main /Cart page, reload page to sync full receipt
            if (window.location.pathname.toLowerCase().includes('/cart')) {
                window.location.reload();
            }
        }
    } catch (err) {
        console.error(err);
    }
}

async function removeDrawerItem(productId) {
    try {
        const formData = new FormData();
        formData.append('productId', productId);
        formData.append('isAjax', 'true');

        const response = await fetch('/Cart/Remove', {
            method: 'POST',
            body: formData,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        });

        if (response.ok) {
            showToast('Item removed from cart', 'success');
            await refreshCartDrawer();
            if (window.location.pathname.toLowerCase().includes('/cart')) {
                window.location.reload();
            }
        }
    } catch (err) {
        console.error(err);
    }
}

function initQuickAddButtons() {
    document.querySelectorAll('[data-quick-add]').forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            e.stopPropagation();
            const id = btn.getAttribute('data-quick-add');
            addToCart(id, 1);
        });
    });
}

function initSearch() {
    const searchInput = document.getElementById('navSearchInput');
    if (searchInput) {
        searchInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter' && searchInput.value.trim() !== '') {
                window.location.href = `/Shop?search=${encodeURIComponent(searchInput.value.trim())}`;
            }
        });
    }
}
