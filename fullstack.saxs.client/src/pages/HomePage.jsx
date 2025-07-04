import React, { useEffect, useState } from 'react';
import { pingAuth } from '../api/SystemApi';

export default function HomePage() {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        pingAuth()
            .then(() => setLoading(false))
            .catch(err => {
                setError(err.message || 'Error loading');
                setLoading(false);
            });
    }, []);

    if (error)
        return (
            <div className="min-h-screen flex justify-center items-center bg-red-900 text-red-300 text-lg font-semibold p-6">
                Error: {error}
            </div>
        );

    if (loading)
        return (
            <div className="flex justify-center items-center min-h-screen bg-gray-900 text-white text-xl">
                Loading...
            </div>
        );

    return (
        <div className="min-h-screen flex flex-col justify-center items-center bg-gradient-to-r from-purple-800 via-indigo-900 to-blue-900 text-white px-4">
            <h2 className="text-4xl font-bold mb-6 select-none">Welcome!</h2>
            <p className="text-lg max-w-xl text-center">
                This is your dashboard. Use the navigation menu to create and manage your systems.
            </p>
        </div>
    );
}