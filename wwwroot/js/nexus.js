/**
 * NexusStore - Dynamic Athletic D2C Interactivity & Cart Drawer Engine
 */

document.addEventListener('DOMContentLoaded', () => {
    initCartDrawer();
    initMobileMenu();
    initQuickAddButtons();
    initSearch();
    initSizeSelectors();
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
        const shippingProgressContainer = document.getElementById('drawerFreeShippingBar');

        // Parse numerical subtotal from currency string (e.g., "$120.00" -> 120.00)
        let subtotalNum = 0;
        if (data.subtotal) {
            subtotalNum = parseFloat(data.subtotal.replace(/[^0-9.-]+/g, '')) || 0;
        }

        // Render Free Shipping Progress Bar
        if (shippingProgressContainer) {
            const threshold = 100.0;
            const progress = Math.min(100, Math.round((subtotalNum / threshold) * 100));
            const remaining = Math.max(0, threshold - subtotalNum);

            if (subtotalNum >= threshold && data.totalItems > 0) {
                shippingProgressContainer.innerHTML = `
                    <div class="free-shipping-text">
                        <span>🎉 <strong>FREE EXPRESS SHIPPING UNLOCKED!</strong></span>
                        <span class="text-success fw-bold">100%</span>
                    </div>
                    <div class="free-shipping-track">
                        <div class="free-shipping-fill unlocked" style="width: 100%;"></div>
                    </div>
                `;
            } else {
                shippingProgressContainer.innerHTML = `
                    <div class="free-shipping-text">
                        <span>Add <strong>$${remaining.toFixed(2)}</strong> for <strong>Free Shipping</strong></span>
                        <span>${progress}%</span>
                    </div>
                    <div class="free-shipping-track">
                        <div class="free-shipping-fill" style="width: ${progress}%;"></div>
                    </div>
                `;
            }
        }

        if (!drawerBody) return;

        if (!data.items || data.items.length === 0) {
            drawerBody.innerHTML = `
                <div class="cart-empty-state">
                    <div class="cart-empty-icon">🛍️</div>
                    <h4 style="margin-bottom: 0.5rem; color: #0a0c10; font-family: var(--font-heading); font-weight: 800; text-transform: uppercase;">Your bag is empty</h4>
                    <p style="font-size: 0.88rem; margin-bottom: 1.5rem; color: #667085;">Explore our curated performance gear and elevate your daily kit.</p>
                    <a href="/Shop" class="btn-nexus-primary" onclick="closeCartDrawer()">Start Shopping →</a>
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
                            <button type="button" class="cart-qty-btn" onclick="updateItemQty(${item.productId}, ${item.quantity - 1})">−</button>
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
            showToast('Added to your bag!', 'success');
            openCartDrawer();
        } else {
            showToast('Could not add item', 'error');
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
            showToast('Item removed', 'success');
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

// Size / Spec selector toggle
function initSizeSelectors() {
    document.querySelectorAll('.size-pill-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            document.querySelectorAll('.size-pill-btn').forEach(b => b.classList.remove('active'));
            btn.classList.add('active');
            const sizeLabel = document.getElementById('selectedSizeLabel');
            if (sizeLabel) {
                sizeLabel.textContent = btn.getAttribute('data-size') || btn.textContent.trim();
            }
        });
    });
}

// Interactive filter for Home Showcase
function filterShowcase(categorySlug, btnEl) {
    document.querySelectorAll('.showcase-tab-btn').forEach(b => b.classList.remove('active'));
    if (btnEl) btnEl.classList.add('active');

    const cards = document.querySelectorAll('.product-showcase-item');
    cards.forEach(card => {
        const cardCat = card.getAttribute('data-category');
        if (categorySlug === 'all' || cardCat === categorySlug) {
            card.style.display = 'flex';
            card.style.opacity = '1';
        } else {
            card.style.display = 'none';
        }
    });
}

// Mobile Menu Off-Canvas Drawer Logic
function initMobileMenu() {
    const btn = document.getElementById('mobileMenuBtn');
    const closeBtn = document.getElementById('mobileMenuCloseBtn');
    const backdrop = document.getElementById('mobileMenuBackdrop');
    const drawer = document.getElementById('mobileMenuDrawer');

    if (btn) {
        btn.addEventListener('click', () => {
            if (backdrop && drawer) {
                backdrop.classList.add('active');
                drawer.classList.add('active');
                document.body.style.overflow = 'hidden';
            }
        });
    }

    const closeMobileMenu = () => {
        if (backdrop && drawer) {
            backdrop.classList.remove('active');
            drawer.classList.remove('active');
            document.body.style.overflow = '';
        }
    };

    if (closeBtn) closeBtn.addEventListener('click', closeMobileMenu);
    if (backdrop) backdrop.addEventListener('click', closeMobileMenu);
}

// Mobile Shop Filter Toggle
function toggleMobileFilters() {
    const sidebar = document.querySelector('.filter-sidebar');
    if (sidebar) {
        sidebar.classList.toggle('mobile-open');
    }
}

