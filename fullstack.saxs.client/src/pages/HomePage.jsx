import React, { useEffect, useState } from 'react';
import { pingAuth } from '../api/SystemApi';

export default function HomePage() {
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        pingAuth()
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <div>Loading...</div>;

    return (
        <div>
            <h2>Welcome!</h2>
        </div>
    );
}