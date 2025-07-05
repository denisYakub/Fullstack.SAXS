const REDIRECT_PAGE = 'https://localhost:7135/Identity/Account/Login';
const API_BASE = import.meta.env.VITE_API_URL || '';

export async function pingAuth() {
    const res = await fetch(`${API_BASE}/ping-auth`, {
        credentials: 'include',
    });

    if (res.status === 401) {
        window.location.href = REDIRECT_PAGE;
        throw new Error('Unauthorized');
    }

    if (!res.ok) {
        throw new Error('Network error');
    }
    return;
}

export async function createSystem(requestBody) {
    const res = await fetch(`${API_BASE}/api/saxs/systems`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(requestBody),
    });

    if (res.status === 401) {
        window.location.href = REDIRECT_PAGE;
        throw new Error('Unauthorized');
    }

    if (!res.ok) {
        const errorText = await res.text();
        throw new Error(errorText || 'Error while creating system');
    }

    return res;
}

export async function getAllGenerations() {
    const res = await fetch(`${API_BASE}/api/saxs/generations`, {
        credentials: 'include',
    });

    if (res.status === 401) {
        window.location.href = REDIRECT_PAGE;
        throw new Error('Unauthorized');
    }

    if (!res.ok) {
        throw new Error('Error loading generations');
    }

    return res.json();
}

export async function getMyGenerations() {
    const res = await fetch(`${API_BASE}/api/saxs/users/me/generations`, {
        credentials: 'include',
    });

    if (!res.ok) {
        if (res.status === 401) {
            window.location.href = REDIRECT_PAGE;
            throw new Error('Unauthorized');
        }
        throw new Error('Error loading your generations');
    }

    return res.json();
}

export async function getSystem(id) {
    const res = await fetch(`${API_BASE}/api/saxs/systems/${id}`)

    if (!res.ok) {
        if (res.status === 401) {
            window.location.href = REDIRECT_PAGE;
            throw new Error('Unauthorized');
        }
        throw new Error('Error loading your generations');
    }

    return res.json();
}

export async function openPhiGraph(id, layersNum = 5) {
    const res = await fetch(
        `${API_BASE}/api/saxs/systems/${id}/graphs/phi?layersNum=${layersNum}`,
        {
            method: 'POST',
            credentials: 'include',
        }
    );

    if (!res.ok) {
        if (res.status === 401) {
            window.location.href = REDIRECT_PAGE;
            throw new Error('Unauthorized');
        }
        throw new Error('Failed to generate graph');
    }

    const html = await res.text();
    const blob = new Blob([html], { type: 'text/html' });
    const url = URL.createObjectURL(blob);

    window.open(url, '_blank');

    // Откладываем удаление URL, чтобы окно успело загрузить содержимое
    setTimeout(() => URL.revokeObjectURL(url), 1000);
}