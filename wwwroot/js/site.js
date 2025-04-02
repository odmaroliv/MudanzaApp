/* MudanzaApp/wwwroot/js/site.js */

// Funci�n para copiar texto al portapapeles
window.copyToClipboard = function (text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
        return navigator.clipboard.writeText(text)
            .then(() => true)
            .catch(() => false);
    } else {
        // Fallback para navegadores que no soportan Clipboard API
        const textArea = document.createElement('textarea');
        textArea.value = text;
        textArea.style.position = 'fixed';
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();

        try {
            const successful = document.execCommand('copy');
            document.body.removeChild(textArea);
            return successful;
        } catch (err) {
            document.body.removeChild(textArea);
            return false;
        }
    }
};

// Funci�n para detectar si es un dispositivo m�vil
window.isMobileDevice = function () {
    return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
};

// Funci�n para obtener el idioma del navegador
window.getBrowserLanguage = function () {
    return navigator.language || navigator.userLanguage;
};

// Funci�n para abrir la c�mara
window.openCamera = async function (inputElementId) {
    const inputElement = document.getElementById(inputElementId);
    if (!inputElement) return false;

    // Verificar si el dispositivo tiene una c�mara
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        try {
            // Crear un contenedor para la previsualizaci�n de la c�mara
            let previewId = inputElementId + "_preview";
            let previewElement = document.getElementById(previewId);

            if (!previewElement) {
                previewElement = document.createElement("video");
                previewElement.id = previewId;
                previewElement.style.width = "100%";
                previewElement.style.maxWidth = "400px";
                previewElement.style.border = "1px solid #ccc";
                previewElement.style.borderRadius = "8px";
                previewElement.autoplay = true;

                inputElement.parentNode.insertBefore(previewElement, inputElement.nextSibling);
            }

            // Obtener el stream de la c�mara
            const stream = await navigator.mediaDevices.getUserMedia({ video: true });
            previewElement.srcObject = stream;

            // Crear bot�n de captura
            let captureButtonId = inputElementId + "_capture";
            let captureButton = document.getElementById(captureButtonId);

            if (!captureButton) {
                captureButton = document.createElement("button");
                captureButton.id = captureButtonId;
                captureButton.className = "btn btn-primary mt-2";
                captureButton.innerHTML = '<i class="fas fa-camera"></i> Capture Photo';

                previewElement.parentNode.insertBefore(captureButton, previewElement.nextSibling);
            }

            // Manejar clic en el bot�n de captura
            captureButton.onclick = () => {
                // Crear un canvas para capturar la imagen
                const canvas = document.createElement("canvas");
                canvas.width = previewElement.videoWidth;
                canvas.height = previewElement.videoHeight;

                // Dibujar el frame del video en el canvas
                const context = canvas.getContext("2d");
                context.drawImage(previewElement, 0, 0, canvas.width, canvas.height);

                // Convertir canvas a archivo
                canvas.toBlob((blob) => {
                    // Crear un objeto File
                    const file = new File([blob], "captured_image.jpg", { type: "image/jpeg" });

                    // Crear un objeto similar a FileList
                    const dataTransfer = new DataTransfer();
                    dataTransfer.items.add(file);

                    // Asignar el archivo al elemento input
                    inputElement.files = dataTransfer.files;

                    // Disparar evento change
                    const event = new Event("change", { bubbles: true });
                    inputElement.dispatchEvent(event);

                    // Limpiar
                    stream.getTracks().forEach(track => track.stop());
                    previewElement.remove();
                    captureButton.remove();
                }, "image/jpeg", 0.9);
            };

            return true;
        } catch (error) {
            console.error("Error accessing camera:", error);
            return false;
        }
    } else {
        return false;
    }
};

// Funci�n para generar un c�digo QR
window.generateQRCode = function (elementId, text) {
    if (!window.QRCode) {
        console.error("QRCode library not loaded");
        return false;
    }

    const element = document.getElementById(elementId);
    if (element) {
        // Si ya existe un canvas dentro del elemento, eliminarlo
        while (element.firstChild) {
            element.removeChild(element.firstChild);
        }

        // Generar el c�digo QR
        try {
            QRCode.toCanvas(element, text, { width: 200 }, function (error) {
                if (error) console.error(error);
            });
            return true;
        } catch (error) {
            console.error("Error generating QR code:", error);
            return false;
        }
    }
    return false;
};

// Funci�n para abrir en modo pantalla completa
window.openFullscreen = function (elementId) {
    const element = document.getElementById(elementId);
    if (!element) return false;

    if (element.requestFullscreen) {
        element.requestFullscreen();
    } else if (element.mozRequestFullScreen) { // Firefox
        element.mozRequestFullScreen();
    } else if (element.webkitRequestFullscreen) { // Chrome, Safari y Opera
        element.webkitRequestFullscreen();
    } else if (element.msRequestFullscreen) { // IE/Edge
        element.msRequestFullscreen();
    } else {
        return false;
    }

    return true;
};

// Funci�n para salir del modo pantalla completa
window.closeFullscreen = function () {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) { // Firefox
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) { // Chrome, Safari y Opera
        document.webkitExitFullscreen();
    } else if (document.msExitFullscreen) { // IE/Edge
        document.msExitFullscreen();
    } else {
        return false;
    }

    return true;
};

// Funci�n para mostrar notificaciones nativas del navegador
window.showNotification = function (title, body, icon) {
    // Verificar si el navegador soporta notificaciones
    if (!("Notification" in window)) {
        console.warn("Este navegador no soporta notificaciones de escritorio");
        return false;
    }

    // Verificar si ya tenemos permiso
    if (Notification.permission === "granted") {
        // Crear y mostrar la notificaci�n
        const notification = new Notification(title, {
            body: body,
            icon: icon || "/favicon.png"
        });
        return true;
    } else if (Notification.permission !== "denied") {
        // Pedir permiso
        Notification.requestPermission().then(function (permission) {
            if (permission === "granted") {
                const notification = new Notification(title, {
                    body: body,
                    icon: icon || "/favicon.png"
                });
                return true;
            }
        });
    }

    return false;
};

// Funci�n para inicializar tooltips de Bootstrap
window.initTooltips = function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
};

// Funci�n para mostrar u ocultar elementos HTML
window.toggleElementVisibility = function (elementId, show) {
    const element = document.getElementById(elementId);
    if (element) {
        element.style.display = show ? 'block' : 'none';
        return true;
    }
    return false;
};

// Inicializar cuando el documento est� listo
document.addEventListener('DOMContentLoaded', function () {
    // Inicializar tooltips
    window.initTooltips();

    // Otras inicializaciones si son necesarias
});