const API_URL = "http://localhost:5000/api";

// Manejar login
document.getElementById('loginForm')?.addEventListener('submit', async function (e) {
    e.preventDefault();
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const res = await fetch(`${API_URL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Email: email, Password: password })
    });

    const data = await res.json();
    if (res.ok) {
        localStorage.setItem('token', data.Token);
        window.location.href = 'dashboard.html';
    } else {
        alert('Error al iniciar sesión');
    }
});

// Manejar registro
document.getElementById('registerForm')?.addEventListener('submit', async function (e) {
    e.preventDefault();
    const username = document.getElementById('username').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const res = await fetch(`${API_URL}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Username: username, Email: email, Password: password })
    });

    if (res.ok) {
        alert('Registro exitoso. Inicia sesión.');
        window.location.href = 'login.html';
    } else {
        alert('Error al registrarse');
    }
});

// Cerrar sesión
function logout() {
    localStorage.removeItem('token');
    window.location.href = 'index.html';
}
