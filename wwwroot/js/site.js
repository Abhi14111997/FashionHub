// FashionHub JS
document.addEventListener('DOMContentLoaded', function() {
    // Payment option selection
    document.querySelectorAll('.pay-option').forEach(option => {
        option.addEventListener('click', function() {
            document.querySelectorAll('.pay-option').forEach(o => o.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Wishlist button
    document.querySelectorAll('.wishlist-btn').forEach(btn => {
        btn.addEventListener('click', function(e) {
            e.preventDefault();
            this.style.color = '#FF3F6C';
            this.innerHTML = '<i class="fas fa-heart"></i>';
        });
    });

    // Pincode check
    const pincodeInput = document.querySelector('.delivery-check input');
    const pincodeBtn = document.querySelector('.delivery-check button');
    if (pincodeBtn) {
        pincodeBtn.addEventListener('click', function() {
            if (pincodeInput.value.length === 6) {
                pincodeBtn.textContent = '✅ Delivery Available!';
                pincodeBtn.style.background = '#26a541';
            } else {
                alert('Please enter a valid 6-digit pincode');
            }
        });
    }

    // Success page confetti effect
    if (document.querySelector('.success-card')) {
        const card = document.querySelector('.success-card');
        card.style.animation = 'slideUp 0.6s ease';
    }
});
