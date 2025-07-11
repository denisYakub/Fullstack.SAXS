import Area from "../context/Area";

const REDIRECT_PAGE = 'https://localhost:7135/Identity/Account/Login';
const API_BASE = import.meta.env.VITE_API_URL || '';

function checkResponse(res) {
    if (res.status === 401) {
        window.location.href = REDIRECT_PAGE;
    }
    else if (!res.ok) {
        console.error(res)
        window.location.href = '/service-unavailable';
    }
}

export async function pingAuth() {
    const res = await fetch(`${API_BASE}/ping-auth`, {
        credentials: 'include',
    });

    checkResponse(res);

    return;
}

export async function createSystem(requestBody) {
    const res = await fetch(`${API_BASE}/api/saxs/systems`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(requestBody),
    });

    checkResponse(res);

    return res;
}

export async function getAllGenerations() {
    const res = await fetch(`${API_BASE}/api/saxs/generations`, {
        credentials: 'include',
    });

    checkResponse(res);

    return res.json();
}

export async function getMyGenerations() {
    const res = await fetch(`${API_BASE}/api/saxs/users/me/generations`, {
        credentials: 'include',
    });

    checkResponse(res);

    return res.json();
}

export async function getSystem(id) {
    const res = await fetch(`${API_BASE}/api/saxs/systems/${id}`)

    checkResponse(res);

    const json = await res.json();

    return new Area(json.OuterRadius, json.Particles);;
}

export async function openPhiGraph(id, layersNum = 5) {
    const res = await fetch(
        `${API_BASE}/api/saxs/systems/${id}/graphs/phi?layersNum=${layersNum}`,
        {
            method: 'POST',
            credentials: 'include',
        }
    );

    checkResponse(res);

    const html = await res.text();
    const blob = new Blob([html], { type: 'text/html' });
    const url = URL.createObjectURL(blob);

    window.open(url, '_blank');

    // Откладываем удаление URL, чтобы окно успело загрузить содержимое
    setTimeout(() => URL.revokeObjectURL(url), 1000);
}