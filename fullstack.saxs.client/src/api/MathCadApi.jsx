const REDIRECT_PAGE = import.meta.env.VITE_REDIRECT_URL + ':' + import.meta.env.VITE_REDIRECT_PORT + '/Identity/Account/Login';
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

export async function GetAtomsCoord(id) {
    const res = await fetch(
        `${API_BASE}/api/mathcad/systems/${id}/atoms-coordinates`,
        {
            credentials: 'include',
        }
    );

    checkResponse(res);

    const blob = await res.blob();
    const url = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = 'atoms_coordinates.csv';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    window.URL.revokeObjectURL(url);
}

export async function GetQI(requestBody) {
    const res = await fetch(
        `${API_BASE}/api/mathcad/q-values`,
        {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestBody)
        }
    );

    checkResponse(res);

    const data = await res.json();

    // ����������� ������ � �����: ������ ������� � ����� ������
    const textContent = data.join('\n');

    // ������� Blob �� ������
    const blob = new Blob([textContent], { type: 'text/plain' });

    // ������� ��������� URL ��� ����������
    const url = URL.createObjectURL(blob);

    // ������� ������ ��� ����������
    const a = document.createElement('a');
    a.href = url;
    a.download = 'q_values.txt'; // ��� �����
    document.body.appendChild(a);
    a.click();

    // ������� ������ � ����������� ������
    document.body.removeChild(a);
    URL.revokeObjectURL(url);

    return;
}