export async function pingAuth() {
    const res = await fetch('ping-auth', {
        credentials: 'include',
    });

    if (res.status === 401) {
        // Не авторизован
        throw new Error('Unauthorized');
    }

    if (!res.ok) {
        throw new Error('Network error');
    }
    return;
}

export async function createSystem(requestBody) {
    const res = await fetch('api/saxs/sys', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        credentials: 'include',
        body: JSON.stringify(requestBody),
    });

    if (!res.ok) {
        const errorText = await res.text();
        throw new Error(errorText || 'Error while creating system');
    }

    return res;
}

export async function getAllGenerations() {
    const res = await fetch('api/saxs/gen', {
        credentials: 'include',
    });

    if (!res.ok) {
        throw new Error('Error loading generations');
    }

    return res.json();
}

export async function getMyGenerations() {
    const res = await fetch('api/saxs/gen/my', {
        credentials: 'include',
    });

    if (!res.ok) {
        if (res.status === 401) {
            throw new Error('Unauthorized');
        }
        throw new Error('Error loading your generations');
    }

    return res.json();
}

export async function getSystem(id) {
    const res = await fetch(`/api/saxs/sys/${id}`)

    if (!res.ok) {
        if (res.status === 401) {
            throw new Error('Unauthorized');
        }
        throw new Error('Error loading your generations');
    }

    return res.json();
}