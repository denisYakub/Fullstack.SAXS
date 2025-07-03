import React, { useEffect, useState } from 'react';
import { pingAuth } from '../api/SystemApi';

export default function HomePage() {
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        pingAuth()
            .catch(() => {
                window.location.href = 'https://localhost:7135/Identity/Account/Login';
            })
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <div>Loading...</div>;

    return (
        <div>
            <h2>Welcome!</h2>
        </div>
    );
}