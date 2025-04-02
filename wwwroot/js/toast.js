// Toast functionality
window.showToast = (type, message, title, duration) => {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) return;

    // Create toast element
    const toastElement = document.createElement('div');
    toastElement.className = `max-w-xs w-full shadow-lg rounded-lg pointer-events-auto overflow-hidden transform transition-all duration-300 ease-in-out opacity-0 translate-y-2`;

    // Add appropriate styling based on type
    let bgClass, iconClass;
    switch (type) {
        case 'success':
            bgClass = 'bg-green-50 dark:bg-green-900/20 border-l-4 border-green-500';
            iconClass = 'fas fa-check-circle text-green-500';
            break;
        case 'error':
            bgClass = 'bg-red-50 dark:bg-red-900/20 border-l-4 border-red-500';
            iconClass = 'fas fa-times-circle text-red-500';
            break;
        case 'warning':
            bgClass = 'bg-amber-50 dark:bg-amber-900/20 border-l-4 border-amber-500';
            iconClass = 'fas fa-exclamation-triangle text-amber-500';
            break;
        case 'info':
        default:
            bgClass = 'bg-blue-50 dark:bg-blue-900/20 border-l-4 border-blue-500';
            iconClass = 'fas fa-info-circle text-blue-500';
            break;
    }

    toastElement.classList.add(...bgClass.split(' '));

    // Create the inner content
    const contentHTML = `
        <div class="p-4">
            <div class="flex items-start">
                <div class="flex-shrink-0">
                    <i class="${iconClass} text-lg"></i>
                </div>
                <div class="ml-3 w-0 flex-1">
                    ${title ? `<p class="text-sm font-medium text-gray-900 dark:text-white">${title}</p>` : ''}
                    <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">${message}</p>
                </div>
                <div class="ml-4 flex-shrink-0 flex">
                    <button class="inline-flex text-gray-400 hover:text-gray-500 focus:outline-none">
                        <span class="sr-only">Close</span>
                        <i class="fas fa-times text-sm"></i>
                    </button>
                </div>
            </div>
        </div>
    `;

    toastElement.innerHTML = contentHTML;

    // Add close functionality to the button
    const closeButton = toastElement.querySelector('button');
    closeButton.addEventListener('click', () => {
        removeToast(toastElement);
    });

    // Add toast to container
    toastContainer.appendChild(toastElement);

    // Trigger animation after a small delay (to ensure the DOM has updated)
    setTimeout(() => {
        toastElement.classList.remove('opacity-0', 'translate-y-2');
    }, 10);

    // Auto-remove after duration
    if (duration > 0) {
        setTimeout(() => {
            removeToast(toastElement);
        }, duration);
    }

    // Function to remove toast with animation
    function removeToast(element) {
        element.classList.add('opacity-0', 'translate-y-2');

        // Remove from DOM after animation completes
        setTimeout(() => {
            if (element.parentNode === toastContainer) {
                toastContainer.removeChild(element);
            }
        }, 300); // Matches the duration in the CSS class
    }
};